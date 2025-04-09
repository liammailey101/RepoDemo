using RepoDemo.Data.Entities;
using RepoDemo.DTO;

namespace RepoDemo.Service
{
    public interface ICustomerService
    {
        Task<ServiceResult<Customer?>> GetCustomerByIdAsync(int id);
        Task<ServiceResult<List<CustomerSummary>>> GetAllCustomersAsync();
        Task<ServiceResult<bool>> AddCustomerAsync(Customer customer);
        Task<ServiceResult<bool>> UpdateCustomerAsync(Customer customer);
        Task<ServiceResult<bool>> DeleteCustomerAsync(int id);
    }
}
