using demo.Areas.Identity.Data;
using demo.Models;
using demo.Areas;
using demo.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using System.Data.SqlClient;
using System.Linq;
using demo.Views.reg;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Mvc.Rendering;
using demo.Migrations;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using System.Security.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V5.Pages.Account.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Net.Mail;
using System.Net;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
//using Aspose.Email;
namespace demo.Controllers
{
	public class RegNo : Controller
	{
		private readonly demoContext _context;
        private readonly UserManager<demoUser> _userManager;
        private readonly ILogger<regModel> _logger;
		private readonly EmailSettings _emailSettings;
		//public YourController(UserManager<ApplicationUser> userManager)
		//{
		//    _userManager = userManager;
		//}
		public RegNo(demoContext context, UserManager<demoUser> userManager, ILogger<regModel> logger, IOptions<EmailSettings> emailSettings)
		{
			_context = context;
            _userManager = userManager;
            _logger = logger;
			_emailSettings = emailSettings.Value;
		}
		[Route("Reset Password")]
		public IActionResult Index()
		{
			return View("/Views/User/RegNoCheck.cshtml");
		}
		[HttpPost]
		//public IActionResult PostRegNo(ForegetPassword model)
		[Route("RegNo")]
		public async Task<ActionResult> PostRegNo(ForegetPassword model)
		{
			if (ModelState.IsValid)
			{
			
				var regno = model.RegNo;
				var email = model.email;
				var user = _context.demoUser.FirstOrDefault(item => item.RegistrationId == regno);
				bool regNoExists = _context.demoUser.Any(item => item.RegistrationId == regno);
				if (regNoExists)
				{
					var code = await _userManager.GeneratePasswordResetTokenAsync(user);
					var passwordResetLink = Url.Action("ResetPassword", "RegNo",
								new { email = model.email, token = code,regno=model.RegNo }, Request.Scheme);
					_logger.Log(LogLevel.Warning,passwordResetLink);

                    await SendEmailAsync(model.email, "Reset Password",
                                $"Click here to reset your password.<a href='{HtmlEncoder.Default.Encode(passwordResetLink)}'>clicking here</a>.");
                   
                    return View("ForgotPasswordConfrmation");
				}
				else
				{
					TempData["message"] = "Not a registered user";
				}
			}
			return View("/Views/User/RegNoCheck.cshtml");
		}
		[HttpGet]
		[AllowAnonymous]
		public IActionResult ResetPassword(string token, string email, string regno)
		{
			if(token == null || email == null)
			{
				ModelState.AddModelError("", "Invalid password reset token");
			}
			return View("ResetPassword");
		}


        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)


			{

			if (ModelState.IsValid)
			{
				var regnum = model.RegNo;
				var mailreset = model.Email;
				var user = _context.demoUser.FirstOrDefault(item => item.RegistrationId == model.RegNo);
				if(user != null)
				{
					var result=await _userManager.ResetPasswordAsync(user,model.Token,model.Password);
					var emailchange= await _userManager.SetEmailAsync(user, mailreset);
					var usernamerest= await _userManager.SetUserNameAsync(user, mailreset);
                    if (result.Succeeded)
					{
						//return View();
                        return RedirectToAction("Index", "SuccessfulReset"); 
                        //return RedirectToAction("viewuser", "User");
                    }
					foreach(var error in result.Errors)
					{
						ModelState.AddModelError("",error.Description);
					}
					return	View("SuccessfulReset", model);
				}
				return View("SuccessfulReset");
			}
			return View(model);

		}
		private async Task<bool> SendEmailAsync(string email, string subject, string confirmLink)
		{
			try
			{

				MailMessage message = new MailMessage();
				SmtpClient smtpClient = new SmtpClient();

				message.From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName);
				message.To.Add(email);
				message.Subject = subject;
				message.IsBodyHtml = true;
				message.Body = confirmLink;

				smtpClient.Port = _emailSettings.Port;
				smtpClient.Host = _emailSettings.SmtpServer;

				smtpClient.EnableSsl = _emailSettings.EnableSsl;
				smtpClient.UseDefaultCredentials = false;
				smtpClient.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
				smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

				smtpClient.Send(message);
				return true;
			}
			catch (Exception ex)
			{

				return false;
			}
		}



	}


}
