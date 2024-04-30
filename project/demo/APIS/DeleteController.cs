using demo.Areas.Identity.Data;
using demo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace demo.APIS
{


    public class DeleteController : Controller
    {
        IWebHostEnvironment _webHostEnvironment;

        private readonly demoContext _context;
        private readonly UserManager<demoUser> _userManager;
        public DeleteController(demoContext context,
             UserManager<demoUser> userManager)
        {
            _context = context;
            _userManager = userManager;

            //	this._emailStore = emailStore;

        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var demoUser = _context.demoUser.Where(i => i.Id == id).Include(d => d.family).FirstOrDefault();

            if (demoUser != null)
            {
				var userPayments = _context.Payment.Where(p => p.UserId == id);
				_context.Payment.RemoveRange(userPayments);
				var user = await _userManager.FindByIdAsync(id);

                if (user != null)
                {
					

					_context.family.RemoveRange(demoUser.family);
                    // _context.family.RemoveRange(user.family);
                    var result = await _userManager.DeleteAsync(user);
                 

                    if (result.Succeeded)
                    {
                      
                        
                        await _context.SaveChangesAsync();
                       // return RedirectToPage("/User/viewuser");
                    }
                }
            }


            return Redirect("/User/viewuser");
        }
    }

    }

