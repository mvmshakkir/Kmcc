using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace demo.Models
{
    public class ResetPasswordViewModel
    {
        [Key]
        public long resetid { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        
        public string RegNo { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
		[StringLength(100, MinimumLength = 6, ErrorMessage = "The {0} must be at least {2} characters long.")]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$", ErrorMessage = "The password  must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
		[DisplayName("Confirm  Password")]
        [Compare("Password",ErrorMessage ="Password an confirm password must match")]
        public string ConfirmPassword { get; set;}

        public string Token { get; set; }
    }
}
