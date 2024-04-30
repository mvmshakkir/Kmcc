using demo.Areas.Identity.Data;
using demo.Models;
using demo.Views.reg;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace demo.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RegistrController : ControllerBase
    {

        public RegistrController()
        {

        }
        [HttpPost]

        public async Task<bool> RegisterAsync(regModel Reg)
        {
            var user = new demoUser();

            user.FirstName = Reg.FirstName;
            user.LastName = Reg.LastName;
            user.Address = Reg.Address;
            user.City = Reg.City;
            user.Country = Reg.Country;
           user.Phone = Reg.Phone;
            user.Email = Reg.Email;
			//user.UserImage = Reg.UserImage;



			//await _userStore.SetUserNameAsync(user, Reg.Email, CancellationToken.None);
			//await _emailStore.SetEmailAsync(user, Reg.Email, CancellationToken.None);
			//var result = await _userManager.CreateAsync(user, Reg.Password);
		
			return true;
        }
    }

    }

