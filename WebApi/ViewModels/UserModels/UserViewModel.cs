using System;

namespace WebApi.ViewModels.UserModels
{
    public class UserViewModel {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }

    }
}