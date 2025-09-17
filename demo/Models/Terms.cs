using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using demo.Areas.Identity.Data;

namespace demo.Models
{
	public class Terms
	{
		[Key]
		public long TermId { get; set; }

		public String Term {  get; set; }
		public string Description { get; set; }

		public double amount { get; set; }
		public Payment Payment { get; set; }


        // New property for photo
        
        public string TermPhoto { get; set; }

        [NotMapped]
        
        public IFormFile TermPhotoFile { get; set; }


    }
}
