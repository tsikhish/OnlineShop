using Microsoft.AspNetCore.Identity;

namespace OnlineShops.Models.DbModels
{
    public class UserRegistration
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
    public static class Role
    {
        public const string Accountant = "Accountant";
        public const string User = "User";
    }
}
