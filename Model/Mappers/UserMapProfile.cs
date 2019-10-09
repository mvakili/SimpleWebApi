using AutoMapper;
using FluentValidation;
using Model.Entities;
using Model.Validators.UserValidators;
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
                .BeforeMap((viewModel, user) =>
                {
                    var validationResult = new RegisterViewModelValidator().Validate(viewModel);
                    if (!validationResult.IsValid)
                    {
                        throw new ValidationException(validationResult.Errors);
                    }
                })
                .AfterMap((viewModel, user) =>
                {
                    user.PasswordHash = Helpers.CryptoHelper.SHA1Hash(viewModel.Password);
                    user.Id = default(Guid);
                });
        }
    }
}