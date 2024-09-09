using AutoMapper;
using Domain.DTO_Models;
using Domain.Models;

namespace Ecommerce_BE.AutoMapProfiles
{
    public class RecommentProfile : Profile
    {
        public RecommentProfile() 
        {
            CreateMap<Recomment, RecommentCreationDto>().ReverseMap();
        }
    }
}
