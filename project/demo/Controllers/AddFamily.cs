using demo.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using static demo.Views.reg.reg;
using demo.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using demo.Areas.Identity.Pages.Account;
using demo.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Runtime.InteropServices;
using demo.Views.reg;
using System.Security.Cryptography.X509Certificates;

namespace demo.Controllers
{
    public class AddFamily : Controller
    {
        private readonly demoContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AddFamily(demoContext context,
           IWebHostEnvironment webHostEnvironment)
        {
            this._context = context;
            this._webHostEnvironment = webHostEnvironment;


        }
        [HttpPost]
        public async Task SignUp(regModel regModel)
        {
            var user = new regModel()

            {
                FirstName = regModel.FirstName,
                LastName = regModel.LastName,
                Address = regModel.Address,
                City = regModel.City,
                Country = regModel.Country,
                Phone = regModel.Phone,
                Email = regModel.Email,
				Age = regModel.Age,
                Password = regModel.Password,
                ConfirmPassword = regModel.Password,

            };
            
        }


    }
}     