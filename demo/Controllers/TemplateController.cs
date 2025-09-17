using Microsoft.AspNetCore.Mvc;

namespace demo.Controllers
{
    public class TemplateController : Controller
    {
        [Route("Template")]
        public IActionResult Index()
        {
            return View("Template");
        }
    }
}
