using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models.ViewModels
{
	public class RoleVM
	{
		[Required(ErrorMessage = "*")]
		public string Name { get; set; }
	}
}