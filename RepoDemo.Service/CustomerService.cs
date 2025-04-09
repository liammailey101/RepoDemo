using RepoDemo.Common;
using RepoDemo.Data.Entities;
using RepoDemo.DTO;
using RepoDemo.GenericRepository.Repository;
using RepoDemo.Service.Specifications;

namespace RepoDemo.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITelemetryClient _telemetryClient;

        public CustomerService(IUnitOfWork unitOfWork, ITelemetryClient telemetryClient)
        {
            _unitOfWork = unitOfWork;
            _customerRepository = _unitOfWork.Repository<Customer>();
            _telemetryClient = telemetryClient;
        }

        public async Task<ServiceResult<Customer?>> GetCustomerByIdAsync(int id)
        {
            try
            {
                var specification = new CustomerWithOrdersSpecification(id);
                var customer = await _customerRepository.GetSingleAsync(specification);
                return ServiceResult<Customer?>.Success(customer);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                return ServiceResult<Customer?>.Failure("An error occurred while retrieving the customer.");
            }
        }

        public async Task<ServiceResult<List<CustomerSummary>>> GetAllCustomersAsync()
        {
            try
            {
                var specification = new CustomersAllSpecification();
                var customers = await _customerRepository.GetListAsync<CustomerSummary>(specification);
                return ServiceResult<List<CustomerSummary>>.Success(customers);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                return ServiceResult<List<CustomerSummary>>.Failure("An error occurred while retrieving all customers.");
            }
        }

        public async Task<ServiceResult<bool>> AddCustomerAsync(Customer customer)
        {
            try
            {
                await _customerRepository.AddAsync(customer);
                await _unitOfWork.CompleteAsync();
                return ServiceResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                return ServiceResult<bool>.Failure("An error occurred while adding the customer.");
            }
        }

        public async Task<ServiceResult<bool>> UpdateCustomerAsync(Customer customer)
        {
            try
            {
                _customerRepository.Update(customer);
                await _unitOfWork.CompleteAsync();
                return ServiceResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                return ServiceResult<bool>.Failure("An error occurred while updating the customer.");
            }
        }

        public async Task<ServiceResult<bool>> DeleteCustomerAsync(int id)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(id);
                if (customer != null)
                {
                    _customerRepository.Delete(customer);
                    await _unitOfWork.CompleteAsync();
                    return ServiceResult<bool>.Success(true);
                }
                return ServiceResult<bool>.Failure("Customer not found.");
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                return ServiceResult<bool>.Failure("An error occurred while deleting the customer.");
            }
        }
    }
}