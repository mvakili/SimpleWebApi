using AutoMapper;
using FluentValidation;
using Model.Entities;
using Model.Validators.UserValidators;
using Model.ViewModels.Chat;
using Model.ViewModels.UserModels;
using System;

namespace Model.Mappers
{
    public class ChatMapProfile : Profile
    {
        public ChatMapProfile()
        {
        }
    }
}