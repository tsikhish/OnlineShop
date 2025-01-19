using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OnlineShops.Models.DbModels;

namespace OnlineShops.Models.Services
{
    public interface IUserService
    {
        Task<AppUser> Register([FromBody] UserRegistration user);
    }
    public class UserService : IUserService
    {
        private readonly AdventureWorksLT2019Context _personcontext;
        public UserService(AdventureWorksLT2019Context personcontext)
        {
            _personcontext = personcontext;
        }

        public async Task<AppUser> Register([FromBody] UserRegistration user)
        {
            var existingUser = await _personcontext.AppUser.FirstOrDefaultAsync(x => x.UserName == user.UserName);
            if (existingUser != null)
            {
                throw new Exception($"Already exists.");
            }
            var newUser = new AppUser
            {
                UserName = user.UserName,
                Password = user.Password,
                Role = user.Role,
            };
            await _personcontext.AppUser.AddAsync(newUser);
            await _personcontext.SaveChangesAsync();
            return newUser;
        }
    }
}
