namespace UserManagement.Models.ViewModels
{
    public class UserRolesVM
    {
        public string UserName { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
