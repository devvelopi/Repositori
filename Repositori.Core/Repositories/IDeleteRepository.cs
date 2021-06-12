using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repositori.Core.Repositories
{
    /// <summary>
    /// Repository with only delete functionality
    /// </summary>
    /// <typeparam name="TEntity">The repository data object type</typeparam>
    public interface IDeleteRepository<TEntity>
    {
    
        /// <summary>
        /// Delete a data object synchronously
        /// </summary>
        /// <param name="entity">The data object to delete</param>
        /// <returns>The deleted data object</returns>
        TEntity Delete(TEntity entity);

        /// <summary>
        /// Delete a data object asynchronously
        /// </summary>
        /// <param name="entity">The data object to delete</param>
        /// <returns>An awaitable task providing the deleted data object</returns>
        Task<TEntity> DeleteAsync(TEntity entity);
        
        TEntity DeleteBy(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Delete a collection of data objects synchronously
        /// </summary>
        /// <param name="entities">The data objects to delete</param>
        /// <returns>A list of deleted data objects</returns>
        List<TEntity> Delete(ICollection<TEntity> entities);

        /// <summary>
        /// Delete a collection of data objects asynchronously
        /// </summary>
        /// <param name="entities">The data objects to delete</param>
        /// <returns>An awaitable task providing a list of created data objects</returns>
        Task<List<TEntity>> DeleteAsync(ICollection<TEntity> entities);

        Task<TEntity> DeleteByAsync(Expression<Func<TEntity, bool>> filter);
    }
}