using EFCore.Entities;
using System.Security.Claims;
using UserManagement.Models.ViewModels;

namespace UserManagement.Services
{
    public interface IAccountService
    {
        public Task<AuthResult> RegisterAsync(RegisterViewModel user,
            IFormFile profilePic);

        public Task<AuthResult> LoginAsync(LoginViewModel user);

        public Task<AuthResult> UpdateUser(ClaimsPrincipal user,
            AccountManageViewModel reqUser, IFormFile profilePic);

        public Task<AccountManageViewModel> GetUser(ClaimsPrincipal user);

        public Task Logout();

    }
}