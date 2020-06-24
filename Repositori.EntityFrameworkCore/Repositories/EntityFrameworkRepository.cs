using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repositori.Core.Model;
using Repositori.Core.Repositories;

namespace Repositori.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// Implementation of the repository pattern encapsulating entity framework
    /// </summary>
    /// <typeparam name="TEntity">The data object type</typeparam>
    /// <typeparam name="TIdentifier">The data object identifier type</typeparam>
    public class EntityFrameworkRepository<TEntity, TIdentifier> : IRepository<TEntity, TIdentifier>
        where TEntity : class, IIdentifiable<TIdentifier>
    {
        /// <summary>
        /// Database context used for repository actions
        /// </summary>
        protected readonly DbContext Context;

        /// <summary>
        /// Constructor that initializes with a DbContext
        /// </summary>
        /// <param name="context">The database context to use for the repository actions</param>
        public EntityFrameworkRepository(DbContext context)
        {
            Context = context;
        }

        /// <inheritdoc />
        public IQueryable<TEntity> Query => Context.Set<TEntity>();

        /// <inheritdoc />
        public TEntity GetById(TIdentifier id) => Query.FirstOrDefault(e => e.Id.Equals(id));

        /// <inheritdoc />
        public async Task<TEntity> GetByIdAsync(TIdentifier id) =>
            await Query.FirstOrDefaultAsync(e => e.Id.Equals(id));

        /// <inheritdoc />
        public TEntity Create(TEntity entity)
        {
            var entityEntry = Context.Set<TEntity>().Add(entity);
            return entityEntry.Entity;
        }

        /// <inheritdoc />
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            var entityEntry = await Context.Set<TEntity>().AddAsync(entity);
            return entityEntry.Entity;
        }

        /// <inheritdoc />
        public List<TEntity> Create(ICollection<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
            return entities.ToList();
        }

        /// <inheritdoc />
        public async Task<List<TEntity>> CreateAsync(ICollection<TEntity> entities)
        {
            await Context.Set<TEntity>().AddRangeAsync(entities);
            return entities.ToList();
        }

        /// <inheritdoc />
        public TEntity Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        /// <inheritdoc />
        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            return await Task.Run(() =>
            {
                Context.Entry(entity).State = EntityState.Modified;
                return entity;
            });
        }

        /// <inheritdoc />
        public List<TEntity> Update(ICollection<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Context.Entry(entity).State = EntityState.Modified;
            }

            return entities.ToList();
        }

        /// <inheritdoc />
        public async Task<List<TEntity>> UpdateAsync(ICollection<TEntity> entities)
        {
            return await Task.Run(() =>
            {
                foreach (var entity in entities)
                {
                    Context.Entry(entity).State = EntityState.Modified;
                }

                return entities.ToList();
            });
        }
    }
}