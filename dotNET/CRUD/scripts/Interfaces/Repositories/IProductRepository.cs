public interface IProductRepository : IRepository<Product>
{
    public IEnumerable<Product> GetProductsByCategory(string category);
    public IEnumerable<Product> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);
}