using Business.Services;
using FluentValidation;
using Model.Exceptions.UserExceptions;
using Model.ViewModels.UserModels;
using System;
using System.Linq;
using Xunit;

namespace WebAPITests
{

    public class UserServiceTest
    {
        private readonly IUserService _userService;

        public UserServiceTest(IUserService userService)
        {
            _userService = userService;
        }
        [Fact]
        public async System.Threading.Tasks.Task RegisterInvalidPassword()
        {
            var input = new RegisterViewModel()
            {
                FirstName = "Test",
                LastName = "Test",
                Username = "TestUsername",
                Password = "123"
            };

            await Assert.ThrowsAnyAsync<ValidationException>(() => _userService.RegisterAsync(input));
        }

        [Fact]
        public async System.Threading.Tasks.Task RegisterNoPassword()
        {
            var input = new RegisterViewModel()
            {
                FirstName = "Test",
                LastName = "Test",
                Username = "TestUsername",
                Password = "123"
            };

            await Assert.ThrowsAnyAsync<ValidationException>(() => _userService.RegisterAsync(input));
        }

        [Fact]
        public async System.Threading.Tasks.Task RegisterValidUser()
        {
            var input = new RegisterViewModel()
            {
                FirstName = "Test",
                LastName = "Test",
                Username = "TestUsername",
                Password = "as61dasd51asd"
            };

            await _userService.RegisterAsync(input);
        }

        [Fact]
        public async System.Threading.Tasks.Task RegisterDuplicateUsername()
        {
            var user1 = new RegisterViewModel()
            {
                FirstName = "Test",
                LastName = "Test",
                Username = "TestUsername",
                Password = "as61dasd51asd"
            };

            await _userService.RegisterAsync(user1);

            var user2 = new RegisterViewModel()
            {
                FirstName = "Test2",
                LastName = "Test2",
                Username = "TestUsername",
                Password = "dfgfgf12fgf"
            };

            await Assert.ThrowsAsync<DuplicateUsernameException>(() => _userService.RegisterAsync(user2));
        }
    }
}
