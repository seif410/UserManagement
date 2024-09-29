using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models.ViewModels
{
	public class LoginVM
	{
		[Required(ErrorMessage = "*")]
		[RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$"
			, ErrorMessage = "Please enter a valid email address.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "*")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }
	}
}