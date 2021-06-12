namespace Repositori.Core.Repositories
{
    /// <summary>
    /// Repository with all read and write functionality
    /// </summary>
    /// <typeparam name="TEntity">The repository data object type</typeparam>
    public interface IRepository<TEntity> : IReadRepository<TEntity>, IWriteRepository<TEntity>
    {
    }
}