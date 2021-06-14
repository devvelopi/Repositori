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
        /// <summary>
        /// Get a data object synchronously, matching the <paramref name="filter"/>.
        /// <paramref name="filter"/> should always return either one item or null.
        /// </summary>
        /// <param name="filter">The expression which filters down to the data object</param>
        /// <returns>The data object matching the <paramref name="filter"/></returns>
        TEntity GetBy(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Get a data object asynchronously, matching the <paramref name="filter"/>.
        /// <paramref name="filter"/> should always return either one item or null.
        /// </summary>
        /// <param name="filter">The expression which filters down to the data object</param>
        /// <returns>The data object matching the <paramref name="filter"/></returns>
        Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// List all data objects synchronously, matching the <paramref name="filter"/>.
        /// </summary>
        /// <param name="filter">The expression which filters down to the data objects</param>
        /// <param name="skip">To enable pagination. eg., skip the first 10 elements</param>
        /// <param name="take">To enable pargination. eg., take only 10 elements</param>
        /// <returns>The list of data objects matching the <paramref name="filter"/></returns>
        ICollection<TEntity> ListBy(Expression<Func<TEntity, bool>> filter, int skip, int take);

        /// <summary>
        /// List all data objects asynchronously, matching the <paramref name="filter"/>.
        /// </summary>
        /// <param name="filter">The expression which filters down to the data objects</param>
        /// <param name="skip">To enable pagination. eg., skip the first 10 elements</param>
        /// <param name="take">To enable pargination. eg., take only 10 elements</param>
        /// <returns>The list of data objects matching the <paramref name="filter"/></returns>
        Task<ICollection<TEntity>> ListByAsync(Expression<Func<TEntity, bool>> filter, int skip, int take);
    }
}