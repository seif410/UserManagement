using AutoMapper;
using EFCore.Entities;
using Microsoft.AspNetCore.Identity;
using System.Data;
using UserManagement.Models.ViewModels;

namespace UserManagement.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleService(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public IEnumerable<IdentityRole> GetRoles()
        {
            return _roleManager.Roles;
        }
        public async Task<AuthResult> AddRoleAsync(RoleVM role)
        {
            if (await _roleManager.FindByNameAsync(role.Name!) is not null)
                return new AuthResult { Success = false, Error = "This role already exists." };
            var identityRole = _mapper.Map<IdentityRole>(role);
            var result = await _roleManager.CreateAsync(identityRole);
            if (!result.Succeeded)
            {
                var errorMsg = string.Empty;
                foreach (var error in result.Errors)
                    errorMsg += $"{error} \n";
                return new AuthResult { Success = false, Error = errorMsg };
            }
            return new AuthResult { Success = true };
        }


    }
}