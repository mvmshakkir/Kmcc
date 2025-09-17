using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using demo.Areas.Identity.Data;

namespace demo.APIS
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmailCheckController : ControllerBase
	{
		private readonly demoContext _context;

		public EmailCheckController(demoContext context)
		{
			_context = context;
		}

		[HttpPost]
		public async Task<IActionResult> EmailCheck([FromBody] EmailCheckRequest request)
		{
			if (request == null || string.IsNullOrWhiteSpace(request.Phone))
			{
				return BadRequest("Email parameter is required.");
			}

			var existingUser = await _context.Users.AnyAsync(u => u.Phone == request.Phone);
			return Ok(new { exists = existingUser });
		}
	}

	public class EmailCheckRequest
	{
		public string Phone { get; set; }
	}
}