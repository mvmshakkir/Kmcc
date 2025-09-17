using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace demo.Views.User
{
    public class DashBoardModel : PageModel
    {
        public  IActionResult Index()
        {
          
         return RedirectToPage("~/User/DashBoard/");
        }
    }
}
