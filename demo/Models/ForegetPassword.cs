using System.ComponentModel.DataAnnotations;
using demo.Areas.Identity.Data;
//using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;

namespace demo.Models
{
	public class ForegetPassword
	{
		[Key]
		public long RegId { get; set; }

		public String email {  get; set; }
		public string RegNo { get; set; }

		public bool EmailSent { get; set; }

		
       
    }	
}
