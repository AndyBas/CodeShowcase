using Microsoft.EntityFrameworkCore;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(DbContext context) : base(context)
    {
    }

    public IEnumerable<Product> GetProductsByCategory(string category)
    {
        return _context.Set<Product>().Where(p => p.Category == category).ToList();
    }

    public IEnumerable<Product> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
    {
        return _context.Set<Product>().Where(p => p.Price > minPrice && p.Price < maxPrice).ToList();
    }
}