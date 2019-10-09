using Model.ViewModels.UserModels;
using System;
using System.Threading.Tasks;


namespace Business.Services
{
    public interface IUserService
    {
        string Authenticate(LoginViewModel input);
        Task RegisterAsync(RegisterViewModel input);
        UserViewModel GetUser(Guid id);

    }
}