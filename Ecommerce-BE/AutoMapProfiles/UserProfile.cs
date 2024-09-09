using AutoMapper;
using Domain.DTO_Models;
using Domain.Models;

namespace Ecommerce_BE.AutoMapProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserCreationDto>().ReverseMap();
        }
    }
}
