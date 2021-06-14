namespace Repositori.Core.Repositories
{
    /// <summary>
    /// Repository with all write functionality
    /// </summary>
    /// <typeparam name="TEntity">The repository data object type</typeparam>
    public interface IWriteRepository<TEntity> : ICreateRepository<TEntity>, IUpdateRepository<TEntity>,
        IDeleteRepository<TEntity>
    {
    }
}