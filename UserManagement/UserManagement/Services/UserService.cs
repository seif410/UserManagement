using EFCore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models.ViewModels;

namespace UserManagement.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<UserRolesViewModel>> GetUsersRolesAsync()
        {
            var userRoles = new List<UserRolesViewModel>();
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(new UserRolesViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName!,
                    Roles = roles
                });
            }

            return userRoles;
        }

        public async Task<SelectedRolesViewModel> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = new List<RoleViewModel>();

            foreach (var role in roles)
            {
                userRoles.Add(new RoleViewModel
                {
                    Name = role.Name,
                    IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
                });
            }

            var selUserRoles = new SelectedRolesViewModel()
            {
                UserName = user.UserName,
                Roles = userRoles
            };

            return selUserRoles;
        }
    }
}
