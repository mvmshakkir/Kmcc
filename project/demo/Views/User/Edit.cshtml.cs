using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace demo.Views.User
{
    public class EditModel : PageModel
    {
        [Route("User/Edit")]
        public IActionResult OnGet()
        {
            return RedirectToPage("/Views/User/Edit.cshtml");
        }
    }
}
