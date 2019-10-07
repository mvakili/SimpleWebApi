using AutoMapper;
using WebApi.Entities;
using WebApi.ViewModels;

namespace WebApi.ViewModels.UserModels
{
    public class UserMapProfile : Profile
    {
        public UserMapProfile()
        {
            CreateMap<User, UserViewModel>();
        }
    }
}