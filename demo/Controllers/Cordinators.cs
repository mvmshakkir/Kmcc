//using demo.Areas.Identity.Data;
//using demo.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace demo.Controllers
//{
//    public class Cordinators : Controller
//    {
//		private readonly demoContext _context; // Replace YourDbContext with your actual DbContext
//		public Cordinators(demoContext context)
//		{
//			_context = context;
//		}

//        [HttpPost]
//        public async Task<ActionResult> PostCordinator(Cordinator Cordinator)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }
//			if (_context.Cordinator.Any(c => c.Email == Cordinator.Email))
//			{
//				ModelState.AddModelError("Email", "Email address already exists.");
//			}
//			//var ward = _context.Ward.FirstOrDefault(w => w.Id == wards.Id);

//			//if (ward != null)
//			//{

//			//    ward.Wardno = wards.Wardno;
//			//    ward.Wardname = wards.Wardname;

//			//    _context.SaveChanges();
//			//}
//			//else
//			//{

//			_context.Cordinator.Add(Cordinator);
//            //}

//            await _context.SaveChangesAsync();

//            return RedirectToAction("Index", "Cordinators");
//        }
//		[Route("Cordinator")]
//		public IActionResult Index()
//		{
//			var ward = _context.Ward.ToList();
//			ViewBag.Ward = ward;
//			return View("/Views/Cordinator/Cordinator.cshtml");
//		}

//	}
//}
using demo.Areas.Identity.Data;
using demo.Migrations;
using demo.Models;
using demo.Views.reg;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stimulsoft.Data.Extensions;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace demo.Controllers
{
	public class CordinatorsController : Controller
	{
		private readonly demoContext _context;
		private readonly UserManager<demoUser> _userManager;
		//private readonly demoContext _context;
		private readonly IUserStore<demoUser> _userStore;
		public CordinatorsController(demoContext context, UserManager<demoUser> userManager,
			 IUserStore<demoUser> userStore)
		{
			_context = context;

			this._userManager = userManager;
			this._userStore = userStore;
		}
        [Route("ViewCoordinators")]
        public IActionResult ViewCoordinator()
        {
            var ViewCoordinator = _context.demoUser.Where(i => i.IsCoordinator == true).ToList();
            List<regModel> cordinatorlist = new List<regModel>();

            foreach (var item in ViewCoordinator)
            {
                regModel Obj = new regModel();
                Obj.Id = item.Id;
                Obj.FirstName = item.FirstName +" "+ item.LastName;
                Obj.Phone = item.Phone;

                Obj.Email = item.Email;
                //Obj.Address = item.Address;
                var wardId = Convert.ToInt64(item.Ward); 
				var warduser = _context.Ward.FirstOrDefault(w => w.Id == wardId);
				Obj.Ward = $"{warduser.Wardno} {warduser.Wardname}";

                cordinatorlist.Add(Obj);
            }
            var ward = _context.Ward.ToList();
            ViewBag.Ward = ward;
            return View("/Views/User/ViewCoordinators.cshtml", cordinatorlist);
        }
        [HttpPost]
		public async Task<ActionResult> PostCordinator(string Id)
		{
			var user = await _context.demoUser.FindAsync(Id);
			if (user == null)
			{
				return NotFound();
			}

			// Set isCoordinator to true
			user.IsCoordinator = true;


			await _context.SaveChangesAsync();

			 return RedirectToAction("ViewCoordinator", "Cordinators");
		}

		[Route("Cordinator")]
		public IActionResult Index()
		{
            var wardList = _context.Ward.ToList();
            ViewBag.Ward = wardList;

            // Fetch users along with their associated ward information
            //var usersWithWard = (from user in _context.demoUser
            //                     join Ward in _context.Ward on user.Ward equals Ward.Id
            //                     select new
            //                     {
            //                         UserId = user.Id,
            //                         UserName = user.FirstName +" "+ user.LastName,
            //                         WardName = Ward.Wardname,
            //                         WardNo = Ward.Wardno
            //                     }).ToList();
            var wards = _context.Ward.ToList();

            // Fetch users into memory
            var userss = _context.demoUser.ToList();
			var usersWithWard = userss
			  .Join<demoUser, Ward, long?, UserWithWardViewModel>(
				  wards,
				  user =>
				  {
					  // Attempt to parse the string to a long
					  if (long.TryParse(user.Ward, out long wardId))
					  {
						  return wardId; // Successfully parsed
					  }
					  else
					  {
						  return null; // Return null for failed parses
					  }
				  }, // Outer key selector
				  ward => ward.Id, // Inner key selector
				  (user, ward) => new UserWithWardViewModel // Use a defined ViewModel
				  {
					  Id = user.Id,
					  UserName = user.FirstName + " " + user.LastName,
					  WardName = ward.Wardname, // Ensure correct property names
					  WardNo = ward.Wardno      // Ensure correct property names
				  })
			  .Where(x => x.WardName != null) // Filter out any results that could not match a ward
			  .ToList();

			// Assign the list to the ViewBag
			ViewBag.UsersWithWard = usersWithWard;

			var ward = _context.Ward.ToList();
			ViewBag.Ward = ward;
            var users=_context.demoUser.ToList();
            ViewBag.users=users;
			return View("/Views/Cordinator/Cordinator.cshtml");
		}
		//[Route("CordinatorDashboard")]
		[Route("CordinatorDashboard/Cordinators")]
		public IActionResult CordinatorDashboard(string userId)
		{
            if (string.IsNullOrEmpty(userId))
            {
                var userIdClaim = User.FindFirst("UserId");
                if (userIdClaim != null)
                {
                    userId = userIdClaim.Value;
                }
            }

            var ward = _context.demoUser.FirstOrDefault(i => i.Id == userId);
			var b = ward.Ward;
            var cordinator = _context.demoUser.Where(i => i.Ward == b).ToList();
            List<regModel> cordinators = new List<regModel>();

            foreach (var item in cordinator)
            {
                regModel Obj = new regModel();
				Obj.Id = item.Id;
				Obj.FirstName = item.FirstName + item.LastName;
				Obj.AbroadPhone = item.Phone;
				Obj.Email = item.AbroadPhone;
				Obj.Address = item.Address;
                var a = _context.Terms.OrderByDescending(t => t.TermId).FirstOrDefault();
                //Obj.LastName = _context.Payment.FirstOrDefault();
                var TermId = a.TermId;
                if (_context.Payment.Any(p=>p.TermId==TermId && p.UserId == item.Id && p.Varifiedby !=null))
                {
                    Obj.LastName = "verified";
                }else if(_context.Payment.Any(p => p.TermId == TermId && p.UserId == item.Id && p.Varifiedby == null)){
                    Obj.LastName = "verify";
                }
                else{
                    Obj.LastName = "pending";
                }

                cordinators.Add(Obj);
            }
            var term = _context.Terms.OrderByDescending(i => i.TermId).FirstOrDefault();
            ViewBag.seleceterdterm = term;
            var termsList = _context.Terms.ToList();
            ViewBag.TermsList = termsList;
            //return View("/Views/User/ViewWard.cshtml", wardList);

            return View("/Views/Cordinator/CordinatorDashboard.cshtml", cordinators);
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
            if (userIdClaim == null)
            {
                //var exist = ;
                return RedirectToAction("Index", "AccountConfirmation");
                //return RedirectToPage("Index", "Register");
                //var model = new EditId(); // Replace with your actual model initialization logic
                //return View("/Views/Register/Index.cshtml", model);



            }
            else
            {
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


                    var con = long.TryParse(user.Country, out long conid);
                    var contry = _context.ListCountrie.FirstOrDefault(c => c.Id == conid);


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
                        obj.Country = contry.Countrie;
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

                        return View("/Views/Cordinator/CoordinatorProfile.cshtml", new List<regModel> { obj });
                    }
                    else
                    {
                        // Handle the case where user with specified ID is not found
                        return RedirectToAction("Index", "Home");
                    }
                }

            }

        }
        public IActionResult Dashboard(long? Tid)
        {
            var A = _context.ListCountrie.ToList();
            var userIdClaim = User.FindFirst("UserId");
            string actualUserId = userIdClaim?.Value ?? "";
            string uid = actualUserId;
            var termsList = _context.Terms.ToList();
            var lastTermdata = _context.Terms.OrderByDescending(t => t.TermId).FirstOrDefault();
            var lastTermList = new List<demo.Models.Terms>(); // Specify the correct namespace

            if (lastTermdata != null)
            {
                lastTermList.Add(lastTermdata);
            }

            // Now 'lastTermList' contains only the last term (if it exists)


            ViewBag.TermsList = lastTermList;
            var paymentData = termsList.Select(term =>
                      _context.Payment.Where(payment => payment.TermId == term.TermId && payment.UserId == uid).ToList())
                      .ToList();


            ViewBag.PaymentData = paymentData;

            Models.Terms term = null;
            Models.Payment pt = null;
            if (pt == null)
            {

                pt = _context.Payment.OrderByDescending(t => t.TermId).FirstOrDefault();
            }
            if (term == null)
            {

                term = _context.Terms.OrderByDescending(t => t.TermId).FirstOrDefault();
            }

            //List<long> kidList = termsList.Select(t => t.TermId).ToList();
            //var cid = _context.Payment.FirstOrDefault(c => c.TermId == Tid);


            var user = _context.demoUser.FirstOrDefault(u => u.Id == uid);



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

                //var terms = _context.Terms.ToList();

                if (term != null)
                {
                    //var paidTermsList = _context.Payment.Where(p => p.TermId == term.TermId);
                    var termss = _context.Terms.OrderBy(t => t.TermId).ToList();


                }
                Dictionary<string, List<bool>> userExistsDict = new Dictionary<string, List<bool>>();
                var terms = _context.Terms.ToList();

                foreach (var item in terms)
                {
                    bool paymentExists = _context.Payment.Any(user => user.UserId == actualUserId && user.TermId == item.TermId);

                    // Check if the key already exists in the dictionary
                    if (!userExistsDict.ContainsKey(actualUserId))
                    {
                        userExistsDict[actualUserId] = new List<bool>();
                    }

                    // Add the boolean value to the list associated with the key
                    userExistsDict[actualUserId].Add(paymentExists);
                }

                ViewBag.UserExists = userExistsDict;

                var lastTerm = _context.Terms.OrderByDescending(t => t.TermId).FirstOrDefault();
                var payment = _context.Payment.FirstOrDefault(p => p.UserId == actualUserId && p.TermId == lastTerm.TermId);

                if (payment != null && payment.Varifiedby != null)
                {
                    ViewBag.payments = payment;
                }
                var lastterm = _context.Terms.OrderByDescending(t => t.TermId).ToList();
                //var regnoupdate = _context.demoUser.OrderByDescending(demoUser => demoUser.RegistrationId).ToList();

                int termCount = _context.Terms.Count();

              


                return View("/Views/Cordinator/CordinatorHome.cshtml", new List<regModel> { obj });
            }
            else
            {
                // Handle the case where user with specified ID is not found


            }

            return View("/Views/Cordinator/CordinatorHome.cshtml");
        }




    }
}
