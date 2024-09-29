using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models.ViewModels
{
	public class RegisterVM
	{
		[Required]
		[Length(2, 25, ErrorMessage = "Username length should be between 2 and 25")]
		[Display(Name = "Username")]
		public string UserName { get; set; }

		[Required]
		[RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$"
			, ErrorMessage = "Please enter a valid email address.")]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm Password")]
		[Compare("Password", ErrorMessage = "Passwords do not match. Please try again.")]
		public string ConfirmPassword { get; set; }
	}
}