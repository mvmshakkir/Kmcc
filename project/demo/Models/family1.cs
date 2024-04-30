using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using demo.Areas.Identity.Data;

namespace demo.Models
{
	public class family1
	{
		[Key] public int Id { get; set; }
		

		[Required]

		[StringLength(50, ErrorMessage = "Max 50 characters allowed")]

		public String Name { get; set; }

		[Required]
		[StringLength(50, ErrorMessage = "50 characters allowed")]

		public string Relation { get; set; }


		[Required]
		[StringLength(50, ErrorMessage = "50 characters allowed")]

		public string occupation { get; set; }


		[ForeignKey("userId")]
		public string userId { get; set; }
		public virtual demoUser demoUser { get; set; }

	}
}
