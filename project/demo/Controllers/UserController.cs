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
            //         Dictionary<string, long> userExistsDict = new Dictionary<string, long>();
            //var users = _context.demoUser.ToList();

            //foreach (var item in users)
            //         {
            //             string itemId = item.Id; // Assuming item.Id is a string
            //             if (itemId != null && term != null && term != null)
            //             {
            //                 long paymentExists = 0;
            //                 if (_context.Payment.Any(user => user.UserId == itemId && user.TermId == term.TermId))
            //                 {
            //                     paymentExists = _context.Payment.Where(user => user.UserId == itemId && user.TermId == term.TermId).FirstOrDefault().PaymentId;
            //                 }
            //                 userExistsDict[itemId] = paymentExists;
            //             }

            //         }
            //         ViewBag.UserExists = userExistsDict;

            Dictionary<string, bool> userExistsDict = new Dictionary<string, bool>();
            var users = _context.demoUser.ToList();

            foreach (var item in users)
            {
                string itemId = item.Id; // Assuming item.Id is a string
                //var lastTerm = _context.Terms.OrderByDescending(t => t.TermId).FirstOrDefault();
                //var payment = _context.Payment.FirstOrDefault(p => p.UserId == itemId && p.TermId == lastTerm.TermId);

                //if (payment != null && payment.Varifiedby != null)
                //{
                //    ViewBag.payment = payment;
                //}
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


            //Uid = _context.demoUser.Any(U=>U.Id==Id);
            //Uid = ue.Any(u => u.Id == Uid);



            var productCount = _context.demoUser.Count();
            ViewBag.ProductCount = productCount;
            var demoUser = _context.demoUser.ToList();
            List<regModel> regModel = new List<regModel>();
            foreach(var item in demoUser)
            {
                regModel obj = new regModel();
                obj.Id=item.Id;
                obj.RegistrationId = item.RegistrationId;
                obj.Ward=item.Ward;
                obj.FirstName = item.FirstName;
                obj.LastName = item.LastName;
                obj.Address = item.Address;
                obj.City = item.City;
                obj.Country = item.Country;
                obj.AbroadPhone=item.AbroadPhone;
                obj.DateOfBirth = item.DateOfBirth;
               obj.Phone = item.Phone;
               //obj.Gender = item.Gender;
                obj.UserImage = item.UserImage;
                


                obj.Age = item.Age;
                if (item.Gender == Gender.Male)
                {
                    obj.Gender = "Male";
                }
                else if (item.Gender == Gender.Female)
                {
                    obj.Gender = "Female"; 
                }

                //List<string> termNames = _context.Terms.Select(t => t.Term).ToList();


                var terms = _context.Terms.ToList();

                // Transform the terms into a format suitable for display in the view
               
                //long tmId = Tid;
                //bool Exist = _context.Payment.FirstOrDefault(p => p.TermId == tmId);
                ////
                ///
               
                

                




                regModel.Add(obj);

			}
            return View(regModel);
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
