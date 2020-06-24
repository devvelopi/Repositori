using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositori.Core.Repositories
{
    /// <summary>
    /// Repository with only create functionality
    /// </summary>
    /// <typeparam name="TEntity">The repository data object type</typeparam>
    public interface ICreateRepository<TEntity>
    {
        /// <summary>
        /// Create a data object synchronously
        /// </summary>
        /// <param name="entity">The data object to create</param>
        /// <returns>The created data object</returns>
        TEntity Create(TEntity entity);

        /// <summary>
        /// Create a data object asynchronously
        /// </summary>
        /// <param name="entity">The data object to create</param>
        /// <returns>An awaitable task providing the created data object</returns>
        Task<TEntity> CreateAsync(TEntity entity);

        /// <summary>
        /// Create a collection of data objects synchronously
        /// </summary>
        /// <param name="entities">The data objects to create</param>
        /// <returns>A list of created data objects</returns>
        List<TEntity> Create(ICollection<TEntity> entities);

        /// <summary>
        /// Create a collection of data objects asynchronously
        /// </summary>
        /// <param name="entities">The data objects to create</param>
        /// <returns>An awaitable task providing a list of created data objects</returns>
        Task<List<TEntity>> CreateAsync(ICollection<TEntity> entities);
    }
}