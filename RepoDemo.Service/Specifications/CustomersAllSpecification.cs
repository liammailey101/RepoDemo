using RepoDemo.Data.Entities;
using RepoDemo.GenericRepository.Specification;

namespace RepoDemo.Service.Specifications
{
    public class CustomersAllSpecification : BaseSpecification<Customer>
    {
        public CustomersAllSpecification()
        {
            ApplyOrderBy(c => c.Name);
        }
    }
}
