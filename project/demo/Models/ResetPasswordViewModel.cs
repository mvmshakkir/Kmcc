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
        [DisplayName("Conform  Password")]
        [Compare("Password",ErrorMessage ="Password an conform password must match")]
        public string ConfirmPassword { get; set;}

        public string Token { get; set; }
    }
}
