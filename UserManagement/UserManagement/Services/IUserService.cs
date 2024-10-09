using UserManagement.Models.ViewModels;

namespace UserManagement.Services
{
    public interface IUserService
    {
        public Task<IEnumerable<UserRolesViewModel>> GetUsersRolesAsync();

        public Task<SelectedRolesViewModel> GetUserRolesAsync(string userId);
    }
}