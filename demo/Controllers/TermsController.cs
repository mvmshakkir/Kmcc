using demo.Areas.Identity.Data;
using demo.Models;
using demo.Views.reg;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace demo.Controllers
{
	[Authorize]
	public class TermsController : Controller
	{
		private readonly demoContext _context;
		IWebHostEnvironment _webHostEnvironment;
		public TermsController(demoContext context, IWebHostEnvironment webHostEnvironment)
		{
			
			_webHostEnvironment = webHostEnvironment;
			_context = context;
		}
        [Route("Index")]
        public IActionResult Index()
		{

			var Termdata = _context.Terms.ToList();
			List<Terms> TermsList = new List<Terms>();

			foreach (var item in Termdata)
			{
				Terms Obj = new Terms();
				Obj.TermId = item.TermId;
				Obj.Term = item.Term;
				Obj.Description = item.Description;
				Obj.amount = item.amount;
				TermsList.Add(Obj);
			}
			return View("/Views/User/ViewTerms.cshtml", TermsList);
		}
		[HttpPost]
		public async Task<ActionResult> PostTerm(Terms terms,long id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var term = _context.Terms.FirstOrDefault(i => i.TermId == terms.TermId);
			if (terms.TermPhotoFile != null)
			{
				string folder = "Terms"; // Relative folder path

				// Generate a unique file name for the uploaded photo
				string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(terms.TermPhotoFile.FileName);

				// Combine the folder path with the unique file name
				string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

				// Ensure the directory exists, create if not
				if (!Directory.Exists(serverFolder))
				{
					Directory.CreateDirectory(serverFolder);
				}

				// Combine the folder path with the unique file name to get the absolute path where the file will be saved on the server
				string filePath = Path.Combine(serverFolder, uniqueFileName);

				// Copy the uploaded photo to the specified folder
				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					await terms.TermPhotoFile.CopyToAsync(fileStream);
				}

				// Save the file path to the database or wherever needed
				// For example, you can save it to a property in your Terms model
				terms.TermPhoto = Path.Combine( uniqueFileName);
			}

			if (term != null)
			{
				
				term.Term = terms.Term;
				term.Description = terms.Description;
				term.amount = terms.amount;
				term.TermPhoto = terms.TermPhoto;
				
				_context.SaveChanges();
			}
			else
			{
				
				_context.Terms.Add(terms);
			}

			await _context.SaveChangesAsync();

			return RedirectToAction("Index", "Terms");
		}

		public Terms Get(long id)
        {
            var Terms = _context.Terms.Where(i => i.TermId == id).FirstOrDefault();
            if (Terms != null)
            {

                var Term = new Terms
                {
                    TermId = id,
                    Term = Terms.Term,
                    Description = Terms.Description,
                    amount = Terms.amount,
                    


                };

                return Term; // Ok() signifies a successful response with the user settings
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            var term = await _context.Terms.FirstOrDefaultAsync(i => i.TermId == id);

            if (term != null)
            {
                _context.Terms.Remove(term);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");

        }
    }
}
