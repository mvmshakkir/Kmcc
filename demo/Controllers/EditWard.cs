using demo.Areas.Identity.Data;
using demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace demo.Controllers
{
    [Authorize]
    public class EditWard : Controller
    {
        private readonly demoContext _context;
        public EditWard(demoContext context)
        {
            _context = context;
        }

        public IActionResult Index(long id)
        {
            var ward = _context.Ward.FirstOrDefault(w => w.Id == id);
            if (ward != null)
            {

                var wardmodel = new Ward
                {
                    Id = id,
                    Wardno = ward.Wardno,
                    Wardname = ward.Wardname,

                };


                return View("/Views/User/EditWard.cshtml", wardmodel);
            }


            return NotFound();
        }
    }
}