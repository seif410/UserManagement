using UserManagement.Models.ViewModels;

namespace UserManagement.Services
{
	public interface IAccountService
	{
		public Task<AuthResult> RegisterAsync(RegisterViewModel user);

		public Task<AuthResult> LoginAsync(LoginViewModel user);

		public void Logout();
	}
}