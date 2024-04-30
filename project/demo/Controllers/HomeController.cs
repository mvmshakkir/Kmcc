using System.Diagnostics;
using demo.Areas.Identity.Data;
using demo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace demo.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly demoContext _context;
		public HomeController(ILogger<HomeController> logger, demoContext context)
        {
            _logger = logger;
			_context = context;
        }

		public IActionResult Index()
		{
			var countries = _context.ListCountrie.Select(c => c.Countrie).ToList();
			ViewBag.CountryList = countries;

			var EditId = new EditId();
			// Assuming 'Ward' property in 'EditId' is of type List<Ward>
		
			List<ListCountrie> ListCountrie = _context.ListCountrie.ToList();
			EditId.ListCountrie = ListCountrie;
			return View(EditId);
		}


		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}