﻿using EFCore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models.ViewModels;

namespace UserManagement.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
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
                if (role.Name != "User")
                {
                    userRoles.Add(new RoleViewModel
                    {
                        Name = role.Name,
                        IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
                    });
                }
            }
            await _signInManager.RefreshSignInAsync(user);
            return new SelectedRolesViewModel()
            {
                UserName = user.UserName,
                Roles = userRoles
            };
        }

        public async Task SaveRoleChanges(SelectedRolesViewModel selectedRoles)
        {
            var user = await _userManager.FindByNameAsync(selectedRoles.UserName);
            var roles = await _roleManager.Roles.ToListAsync();

            foreach (var role in roles)
            {
                if (role.Name == "User")
                    continue;

                var selected = selectedRoles.Roles
                    .FirstOrDefault(r => r.Name == role.Name)!.IsSelected;
                if (selected &&
                    !await _userManager.IsInRoleAsync(user, role.Name))
                    await _userManager.AddToRoleAsync(user, role.Name);

                if (!selected &&
                    await _userManager.IsInRoleAsync(user, role.Name))
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
            }
        }
    }
}
