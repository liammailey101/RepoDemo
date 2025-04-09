using RepoDemo.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace RepoDemo.Data
{
    public class DemoDbContext(DbContextOptions<DemoDbContext> options) : DbContext(options)
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
