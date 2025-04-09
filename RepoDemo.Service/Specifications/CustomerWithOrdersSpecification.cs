using RepoDemo.Data.Entities;
using RepoDemo.GenericRepository.Specification;

namespace RepoDemo.Service.Specifications
{
    public class CustomerWithOrdersSpecification : BaseSpecification<Customer>
    {
        public CustomerWithOrdersSpecification(int id)
        {
            Criteria = c => c.CustomerId == id; 
            AddInclude(c => c.Orders);
        }
    }
}
