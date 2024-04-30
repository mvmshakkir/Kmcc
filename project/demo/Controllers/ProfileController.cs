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

namespace demo.Controllers
{
    //[Authorize]
    public class ProfileController : Controller
    {
        private readonly demoContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileController(demoContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            this._context = context;
            this._webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        //[Route("User/userprofile")]
        public IActionResult viewprofile(long? Tid)
        {
            //var lastTerm = _context.Terms.OrderByDescending(t => t.TermId).ToList();
            //ViewBag.tl = lastTerm;




            //var termsList = _context.Terms.ToList();
            //ViewBag.TermsList = termsList;
            Models.Terms term = null;
            if (Tid != null)

            {
                term = _context.Terms.FirstOrDefault(t => t.TermId == Tid);
            }
            if (term == null)
            {

                term = _context.Terms.OrderBy(t => t.TermId).FirstOrDefault();
            }

            //List<long> kidList = termsList.Select(t => t.TermId).ToList();
            //var cid = _context.Payment.FirstOrDefault(c => c.TermId == Tid);
            if (term != null)
            {
                var paidTermsList = _context.Payment.FirstOrDefault(p => p.TermId == term.TermId);

            }
            Dictionary<string, bool> userExistsDict = new Dictionary<string, bool>();
            var users = _context.demoUser.ToList();

            foreach (var item in users)
            {
                string itemId = item.Id; // Assuming item.Id is a string
                if (itemId != null && term != null && term != null)
                {
                    bool paymentExists = _context.Payment.Any(user => user.UserId == itemId && user.TermId == term.TermId);
                    userExistsDict[itemId] = paymentExists;
                }
                else
                {
                    userExistsDict[itemId] = false;
                }

            }
            ViewBag.UserExists = userExistsDict;
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null) {

                return RedirectToAction("Index", "AccountConfirmation");

            }
            else {
                string userId = userIdClaim.Value;
                var admin = _context.Users.FirstOrDefault(u => u.Id == userId && u.Email == "kmccvazhakkadofficial@gmail.com");

                if (admin != null)
                {
                    return RedirectToAction("viewuser", "User");


                }

                else
                {

                    var lastTerm = _context.Terms.OrderByDescending(t => t.TermId).FirstOrDefault();
                    var payment = _context.Payment.FirstOrDefault(p => p.UserId == userId && p.TermId == lastTerm.TermId);

                    if (payment != null && payment.Varifiedby != null)
                    {
                        ViewBag.payments = payment;
                    }

                    var productCount = _context.demoUser.Count();
                    ViewBag.ProductCount = productCount;
                    var user = _context.demoUser.FirstOrDefault(u => u.Id == userId);

                    if (user != null)
                    {
                        regModel obj = new regModel();
                        obj.Id = user.Id;
                        obj.UserImage = user.UserImage;
                        obj.Ward = user.Ward;
                        obj.FirstName = user.FirstName;
                        obj.LastName = user.LastName;
                        obj.Address = user.Address;
                        obj.City = user.City;
                        obj.Country = user.Country;
                        obj.AbroadPhone = user.AbroadPhone;
                        obj.DateOfBirth = user.DateOfBirth;
                        obj.Phone = user.Phone;
                        obj.Email = user.Email;
                        //obj.Gender = user.Gender;
                        obj.UserImage = user.UserImage;

                        obj.Age = user.Age;
                        if (user.Gender == Gender.Male)
                        {
                            obj.Gender = "Male";
                        }
                        else if (user.Gender == Gender.Female)
                        {
                            obj.Gender = "Female";
                        }

                        var terms = _context.Terms.ToList();

                        return View("/Views/User/userprofile.cshtml", new List<regModel> { obj });
                    }
                    else
                    {
                        // Handle the case where user with specified ID is not found
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
        }
    } }