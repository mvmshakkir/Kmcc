using demo.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace demo.Controllers
{
    [Authorize]
    public class DashBoard : Controller
    {
        private readonly demoContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DashBoard(demoContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            this._context = context;
            this._webHostEnvironment = webHostEnvironment;

        }
        [Route("User/DashBoard")]
        public IActionResult Index()
        {
            {
                var productCount = _context.demoUser.Count();
                ViewBag.ProductCount = productCount;
                var paymentcount = _context.Payment.OrderBy(u => u.TermId).Count();
                ViewBag.PaymentCount = paymentcount;
            }
            return View("/Views/User/DashBoard.cshtml");
        }
    }
}
