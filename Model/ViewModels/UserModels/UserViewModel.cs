using System;

namespace Model.ViewModels.UserModels
{
    public class UserViewModel {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }

    }
}