using System;
using System.ComponentModel.DataAnnotations;

namespace Model.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string LastName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
    }
}