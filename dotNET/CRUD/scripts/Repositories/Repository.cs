using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

public class Repository<T> : IRepository<T> where T : class
{
    protected DbContext _context;

    public Repository(DbContext context)
    {
        _context = context;
    }
    public void Add(T newData)
    {
        _context.Set<T>().Add(newData);
    }

    public void Remove(T dataToRemove)
    {
        _context.Set<T>().Remove(dataToRemove);
    }

    public IEnumerable<T> Find(Expression<Func<T,bool>> predicate)
    {
        return _context.Set<T>().Where(predicate);
    }

    public T? Get(int id)
    {
        return _context.Set<T>().Find(id);
    }

}