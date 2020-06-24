using Repositori.Core.Model;

namespace Repositori.Core.Repositories
{
    /// <summary>
    /// Repository with all functionality
    /// </summary>
    /// <typeparam name="TEntity">The repository data object type</typeparam>
    /// <typeparam name="TIdentifier">The identifier of the data object type</typeparam>
    public interface IRepository<TEntity, in TIdentifier> : IReadRepository<TEntity, TIdentifier>,
        ICreateRepository<TEntity>, IUpdateRepository<TEntity> where TEntity : IIdentifiable<TIdentifier>
    {
    }
}