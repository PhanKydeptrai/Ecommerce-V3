using AutoMapper;
using Domain.DTO_Models;
using Domain.Models;

namespace Ecommerce_BE.AutoMapProfiles
{
    public class OrderProfile : Profile 
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderCreationDto>().ReverseMap();
        }
    }
}
