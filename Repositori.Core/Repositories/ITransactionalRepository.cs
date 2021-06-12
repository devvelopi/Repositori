using System.Threading.Tasks;

namespace Repositori.Core.Repositories
{
    /// <summary>
    /// Repository with transactional functionality
    /// </summary>
    public interface ITransactionalRepository<TEntity> : IRepository<TEntity>
    {
        /// <summary>
        /// Begin a transaction surrounding all repository operations
        /// </summary>
        /// <returns>An awaitable task</returns>
        Task StartTransactionAsync();

        /// <summary>
        /// Commit a transaction and all repository operations
        /// </summary>
        /// <returns>An awaitable task</returns>
        Task CommitTransactionAsync();
        
        /// <summary>
        /// Rollback a transaction and all repository operations
        /// </summary>
        /// <returns></returns>
        Task RollbackTransactionAsync();
    }
}