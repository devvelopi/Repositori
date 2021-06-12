namespace Repositori.Core.Repositories
{
    public interface IWriteRepository<TEntity> : ICreateRepository<TEntity>, IUpdateRepository<TEntity>,
        IDeleteRepository<TEntity>
    {
    }
}