using AutoMapper;
using Model.Entities;
using Model.ViewModels.UserModels;
using System;

namespace Model.Mappers
{
    public class UserMapProfile : Profile
    {
        public UserMapProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<RegisterViewModel, User>()
                .AfterMap((viewMode, user) =>
                {
                    user.PasswordHash = Helpers.CryptoHelper.SHA1Hash(viewMode.Password);
                    user.Id = default(Guid);
                });
        }
    }
}