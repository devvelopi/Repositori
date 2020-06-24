using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositori.Core.Repositories
{
    /// <summary>
    /// Repository with only update functionality
    /// </summary>
    /// <typeparam name="TEntity">The repository data object type</typeparam>
    public interface IUpdateRepository<TEntity>
    {
        /// <summary>
        /// Update and persist a data object synchronously
        /// </summary>
        /// <param name="entity">The data object to update</param>
        /// <returns>The updated data object</returns>
        TEntity Update(TEntity entity);
        
        /// <summary>
        /// Update a data object asynchronously
        /// </summary>
        /// <param name="entity">The data object to update</param>
        /// <returns>An awaitable task providing the updated data object</returns>
        Task<TEntity> UpdateAsync(TEntity entity);
        
        /// <summary>
        /// Update a collection of data objects synchronously
        /// </summary>
        /// <param name="entities">The data objects to update</param>
        /// <returns>A list of updated data objects</returns>
        List<TEntity> Update(ICollection<TEntity> entities);
        
        /// <summary>
        /// Update a collection of data objects asynchronously
        /// </summary>
        /// <param name="entities">The data objects to update</param>
        /// <returns>An awaitable task providing a list of updated data objects</returns>
        Task<List<TEntity>> UpdateAsync(ICollection<TEntity> entities);
    }
}