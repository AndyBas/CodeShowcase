using Microsoft.EntityFrameworkCore;

public class UnitOfWork : IUnitOfWork
{
    DbContext _context;

    private IProductRepository _productRepository;
    private IPersonRepository _personRepository;

    public IProductRepository ProductRepository
    {
        get
        {
            if (_productRepository == null)
                _productRepository = new ProductRepository(_context);
            return _productRepository;
        }
    }

    public IPersonRepository PersonRepository
    {
        get
        {
            if (_personRepository == null)
                _personRepository = new PersonRepository(_context);
            return _personRepository;
        }
    }
    
    public UnitOfWork(DbContext context)
    {
        _context = context;
        _personRepository = new PersonRepository(_context);
        _productRepository = new ProductRepository(_context);
    }

    public void Commit()
    {
        // Code to commit the transaction
        _context.SaveChanges();
    }

    public void Rollback()
    {
        // Code to rollback the transaction
        _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}