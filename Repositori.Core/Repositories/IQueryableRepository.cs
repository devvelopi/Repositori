using System.Linq;

namespace Repositori.Core.Repositories
{
    /// <summary>
    /// Repository with querying functionality
    /// </summary>
    /// <typeparam name="TEntity">The repository data object type</typeparam>
    public interface IQueryableRepository<TEntity>
    {
        /// <summary>
        /// Queryable source for filtering and mapping the data objects
        /// NOTE: This should be avoided for majority of situations as this will leak data access concerns into other layers
        /// </summary>
        IQueryable<TEntity> Query { get; }
    }
}