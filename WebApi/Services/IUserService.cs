using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.ViewModels;
using WebApi.ViewModels.UserModels;

namespace WebApi.Services
{
    public interface IUserService
    {
        string Authenticate(LoginViewModel input);
        Task RegisterAsync(RegisterViewModel input);
        UserViewModel GetUser(Guid id);

    }
}