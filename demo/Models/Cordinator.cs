using demo.Controllerse;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace demo.Models
{
	public class Cordinator
	{

		public int Id { get; set; }
		[Required(ErrorMessage = "Name is required")]
		public string Name { get; set; }
		[Required(ErrorMessage = "Phone number is required")]
		public string Phone {  get; set; }

		[Required(ErrorMessage = "Email address is required")]
		[EmailAddress(ErrorMessage = "Invalid email address")]
		//[UniqueEmail]
		public string Email { get; set; }
		[Required(ErrorMessage = "Please select a ward")]
		public long WardId { get; set; }
		[ForeignKey("WardId")]
		public Ward Ward { get; set; }
		[Required(ErrorMessage = "Password is required")]
		[MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
		[DataType(DataType.Password)]
		public string Password {  get; set; }
		[Required(ErrorMessage = "Please confirm your password")]
		[Compare("Password", ErrorMessage = "Passwords do not match")]
		[DataType(DataType.Password)]
		public string Conformpassword {get; set; }

		public bool IsCoordinator {  get; set; }
	}
}
