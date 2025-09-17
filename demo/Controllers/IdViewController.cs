using demo.Areas.Identity.Data;
using demo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace demo.Controllers
{
	public class IdViewController : Controller
	{
        private readonly demoContext _context;
        public IdViewController(demoContext context, UserManager<demoUser> userManager, ILogger<regModel> logger, IOptions<EmailSettings> emailSettings)
        {
            _context = context;

        }
        public IActionResult Index(string id)
		{
            var search = _context.demoUser.FirstOrDefault(i => i.Id == id );
            var searchs = _context.demoUser.Where(i => i.Id == id ).ToList();
            if (search != null)
            {
                var family = _context.family
                    .Where(f => f.demoUserId == search.Id)
                    .ToList();

                ViewBag.data = searchs;
                ViewBag.family = family;
                long wardId = Convert.ToInt64(search.Ward);
                var ward = _context.Ward.Where(w => w.Id == wardId).ToList();
                ViewBag.ward = ward;

            }
            return View("/Views/User/QrView.cshtml");
        }
	}
}
