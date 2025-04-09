//namespace RepoDemo.GenericRepository.Specification
//{
//    public class CustomerSpecification : BaseSpecification<Customer>
//    {
//        public CustomerSpecification(string name, int pageNumber, int pageSize)
//        {
//            Criteria = c => c.Name.Contains(name);
//            AddInclude(c => c.Orders);
//            ApplyOrderBy(c => c.Name);
//            ApplyPaging((pageNumber - 1) * pageSize, pageSize);
//            SetAsNoTracking();
//        }
//    }
//}
