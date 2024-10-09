namespace UserManagement.Models.ViewModels
{
    public class UserRolesViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
