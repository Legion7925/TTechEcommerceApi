using AutoMapper;
using EcommerceApi.Entities;
using TTechEcommerceApi.Model;

namespace TTechEcommerceApi.MapperConfiguration
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderUpdateModel, Order>();   
        }
    }
}
