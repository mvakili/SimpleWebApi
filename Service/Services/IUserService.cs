using Model.Entities;
using Model.ViewModels.UserModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Business.Services
{
    public interface IUserService
    {
        string Authenticate(LoginViewModel input);
        Task RegisterAsync(RegisterViewModel input);
        UserViewModel GetIdentityUser(ClaimsIdentity identity);

    }
}