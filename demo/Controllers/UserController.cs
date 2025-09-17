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
    public class UserController : Controller
    {
        private readonly demoContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(demoContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            this._context = context;
            this._webHostEnvironment= webHostEnvironment;

        }



        public IActionResult Index(long id)
        {
            var terms = _context.Terms.ToList();
            EditIdTerm EditIdTerms = new EditIdTerm();
            EditIdTerms.EditIdTerms = id;
            EditIdTerms.Terms = terms;
            return View(EditIdTerms);
           
        }
        [HttpGet]
        public IActionResult viewuser(long? Tid)
        {
             
           
            var paymentlist=_context.Payment.ToList();
             ViewBag.Paylist= paymentlist;
            

            var termsList = _context.Terms.ToList();
            ViewBag.TermsList = termsList;
            Models.Terms term = null;
            if(Tid!= null)

            {
                term = _context.Terms.FirstOrDefault(t => t.TermId == Tid);
            }
            if(term==null)
            {

                 term = _context.Terms.OrderByDescending(i => i.TermId).FirstOrDefault();
            }
            ViewBag.seleceterdterm = term;
            //List<long> kidList = termsList.Select(t => t.TermId).ToList();
            //var cid = _context.Payment.FirstOrDefault(c => c.TermId == Tid);
            if (term!=null) {
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

            Dictionary<string, bool> userVerificationDict = new Dictionary<string, bool>();
            var user = _context.demoUser.ToList();

            foreach (var item in user)
            {
                string itemId = item.Id; // Assuming item.Id is a string

                // Check if the user has a payment for the last term and it's verified
                var paymentExistsAndVerified = false;
                if (itemId != null && term != null && term != null)
                {
                    var lastTerm = _context.Terms.OrderByDescending(t => t.TermId).FirstOrDefault();
                    var payment = _context.Payment.FirstOrDefault(p => p.UserId == itemId && p.TermId == term.TermId);

                    paymentExistsAndVerified = payment != null && payment.Varifiedby != null;
                }

                userVerificationDict[itemId] = paymentExistsAndVerified;
            }

            ViewBag.UserVerification = userVerificationDict;


           


            var productCount = _context.demoUser.Count();
            ViewBag.ProductCount = productCount;
            var demoUsers = _context.demoUser.ToList();
            var termid = Tid;

            List<regModel> regModelList = new List<regModel>(); // Changed variable name

            if (termid != null)
            {
                var termdata = _context.Terms.FirstOrDefault(t => t.TermId == termid);
                var date = termdata.Term;
                string[] yearParts = date.Split('-');

                if (yearParts.Length == 2 && int.TryParse(yearParts[0], out int startYear) && int.TryParse(yearParts[1], out int endYear))
                {
                    endYear += 2000; // Assuming it's a two-digit representation (e.g., 24)
                    var StartYear = 2021;
                    Console.WriteLine($"Start year: {startYear}");
                    Console.WriteLine($"End year: {endYear}");

                    foreach (var item in demoUsers) // Changed variable name
                    {

                        regModel obj = new regModel();
                        var paymentexist = _context.Payment.FirstOrDefault(p => p.UserId == item.Id && p.TermId == termid);
                        var registration_date = item.RegDate;
                        var registration_year = registration_date.Year;

                        if (paymentexist != null)
                        {
                            obj.Id = item.Id;
                            obj.RegistrationId = item.RegistrationId;
                            obj.Ward = item.Ward;
                            obj.FirstName = item.FirstName;
                            obj.LastName = item.LastName;
                            obj.Address = item.Address;
                            obj.City = item.City;
                            obj.Country = item.Country;
                            obj.AbroadPhone = item.AbroadPhone;
                            obj.DateOfBirth = item.DateOfBirth;
                            obj.Phone = item.Phone;
                            obj.UserImage = item.UserImage;
                            obj.RegDate = item.RegDate;
                            obj.Age = item.Age;

                            obj.Gender = item.Gender == Gender.Male ? "Male" : item.Gender == Gender.Female ? "Female" : "Other";

                            var terms = _context.Terms.ToList();
                            var wd = _context.Ward.ToList();
                            ViewBag.wd = wd;

                            var cont = _context.ListCountrie.ToList();
                            ViewBag.cont = cont;

                            regModelList.Add(obj); // Changed variable name
                        }
                    }
                }
            }
            else
            {
                foreach (var item in demoUsers) // Changed variable name
                {
                    regModel obj = new regModel();

                    var paymentlast = _context.Terms.OrderByDescending(t => t.TermId).FirstOrDefault();
                    var paylst = paymentlast.TermId;
                    //var paymentexist = _context.Payment.FirstOrDefault(p => p.UserId == item.Id && p.TermId == paylst);

                    var registration_date = item.RegDate;
                    var registration_year = registration_date.Year;

                    //if (paymentexist != null)
                    //{
                        obj.Id = item.Id;
                        obj.RegistrationId = item.RegistrationId;
                        obj.Ward = item.Ward;
                        obj.FirstName = item.FirstName;
                        obj.LastName = item.LastName;
                        obj.Address = item.Address;
                        obj.City = item.City;
                        obj.Country = item.Country;
                        obj.AbroadPhone = item.AbroadPhone;
                        obj.DateOfBirth = item.DateOfBirth;
                        obj.Phone = item.Phone;
                        obj.UserImage = item.UserImage;
                        obj.RegDate = item.RegDate;
                        obj.Age = item.Age;

                        obj.Gender = item.Gender == Gender.Male ? "Male" : item.Gender == Gender.Female ? "Female" : "Other";

                        var terms = _context.Terms.ToList();
                        var wd = _context.Ward.ToList();
                        ViewBag.wd = wd;

                        var cont = _context.ListCountrie.ToList();
                        ViewBag.cont = cont;

                        regModelList.Add(obj); // Changed variable name
                   // }
                }
            }
            return View(regModelList); // Changed variable name
        }
        [HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			var demoUser = _context.demoUser.ToList();
			List<String> demoUserId = demoUser.Select(d => d.Id).ToList();
			return View("Get", new { id = demoUserId });
		
		}
	
		[Route("User/Edit/{Id}")]
		public IActionResult Edit(String Id) // Assuming Id is an integer
		{
			// Retrieve user data based on the provided Id
			var user = _context.demoUser.FirstOrDefault(d => d.Id == Id);

			if (user == null)
			{
				// Handle the case where no user with the given Id is found
				// You can return an error view or redirect to a different action
			}

			return View(user);
		}

		private void FetchData()
        {
        }
    }

    internal class sqlDataReader
    {
    }
}
