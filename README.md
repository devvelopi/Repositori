# Repositori
A usage of the repository pattern to abstract data access using LINQ independent of the data source

#### Regarding Repository Pattern
Repositories, being the hotly debated topic that they are, are not for everyone, and is down to personal preference.

The concept behind this library is to be a generic abstraction of data sources in the infrastructure layer.

Tying the power of C# generics with the brilliant tool that is LINQ, it is very
possible to create clean, data-source independent code.

Repositories can be used to wrap:
 - Rest endpoints (I.e. CRUD operations)
 - SQL databases (I.e. EntityFramework)
 - NoSQL databases (I.e. DynamoDb)
 - File storage (I.e. S3)
 - and many other data sources

## Installation

```bash
dotnet add package Repositori.Core
```

## Changelog

See [Changelog](./CHANGELOG.md)

## Usage

#### Creating Custom Repositories
Primary use involves creating custom repositories using the `IRepository` interface. The example below is a simple repository built around a generic list.
```c#
public class ListRepository<TEntity, TIdentifier> : IRepository<TEntity, TIdentifier> where TEntity : IIdentifiable<TIdentifier>
{
  private readonly List<TEntity> _entities;

    public ListRepository(List<TEntity> entities)
    {
        _entities = entities;
    }

    public TEntity GetBy(Expression<System.Func<TEntity, bool>> filter) =>
        _entities.AsQueryable().SingleOrDefault(filter);

    public Task<TEntity> GetByAsync(Expression<System.Func<TEntity, bool>> filter) =>
        Task.Run(() => GetBy(filter));

    public ICollection<TEntity> ListBy(Expression<System.Func<TEntity, bool>> filter, int skip, int take) =>
        _entities.AsQueryable().Where(filter).Skip(skip).Take(take).ToList();

    public Task<ICollection<TEntity>> ListByAsync(Expression<System.Func<TEntity, bool>> filter, int skip, int take) =>
        Task.Run(() => ListBy(filter, skip, take));

    /// <inheritdoc />
    public TEntity Create(TEntity entity)
    {
        _entities.Add(entity);
        return entity;
    }

    /// <inheritdoc />
    public async Task<TEntity> CreateAsync(TEntity entity) => await Task.Run(() => Create(entity));

    /// <inheritdoc />
    public List<TEntity> Create(ICollection<TEntity> entities)
    {
        _entities.AddRange(entities);
        return entities.ToList();
    }

    /// <inheritdoc />
    public async Task<List<TEntity>> CreateAsync(ICollection<TEntity> entities) =>
        await Task.Run(() => Create(entities));

    /// <inheritdoc />
    public TEntity Update(TEntity entity)
    {
        var index = _entities.FindIndex(e => e.Equals(entity));
        if (index <= -1) return default;
        _entities[index] = entity;
        return entity;
    }

    /// <inheritdoc />
    public async Task<TEntity> UpdateAsync(TEntity entity) => await Task.Run(() => Update(entity));

    /// <inheritdoc />
    public List<TEntity> Update(ICollection<TEntity> entities)
    {
        entities.ToList().ForEach(e => Update(e));
        return entities.ToList();
    }

    /// <inheritdoc />
    public async Task<List<TEntity>> UpdateAsync(ICollection<TEntity> entities) =>
        await Task.Run(() => Update(entities));

    /// <inheritdoc />
    public TEntity Delete(TEntity entity)
    {
        _entities.Remove(entity);
        return entity;
    }

    /// <inheritdoc />
    public async Task<TEntity> DeleteAsync(TEntity entity) => await Task.Run(() => Delete(entity));

    /// <inheritdoc />
    public List<TEntity> Delete(ICollection<TEntity> entities)
    {
        foreach(var entity in entities)
            _entities.Remove(entity);
        return entities.ToList();
    }

    /// <inheritdoc />
    public async Task<List<TEntity>> DeleteAsync(ICollection<TEntity> entities) => await Task.Run(() => Delete(entities));

    /// <inheritdoc />
    public ICollection<TEntity> DeleteBy(Expression<System.Func<TEntity, bool>> filter) {
        var toDelete = _entities.AsQueryable().Where(filter).ToList();
        foreach (var entity in toDelete) Delete(entity);
        return toDelete;
    }

    /// <inheritdoc />
    public Task<ICollection<TEntity>> DeleteByAsync(Expression<System.Func<TEntity, bool>> filter) =>
        Task.Run(() => DeleteBy(filter));

    /// <inheritdoc />
    public async Task StartTransactionAsync()
    {
    }

    /// <inheritdoc />
    public async Task CommitTransactionAsync()
    {
    }

    /// <inheritdoc />
    public async Task RollbackTransactionAsync()
    {
    }
  }
}
```

#### Consumption of Custom Repositories
Usage is simple. Even more simple with dependency injection. Using the previously created example:
```c#
public class DataModel : IIdentifiable<string>
{
    public string Id { get; set; }
    public string SomeField { get; set; }
}

...

public class Program
{
    public async Task DoThings()
    {
        var repository = new ListRepository<DataModel, string>(new List<DataModel>
            {new DataModel {Id = "1"}, new DataModel {Id = "2"}});
        var data = await repository.GetByIdAsync("1");
        var query = repository.Query.Where(e => e.SomeField.Equals("SomeValue"));
    }
}
```

#### Consumption of Repository Subsets
Additionally, the `IRepository` interface is a combination of several subsets of functionality reflecting CRUD, namely:
- `ICreateRepository`
- `IReadRepository`
- `IUpdateRepository`
- `IDeleteRepository`
- `ITransactionalRepository`

The reasoning for separation is from a usability standpoint.

Consider the case where you require `readonly` access to a data source, this is now trivial:
```c#
public class SomeService 
{
    private readonly IReadRepository _repository;
    public SomeService(IReadRepository repository)
    {
        _repository = repository;
    }
}
```

## Works well with

### Other Repositori Implementations

See other common Repositori implementations:
- [For Entity Framework Core](https://github.com/jaseaman/Repositori.EntityFrameworkCore)
- [For RESTful Services](https://github.com/jaseaman/Repositori.Rest)

### Custom LINQ Providers

Writing custom LINQ providers is indeed possible [if a little difficult](https://weblogs.asp.net/mehfuzh/writing-custom-linq-provider).

By writing custom providers, it is possible to abstract ANY data-source,
behind the great pre-existing LINQ functionality.

### Uniti
A shameless plug for my other project [Uniti](https://github.com/jaseaman/Uniti), an implementation of the Unit of Work pattern which allows 
transactional functionality across all operations, not just limited to the database.

Such as the pseudo code below.

```c#

public class MockRepository<TEntity, TIdentifier> : IRepository<TEntity, TIdentifier> 
{
    protected readonly IUnitOfWorkBuilder _uow;
    
    public MockRepository() 
    {
        _uow = new UnitOfWorkBuilder();
    }

    ...
    /// <inheritdoc />
    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        _uow.Add(
        async () => 
        { 
            //  Create functionality
        },
        async () => 
        {
            // Rollback create functionality
            await DeleteAsync(entity);
        })
        return entity;
    }
    ...
        
    public async Task StartTransactionAsync() => await _uow.StartAsync();

    public async Task CommitTransactionAsync() => await _uow.CommitAsync();
    
    public async Task RollbackTransactionAsync() => await _uow.RollbackAsync();
}
```