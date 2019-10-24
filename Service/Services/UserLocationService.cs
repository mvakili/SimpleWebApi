using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Model.ViewModels.UserModels;
using Model.Exceptions.UserExceptions;
using Business.Configurations;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using DataAccess.Repositories;
using Model.Entities;
using Model.ViewModels.Location;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Business.Services
{
    public class UserLocationService : IUserLocationService
    {
        private readonly JWTSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;

        public UserLocationService(IMapper mapper, IOptions<JWTSettings> appSettings, IUserService userService, IUserRepository userRepository)
        {
            _appSettings = appSettings.Value;
            _mapper = mapper;
            this._userService = userService;
            _userRepository = userRepository;
        }

        public IEnumerable<UserLocationViewModel> GetUsersAround(double x, double y, double radius)
        {
            var location = new Point(x, y);
            var users = this._userRepository.Get(u => u.Location.Distance(location) <= radius);
            return _mapper.Map<IEnumerable<UserLocationViewModel>>(users);
        }
        public IEnumerable<UserLocationViewModel> GetUsersAroundMe(ClaimsIdentity identity, double radius)
        {
            var user = this._userService.GetIdentityUser(identity);
            var location = this.GetUserLocation(identity);
            if (location == null) {
                throw new UserLocationNotFound();
            }

            var users = this._userRepository.Get(u => u.Id != user.Id && u.Location != null && u.Location.Distance(location) <= radius);
            return _mapper.Map<IEnumerable<UserLocationViewModel>>(users);
        }

        protected Point GetUserLocation(ClaimsIdentity identity)
        {
            var userModel = _userService.GetIdentityUser(identity);
            var user = _userRepository.Single(u => u.Id == userModel.Id);
            return user.Location;
        }

        public LocationViewModel GetUserLocationModel(ClaimsIdentity identity)
        {
            var location = GetUserLocation(identity);
            return _mapper.Map<LocationViewModel>(location);
        }

        public async Task SetUserLocationAsync(LocationViewModel input, ClaimsIdentity identity)
        {
            var userModel = _userService.GetIdentityUser(identity);
            var user = _userRepository.Single(u => u.Id == userModel.Id);
            user.Location = _mapper.Map<Point>(input);
            await _userRepository.SaveAsync();
        }


    }
}