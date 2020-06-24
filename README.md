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

*Coming soon*

## Usage

#### Creating Custom Repositories
Primary use involves creating custom repositories using the `IRepository` interface. The example below is a simple repository built around a generic list.
```c#
public class ListRepository<TEntity, TIdentifier> : IRepository<TEntity, TIdentifier> where TEntity : IIdentifiable<TIdentifier>
{
    protected readonly List<TEntity> _entities;

    public ListRepository(List<TEntity> entities)
    {
        _entities = entities;
    }

    /// <inheritdoc />
    public IQueryable<TEntity> Query => _entities.AsQueryable();

    /// <inheritdoc />
    public TEntity GetById(TIdentifier id) => _entities.FirstOrDefault(e => e.Id.Equals(id));

    /// <inheritdoc />
    public async Task<TEntity> GetByIdAsync(TIdentifier id) => await Task.Run(() => GetById(id));

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
        var index = _entities.FindIndex(e => e.Id.Equals(entity.Id));
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

### Custom LINQ Providers

Writing custom LINQ providers is indeed possible [if a little difficult](https://weblogs.asp.net/mehfuzh/writing-custom-linq-provider).

By writing custom providers, it is possible to abstract ANY data-source,
behind the great pre-existing LINQ functionality.

### Uniti
A shameless plug for my other project [Uniti](https://github.com/jaseaman/Uniti), an implementation of the Unit of Work pattern which allows 
transactional functionality across all operations, not just limited to the database.

It works well with making non-transactional repositories into transactional repositories by using Uniti's `IUnitTransactional` interface. 
Such as the pseudo code below.

```c#

public class MockRepository<TEntity, TIdentifier> : IRepository<TEntity, TIdentifier>, IUnitTransactional 
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
        
    public async Task StartUnitAsync()
    {
        await _uow.StartAsync();
    }

    public async Task CommitUnitAsync() 
    {
        await _uow.CommitAsync();
    }
    
    public async Task RollbackUnitAsync() 
    {
        await _uow.RollbackAsync();
    }
}
```