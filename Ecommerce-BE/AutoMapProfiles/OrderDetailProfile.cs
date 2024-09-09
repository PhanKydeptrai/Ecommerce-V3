using AutoMapper;
using Domain.DTO_Models;
using Domain.Models;

namespace Ecommerce_BE.AutoMapProfiles
{
    public class OrderDetailProfile : Profile
    {
        public OrderDetailProfile()
        {
            CreateMap<OrderDetail, OrderDetailCreationDto>().ReverseMap();
        }
    }
}
