using Microsoft.AspNetCore.Mvc;

namespace demo.Controllers
{
    public class CheckOut : Controller
    {
        [Route("CheckOut")]
        public IActionResult Index()
        {
            return View("/Views/User/CheckOut.cshtml");
        }
    }
}
