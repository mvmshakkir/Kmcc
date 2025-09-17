using demo.Areas.Identity.Data;
using demo.Models;
using demo.Areas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using demo.Migrations;
using Microsoft.EntityFrameworkCore;

namespace demo.Controllers
{
	[Authorize]
	public class Ward1 : Controller
	{
		private readonly demoContext _context; // Replace YourDbContext with your actual DbContext

		public Ward1(demoContext context)
		{
			_context = context;
		}
		[Route("ViewWard")]
		public IActionResult Index()
		{
            var wardData = _context.Ward.ToList();
            List<Ward> wardList = new List<Ward>();

            foreach (var item in wardData)
            {
                Ward Obj = new Ward();
                Obj.Id = item.Id;
                Obj.Wardno = item.Wardno;
                Obj.Wardname = item.Wardname;
                wardList.Add(Obj);
            }

            return View("/Views/User/ViewWard.cshtml", wardList); 
            
          
		}




        [HttpPost]
        public async Task<ActionResult> PostWard(Ward wards, long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ward = _context.Ward.FirstOrDefault(w => w.Id == wards.Id);

            if (ward != null)
            {

                ward.Wardno = wards.Wardno;
                ward.Wardname = wards.Wardname;
               
                _context.SaveChanges();
            }
            else
            {

                _context.Ward.Add(wards);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Ward1");
        }


        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            var ward = await _context.Ward.FirstOrDefaultAsync(i => i.Id == id);

            if (ward != null)
            {
                _context.Ward.Remove(ward);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");

        }


    }
}