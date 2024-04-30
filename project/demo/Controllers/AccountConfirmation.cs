using Microsoft.AspNetCore.Mvc;

namespace demo.Controllers
{
    public class AccountConfirmation : Controller
    {
        [HttpGet]
        [Route("AccountConfirmation")]
        public IActionResult Index()
        {
            return View("AccountConformation");
        }
    }
}
