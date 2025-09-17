using Microsoft.AspNetCore.Mvc;

namespace demo.Models
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
