using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models.ViewModels
{
    public class AccountManageViewModel
    {
        public string? Id { get; set; }

        [Required]
        [Display(Name = "Firstname")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Lastname")]
        public string LastName { get; set; }

        [Length(2, 25, ErrorMessage = "Username length should be between 2 and 25")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$"
            , ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Display(Name = "Profile Picture")]
        public byte[] ProfilePicture { get; set; }
    }
}