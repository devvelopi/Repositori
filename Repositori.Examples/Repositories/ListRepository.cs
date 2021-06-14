using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Repositori.Core.Repositories;
using Repositori.Examples.Repositories;

namespace Repositori.Examples.Repositories
{
    /// <summary>
    /// Example repository that simply uses a list as a lookup
    /// </summary>
    public class ListRepository<TEntity, TIdentifier> : IRepository<TEntity>
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

/// <summary>
/// Example data model containing simple fields
/// </summary>
public class DataModel
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
        var data = await repository.GetByAsync(e => e.Id == "1");
    }
}