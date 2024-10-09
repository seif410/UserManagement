using AutoMapper;
using EFCore.Entities;
using Microsoft.AspNetCore.Identity;
using UserManagement.Models.ViewModels;

namespace UserManagement.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public AccountService(UserManager<ApplicationUser> userManager, IMapper mapper
            , SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
        }

        public async Task<AuthResult> RegisterAsync(RegisterViewModel user)
        {
            if (await _userManager.FindByEmailAsync(user.Email) is not null)
                return new AuthResult
                {
                    Error = "This email address is already registered. " +
                    "Please user a different email or log in"
                };
            var appUser = _mapper.Map<ApplicationUser>(user);
            var result = await _userManager.CreateAsync(appUser, user.Password);
            if (!result.Succeeded)
            {
                var errorMsg = string.Empty;
                foreach (var error in result.Errors)
                    errorMsg += $"{error.Description}. \n";
                return new AuthResult { Error = errorMsg };
            }
            var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
            return roleResult.Succeeded ? new AuthResult { Success = true } :
                new AuthResult { Error = "User role not found!" };
        }

        public async Task<AuthResult> LoginAsync(LoginViewModel user)
        {
            var appUser = await _userManager.FindByEmailAsync(user.Email);
            if (appUser is null || !await _userManager.CheckPasswordAsync(appUser, user.Password))
                return new AuthResult { Error = "Invalid email or password. Please try again." };
            await _signInManager.SignInAsync(appUser, user.RememberMe);
            return new AuthResult { Success = true };
        }

        public async void Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}