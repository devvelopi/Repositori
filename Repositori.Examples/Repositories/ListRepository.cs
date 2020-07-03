using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositori.Core.Model;
using Repositori.Core.Repositories;
using Repositori.Examples.Repositories;

namespace Repositori.Examples.Repositories
{
    /// <summary>
    /// Example repository that simply uses a list as a lookup
    /// </summary>
    public class ListRepository<TEntity, TIdentifier> : IRepository<TEntity, TIdentifier>
        where TEntity : IIdentifiable<TIdentifier>
    {
        private readonly List<TEntity> _entities;

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

        /// <inheritdoc />
        public TEntity Delete(TEntity entity)
        {
            _entities.RemoveAll(e => e.Id.Equals(entity.Id));
            return entity;
        }

        /// <inheritdoc />
        public async Task<TEntity> DeleteAsync(TEntity entity) => await Task.Run(() => Delete(entity));

        /// <inheritdoc />
        public List<TEntity> Delete(ICollection<TEntity> entities)
        {
            _entities.RemoveAll(e => entities.Any(en => en.Id.Equals(e.Id)));
            return entities.ToList();
        }

        /// <inheritdoc />
        public async Task<List<TEntity>> DeleteAsync(ICollection<TEntity> entities) => await Task.Run(() => Delete(entities));
    }
}

/// <summary>
/// Example data model containing simple fields
/// </summary>
public class DataModel : IIdentifiable<string>
{
    public string Id { get; set; }
    public string SomeField { get; set; }
}

/// <summary>
/// Example program that initialises a repository
/// </summary>
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