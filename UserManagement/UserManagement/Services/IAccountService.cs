using UserManagement.Models.ViewModels;

namespace UserManagement.Services
{
	public interface IAccountService
	{
		public Task<AuthResult> RegisterAsync(RegisterVM user);

		public Task<AuthResult> LoginAsync(LoginVM user);
	}
}