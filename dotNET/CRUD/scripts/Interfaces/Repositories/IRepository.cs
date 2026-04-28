using System.Linq.Expressions;

public interface IRepository<T> where T : class
{
    public void Add(T newData);
    public void Remove(T dataToRemove);
    public T? Get(int id);
    public IEnumerable<T> Find(Expression<Func<T,bool>> predicate);

}