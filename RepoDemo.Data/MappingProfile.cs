using AutoMapper;
using RepoDemo.Data.Entities;
using RepoDemo.DTO;

namespace RepoDemo.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerSummary>();
            CreateMap<Customer, CustomerDetail>();
            CreateMap<Order, OrderDetail>();
        }
    }
}
