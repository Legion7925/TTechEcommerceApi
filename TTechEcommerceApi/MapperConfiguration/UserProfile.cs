using AutoMapper;
using EcommerceApi.Entities;
using TTechEcommerceApi.Model;

namespace TTechEcommerceApi.MapperConfiguration
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRequestModel, User>();

            CreateMap<User, UserResponseModel>();

            CreateMap<User, AuthenticateResponseModel>();
        }
    }
}
