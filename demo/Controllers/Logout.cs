using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace demo.Controllers
{
    
    public class Logout : Controller
    {
		//[Route("log")]
		public async Task<IActionResult> Log()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			//return RedirectToPage("/Account/Login", new { area = "Identity" });
			//return RedirectToAction("Index", "Home");
			return View("/Views/Home/Home.cshtml");
		}

    }
}
