using RepoDemo.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace RepoDemo.Data
{
    public static class DataConfiguration
    {
        public static void AddDataConfiguration(this IServiceCollection services)
        {
            services.AddDbContext<DbContext, DemoDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
            services.AddAutoMapper(typeof(MappingProfile));
        }

        public static void SetUpDemoData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DemoDbContext>();

            var customers = new List<Customer>();
            var orders = new List<Order>();
            var random = new Random();

            for (int i = 1; i <= 10; i++)
            {
                var customer = new Customer
                {
                    CustomerId = i,
                    Name = $"Customer {i}",
                    Email = $"customer{i}@example.com",
                    CreatedDate = DateTime.Now,
                    CreatedBy = "System",
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = "System",
                    Orders = [] // Initialize the Orders collection
                };
                customers.Add(customer);

                for (int j = 1; j <= random.Next(0, 16); j++)
                {
                    orders.Add(new Order
                    {
                        OrderId = (i - 1) * 15 + j,
                        OrderDate = DateTime.Now.AddDays(-random.Next(0, 100)),
                        TotalAmount = random.Next(100, 1000),
                        CustomerId = i
                    });
                }
            }
            context.Customers.AddRange(customers);
            context.Orders.AddRange(orders);
            context.SaveChanges();
        }
    }
}
