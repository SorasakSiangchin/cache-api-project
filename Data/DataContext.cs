using CacheProject.Models;
using Microsoft.EntityFrameworkCore;
using Bogus;
namespace CacheProject.Data;

public class DataContext : DbContext
{
    public DataContext()
    {
        
    }
    
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        var testUsers = new Faker<User>()
            .RuleFor(u => u.Id, f => -1 - f.UniqueIndex) // Provide a negative value for 'Id'
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Username))
            .Generate(1000);

        modelBuilder.Entity<User>().HasData(testUsers);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite("Data Source=cache-project.db");
    }

   

    public DbSet<User> Users { get; set; }
}