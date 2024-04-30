using Microsoft.AspNetCore.Mvc;

namespace demo.Controllers
{
    public class SuccessfulPayment : Controller
    {
        [HttpGet]
        [Route("SuccessfulPayment")]

        public IActionResult Index()
        {
            return View("SuccessfullPayment");
        }

        //[HttpGet]
        //public IActionResult redirect() {

        //    return RedirectToAction("Index", "Home");

        //}
    }
}
