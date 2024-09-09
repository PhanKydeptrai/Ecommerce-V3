using AutoMapper;
using Domain.DTO_Models;
using Domain.Models;

namespace Ecommerce_BE.AutoMapProfiles
{
    public class ComboDetailProfile : Profile
    {
        public ComboDetailProfile()
        {
            CreateMap<ComboDetail,ComboDetailCreationDto>().ReverseMap();
        }
    }
}
