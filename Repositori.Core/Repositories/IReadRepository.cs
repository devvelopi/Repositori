using System.Linq;
using System.Threading.Tasks;
using Repositori.Core.Model;

namespace Repositori.Core.Repositories
{
    /// <summary>
    /// Repository with readonly functionality
    /// </summary>
    /// <typeparam name="TEntity">The repository data object type</typeparam>
    /// <typeparam name="TIdentifier">The identifier of the data object type</typeparam>
    public interface IReadRepository<TEntity, in TIdentifier> where TEntity : IIdentifiable<TIdentifier>
    {
        /// <summary>
        /// Queryable source for filtering and mapping the data objects
        /// </summary>
        IQueryable<TEntity> Query { get; }

        /// <summary>
        /// Retrieves a data object by it's unique identifier synchronously
        /// </summary>
        /// <param name="id">Unique identifier of the data object</param>
        /// <returns>A data object with the specified id or null</returns>
        TEntity GetById(TIdentifier id);

        /// <summary>
        /// Retrieves a data object by it's unique identifier asynchronously
        /// </summary>
        /// <param name="id">Unique identifier of the data object</param>
        /// <returns>An awaitable task providing a data object with the specified id or null</returns>
        Task<TEntity> GetByIdAsync(TIdentifier id);
    }
}