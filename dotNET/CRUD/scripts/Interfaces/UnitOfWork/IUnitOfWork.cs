public interface IUnitOfWork : IDisposable
{
    public void Commit();
    public void Rollback();
}