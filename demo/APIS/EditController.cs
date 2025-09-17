using demo.Areas.Identity.Data;
using demo.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace demo.APIS
{
    [Authorize]
    [Route("api/[controller]")]
	[ApiController]
	public class EditController : ControllerBase

	{
		IWebHostEnvironment _webHostEnvironment;

		private readonly demoContext _context;

		public EditController(demoContext context)
		{
			this._context = context;


			//	this._emailStore = emailStore;

		}



		public regModel Get(string id)
		{
			var demoUser = _context.demoUser.Where(i => i.Id == id).Include(d => d.family).FirstOrDefault();

			if (demoUser != null)
			{

				var reg = new regModel
				{
					Id = id,
					FirstName = demoUser.FirstName,
					LastName = demoUser.LastName,
					Email = demoUser.Email,
					Phone = demoUser.Phone,
					Address = demoUser.Address,
					City = demoUser.City,
					Ward = demoUser.Ward,
					Country = demoUser.Country,
					UserImage = demoUser.UserImage,
					Age = demoUser.Age,
					AbroadPhone = demoUser.AbroadPhone,
					DateOfBirth = demoUser.DateOfBirth,
					Whatsapp=demoUser.Whatsapp,
					family = demoUser.family


				};

				return reg; // Ok() signifies a successful response with the user settings
			}
			else
			{
				return null;
			}
		}
		[HttpPost]
		public async Task<IActionResult> PostAsync(regModel Reg, string id)
		{
			var oldData = _context.demoUser.Where(e => e.Id == id).Include(u => u.family).FirstOrDefault();

			if (oldData != null)
			{

				var newData = new demoUser
				{
					Id = id,
					FirstName = Reg.FirstName,
					LastName = Reg.LastName,
					Email = Reg.Email,
					Phone = Reg.Phone,
					Address = Reg.Address,
					City = Reg.City,
					Ward = Reg.Ward,
					Country = Reg.Country,
					UserImage = Reg.UserImage,
					Age = Reg.Age,
					DateOfBirth = Reg.DateOfBirth,
					AbroadPhone = Reg.AbroadPhone,
					Gender = (Gender)Enum.Parse(typeof(Gender), Reg.Gender),
					family = Reg.family,
				};
				foreach (var familyMember in Reg.family)
				{
					oldData.family.Add(new family
					{

						Name = familyMember.Name,
						Age = familyMember.Age,
						Relation = familyMember.Relation
					});
				}


				_context.Entry(oldData).CurrentValues.SetValues(newData);


				await _context.SaveChangesAsync();

				return Ok("Data updated successfully.");
			}

			return NotFound("User not found.");
		}

	}
}

