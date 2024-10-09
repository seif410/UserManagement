using Microsoft.AspNetCore.Identity;
using UserManagement.Models.ViewModels;

namespace UserManagement.Services
{
    public interface IRoleService
    {
        public IEnumerable<IdentityRole> GetRoles();

        public Task<AuthResult> AddRoleAsync(RoleVM role);
    }
}