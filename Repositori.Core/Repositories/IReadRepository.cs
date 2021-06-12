using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repositori.Core.Repositories
{
    /// <summary>
    /// Repository with readonly functionality
    /// </summary>
    /// <typeparam name="TEntity">The repository data object type</typeparam>
    public interface IReadRepository<TEntity>
    {
        TEntity GetBy(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> ListBy(Expression<Func<TEntity, bool>> filter, int skip, int take);
        Task<ICollection<TEntity>> ListByAsync(Expression<Func<TEntity, bool>> filter, int skip, int take);
    }
}