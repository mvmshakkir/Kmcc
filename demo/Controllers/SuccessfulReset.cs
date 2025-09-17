using Microsoft.AspNetCore.Mvc;

namespace demo.Controllers
{
    public class SuccessfulReset : Controller
    {
        [HttpGet]
        [Route("SuccessfulReset")]

        public IActionResult Index()
        {
            return View("SuccessfulReset");
        }

        //[HttpGet]
        //public IActionResult redirect() {

        //    return RedirectToAction("Index", "Home");

        //}
    }
}
