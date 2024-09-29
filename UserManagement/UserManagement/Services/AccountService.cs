using AutoMapper;
using EFCore.Entities;
using Microsoft.AspNetCore.Identity;
using UserManagement.Models.ViewModels;

namespace UserManagement.Services
{
	public class AccountService : IAccountService
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly IMapper mapper;

		public AccountService(UserManager<ApplicationUser> _userManager, IMapper _mapper)
		{
			userManager = _userManager;
			mapper = _mapper;
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

		public Task<AuthResult> LoginAsync(LoginVM user)
		{
			throw new NotImplementedException();
		}
	}
}