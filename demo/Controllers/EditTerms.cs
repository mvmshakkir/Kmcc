using demo.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace demo.Controllers
{
    [Authorize]
    public class EditTerms : Controller
    {
        private readonly demoContext _context;
        public EditTerms(demoContext context)
        {
            _context = context;
        }

		public IActionResult Index(long id)
		{
			var term = _context.Terms.FirstOrDefault(i => i.TermId == id);
			if (term != null)
			{
				
				var termsmodel = new Terms
				{
					TermId = id,
					Term = term.Term,
					Description = term.Description,
					amount = term.amount,
					TermPhoto = term.TermPhoto
				};

				
				return View("/Views/User/EditTerms.cshtml", termsmodel);
			}

			
			return NotFound(); 
		}





       


				
       
    }
}
