using RepoDemo.Data;
using RepoDemo.GenericRepository.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace RepoDemo.Service
{
    public static class ServiceConfiguration
    {
        public static void AddServiceConfiguration(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDataConfiguration();
        }

        public static void SetUpDemoData(IServiceProvider serviceProvider)
        {
            DataConfiguration.SetUpDemoData(serviceProvider);
        }
    }
}
