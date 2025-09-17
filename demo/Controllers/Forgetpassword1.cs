using demo.Areas.Identity.Data;
using demo.Models;
using demo.Areas;
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

namespace demo.Controllers
{ 
public class ForgetPassword1Controller : Controller
{
	private readonly demoContext _context;
	public ForgetPassword1Controller(demoContext context)
	{
		_context = context;
	}

	public IActionResult Index()
	{
		var demoUser = _context.demoUser.ToList();
		var userss = _context.regModel.ToList();

		return View();
	}
}
}
