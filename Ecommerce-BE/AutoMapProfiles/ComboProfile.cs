using AutoMapper;
using Domain.DTO_Models;
using Domain.Models;

namespace Ecommerce_BE.AutoMapProfiles
{
    public class ComboProfile : Profile
    {
        public ComboProfile()
        {
            CreateMap<Combo, ComboCreationDto>().ReverseMap();
        }
    }
}
