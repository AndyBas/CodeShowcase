using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{

    public DbSet<Product> Products { get; set; }
    public DbSet<Person> Persons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "Server=localhost;Database=crud_database_training;User=root;Password=;";
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

    }
}