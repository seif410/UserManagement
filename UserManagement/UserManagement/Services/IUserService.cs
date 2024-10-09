using UserManagement.Models.ViewModels;

namespace UserManagement.Services
{
    public interface IUserService
    {
        public Task<List<UserRolesVM>> GetUserRolesAsync();
    }
}