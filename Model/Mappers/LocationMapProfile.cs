using AutoMapper;
using FluentValidation;
using Model.Entities;
using Model.Validators.UserValidators;
using Model.ViewModels.Chat;
using Model.ViewModels.Location;
using Model.ViewModels.UserModels;
using NetTopologySuite.Geometries;
using System;

namespace Model.Mappers
{
    public class LocationMapProfile : Profile
    {
        public LocationMapProfile()
        {
            CreateMap<User, UserLocationViewModel>()
                .ForMember(vm => vm.Location, user => user.MapFrom(u => u.Location))
                .ForMember(vm => vm.User, user => user.MapFrom(u => u));

            CreateMap<LocationViewModel, Point>().ReverseMap();

        }
    }
}