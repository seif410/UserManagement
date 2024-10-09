using EFCore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models.ViewModels;

namespace UserManagement.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<UserRolesVM>> GetUserRolesAsync()
        {
            var userRoles = new List<UserRolesVM>();
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(new UserRolesVM
                {
                    UserName = user.UserName!,
                    Roles = roles
                });
            }

            return userRoles;
        }
    }
}
