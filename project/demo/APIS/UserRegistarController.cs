using System.Text.Encodings.Web;
using System.Text;
using demo.Areas.Identity.Data;
using demo.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.DiaSymReader;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using demo.Views.reg;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Net.Mail;
using System.Net;
//using System.Web.Mvc;
using Microsoft.Extensions.Options;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace demo.Controllers
{
	//[Authorize]

    [Route("api/[controller]")]
	[ApiController]
	public class UserRegistarController : ControllerBase
	{
		IWebHostEnvironment _webHostEnvironment;
		private readonly UserManager<demoUser> _userManager;
		private readonly demoContext _context;
		private readonly IUserStore<demoUser> _userStore;
		private readonly IUserEmailStore<demoUser> _emailStore;
		private readonly IEmailSender _emailSender;
		private readonly ILogger<regModel> _logger;
		private readonly SignInManager<demoUser> _signInManager;
		private readonly EmailSettings _emailSettings;
		public UserRegistarController(demoContext context,
			IWebHostEnvironment webHostEnvironment,
			 UserManager<demoUser> userManager,
			 IUserStore<demoUser> userStore,
			  IEmailSender emailSender,
				   ILogger<regModel> logger,
				   IOptions<EmailSettings> emailSettings,
			//IUserEmailStore<demoUser> emailStore,
			////IUserEmailStore<demoUser> emailStore,
			SignInManager<demoUser> signInManager)
		{
			this._context = context;

			this._userManager = userManager;
			this._userStore = userStore;
			this._emailSender = emailSender;
			_webHostEnvironment = webHostEnvironment;
			_logger = logger;
			_signInManager = signInManager;
			_emailSettings = emailSettings.Value;
			//	this._emailStore = emailStore;

		}
	
		// GET: api/<UserRegistarController>
		[HttpGet]
		public IEnumerable<string> Get()
		{

			return new string[] { "value1", "value2" };
		}
       

        [HttpPost]
		public async Task PostAsync(regModel Reg, string id)

		{

			Ward ward = _context.Ward.FirstOrDefault();
			ListCountrie ListCountrie = _context.ListCountrie.FirstOrDefault();

			//var con=_context.ListCountrie.Select(x => x.Countrie).ToList();
			//ViewBag.data = con;

			if (ModelState.IsValid)
			{

				if (Reg.Id != null)
				{

					var user = await _userManager.FindByIdAsync(Reg.Id);
					user.FirstName = Reg.FirstName;
					user.LastName = Reg.LastName;
					user.Address = Reg.Address;
					user.City = Reg.City;
					user.Country = Reg.Country;
					user.Phone = Reg.Phone;
					user.Age = Reg.Age;
					//user.Ward = Reg.Ward;
					user.RegId = Reg.RegId;
					user.UserImage = Reg.UserImage;
					user.image = Reg.image;
					user.AbroadPhone = Reg.AbroadPhone;
					user.DateOfBirth = Reg.DateOfBirth;
                    List<Ward> wardList = _context.Ward.ToList();
                    if (Reg.Ward != null)
                    {
                        // Finding a Ward in wardList with Id converted to string matching Reg.Ward
                        Ward foundWard = wardList.Find(w => w.Id.ToString() == Reg.Ward);

                        if (foundWard != null)
                        {
                            // Concatenating Wardno and Wardname from the found Ward
                            user.Ward = foundWard.Wardno + "-" + foundWard.Wardname;
                        }

                    }
					_context.SaveChanges();

					var existingFamilyRows = _context.family.Where(f => f.demoUserId == user.Id).ToList();

					// Identify rows to remove
					var rowsToRemove = existingFamilyRows.Where(existingRow => !Reg.family.Any(r => r.familyId == existingRow.familyId)).ToList();

					// Remove rows that need to be removed
					if (rowsToRemove.Any())
					{
						_context.family.RemoveRange(rowsToRemove);
						_context.SaveChanges();
					}

					// Iterate through new family members
					foreach (var newFamilyMember in Reg.family)
					{
						// Check if the new family member already exists
						var existingMember = existingFamilyRows.FirstOrDefault(f => f.familyId == newFamilyMember.familyId);

						if (existingMember != null)
						{
							// If the family member already exists, update its properties
							existingMember.Name = newFamilyMember.Name;
							existingMember.Relation = newFamilyMember.Relation;
							existingMember.Age = newFamilyMember.Age;// Update other properties as needed
							_context.SaveChanges();
						}
						else
						{
							// If the family member doesn't exist, add it to the context
							_context.family.Add(newFamilyMember);
							newFamilyMember.demoUserId = user.Id;
							_context.SaveChanges();
						}
					}

					// Update user's family to match Reg.family
					//user.family = Reg.family;

					//// Save changes
					//_context.SaveChanges();

				 //RedirectToAction("");



				}
				else
				{

					var user = CreateUser();
					
					user.FirstName = Reg.FirstName;
					user.LastName = Reg.LastName;
					user.Address = Reg.Address;
					user.City = Reg.City;
					user.Country = Reg.Country;
					user.Phone = Reg.Phone;
					user.Email = Reg.Email;
					user.Age = Reg.Age;
					user.RegDate = DateTime.UtcNow;
					
					user.AbroadPhone = Reg.AbroadPhone;
					user.DateOfBirth = Reg.DateOfBirth;
					user.RegId = Reg.RegId;
					user.UserImage = Reg.UserImage;
					user.image = Reg.image;

					List<Ward> wardList = _context.Ward.ToList();
					if (Reg.Ward != null)
					{
						// Finding a Ward in wardList with Id converted to string matching Reg.Ward
						Ward foundWard = wardList.Find(w => w.Id.ToString() == Reg.Ward);

						if (foundWard != null)
						{
							// Concatenating Wardno and Wardname from the found Ward
							user.Ward = foundWard.Wardno + "-" + foundWard.Wardname;
						}
						else
						{
							// If reg.ward is null, set it to the first value in wardList
							if (wardList.Count > 0)
							{
								user.Ward = wardList[0].Wardno + "-" + wardList[0].Wardname;
							}

						}
					}
                    if (Reg.Gender == "0")
					{
						user.Gender = Gender.Male;
					}
					else
					{
						user.Gender = Gender.Female;
					}


                    string lastRegistrationId = _context.demoUser.OrderByDescending(p => p.RegistrationId).Select(p => p.RegistrationId).FirstOrDefault();
                    long lastIdNumericPart = 0;

                    if (!string.IsNullOrEmpty(lastRegistrationId))
                    {
                        if (long.TryParse(lastRegistrationId, out lastIdNumericPart))
                        {
                            lastIdNumericPart++; // Increment by 1
                        }
                    }

                    // Set the RegistrationId for the user
                    user.RegistrationId = lastIdNumericPart.ToString();



                    await _userStore.SetUserNameAsync(user, Reg.Email, CancellationToken.None);
					//await _emailStore.SetEmailAsync(user, Reg.Email, CancellationToken.None);

					var result = await _userManager.CreateAsync(user, Reg.Password);

					if (result.Succeeded)
					{
						_logger.LogInformation("User created a new account with password.");

						var userId = await _userManager.GetUserIdAsync(user);


						if (Reg.family != null)
						{
							try
							{
								foreach (var familyMember in Reg.family)

								{
									// Assign the demoUserId for each family member
									familyMember.demoUserId = userId;
									_context.AddRange(Reg.family);

									// Save changes to the database

								}
								_context.SaveChanges();
								
							}
							catch (Exception ex)
							{

							}

							var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
							var regno=user.RegistrationId;
							code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
							var callbackUrl = Url.Page(
								"/Account/ConfirmEmail",
								pageHandler: null,
								values: new { area = "Identity", userId = userId, code = code, },
								protocol: Request.Scheme);

							//await SendEmailAsync(Reg.Email, "Confirm your email",
							//	$"Your registration number is {regno},Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
							await SendEmailAsync(Reg.Email, "Account Confirmation",
								$"Your kmcc account is successfully created.Your registration number is {regno}");
							if (_userManager.Options.SignIn.RequireConfirmedAccount)
							{
								RedirectToPage("RegisterConfirmation", new { email = Reg.Email, });
							}
							else
							{
								await _signInManager.SignInAsync(user, isPersistent: false);

							}
                            //return View("");

                        }


						// Add all family members to the context



						foreach (var error in result.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						}
					}


					


				}

			}

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

				smtpClient.EnableSsl = true;
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
			// PUT api/<UserRegistarController>/5
			[HttpPut("{id}")]
			public void Put(int id, [FromBody] string value)
			{
			}

        // DELETE api/<UserRegistarController>/5
       
                    //[HttpPost("{id}")]

                    private demoUser CreateUser()
			{
				try
				{
					return Activator.CreateInstance<demoUser>();
				}
				catch
				{
					throw new InvalidOperationException($"Can't create an instance of '{nameof(demoUser)}'. " +
						$"Ensure that '{nameof(demoUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
						$"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
				}
			}
		}
	} 
