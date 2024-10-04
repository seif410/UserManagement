using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models.ViewModels
{
	public class RoleVM
	{
		[Required]
		[Display(Name = "Role Name")]
		public string Name { get; set; }
	}
}