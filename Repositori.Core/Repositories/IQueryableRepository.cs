using System.Linq;

namespace Repositori.Core.Repositories
{
    public class IQueryableRepository<TEntity>
    {
        /// <summary>
        /// Queryable source for filtering and mapping the data objects
        /// NOTE: This should be avoided for majority of situations as this will leak data access concerns into other layers
        /// </summary>
        IQueryable<TEntity> Query { get; }
    }
}