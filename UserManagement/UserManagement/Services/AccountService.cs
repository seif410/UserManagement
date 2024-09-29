using AutoMapper;
using EFCore.Entities;
using Microsoft.AspNetCore.Identity;
using UserManagement.Models.ViewModels;

namespace UserManagement.Services
{
	public class AccountService : IAccountService
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly IMapper mapper;

		public AccountService(UserManager<ApplicationUser> _userManager, IMapper _mapper
			, SignInManager<ApplicationUser> _signInManager)
		{
			userManager = _userManager;
			mapper = _mapper;
			signInManager = _signInManager;
		}

		public async Task<AuthResult> RegisterAsync(RegisterVM user)
		{
			if (await userManager.FindByEmailAsync(user.Email) is not null)
				return new AuthResult
				{
					Error = "This email address is already registered. " +
					"Please user a different email or log in"
				};
			var appUser = mapper.Map<ApplicationUser>(user);
			var result = await userManager.CreateAsync(appUser, user.Password);
			if (!result.Succeeded)
			{
				var errorMsg = string.Empty;
				foreach (var error in result.Errors)
					errorMsg += $"{error.Description}. \n";
				return new AuthResult { Error = errorMsg };
			}
			return new AuthResult { Success = true };
		}

		public async Task<AuthResult> LoginAsync(LoginVM user)
		{
			var appUser = await userManager.FindByEmailAsync(user.Email);
			if (appUser is null || !await userManager.CheckPasswordAsync(appUser, user.Password))
				return new AuthResult { Error = "Invalid email or password. Please try again." };
			await signInManager.SignInAsync(appUser, user.RememberMe);
			return new AuthResult { Success = true };
		}

		public async void Logout()
		{
			await signInManager.SignOutAsync();
		}
	}
}