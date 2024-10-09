using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models.ViewModels
{
    public class RoleViewModel
    {
        [Required]
        [Display(Name = "Role Name")]
        public string Name { get; set; }

        public bool IsSelected { get; set; }
    }
}