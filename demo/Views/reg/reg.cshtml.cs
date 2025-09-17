using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using demo.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Security.Policy;
using System.Web.Mvc;
using demo.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using static demo.Views.reg.reg;

namespace demo.Views.reg
{

    public class reg : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public reg(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync(regModel Reg)
        {
            {
                //if (Reg.ProfileImage != null)
                //{
                //    string folder = "profile/photo";
                //    folder += Reg.UserImage + Guid.NewGuid().ToString();
                //    Reg.UserImage = folder;
                //    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                //    new FileStream(serverFolder, FileMode.Create);


                //}
               

            }
            return Page();
        }
        

    }

}





