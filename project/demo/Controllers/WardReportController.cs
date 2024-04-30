using demo.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace demo.Controllers
{
	public class WardReportController : Controller
	{

		private readonly demoContext _context; // Replace YourDbContext with your actual DbContext

		public WardReportController(demoContext context)
		{
			_context = context;
		}

		[Route("Ward Report")]
		public IActionResult Index()


		{
			var terms = _context.Terms.ToList();
			ViewBag.Terms = terms;
			var ward = _context.Ward.ToList();
			ViewBag.Ward = ward;
			return View("/Views/User/WardWiseReport.cshtml");
		}
        [Route("WardWiseReport")]
        public IActionResult PostWard(string wardId, long TermId, string statusid)
        {
            List<demoUser> usersWithWard = new List<demoUser>();

            if (wardId != null)
            {
                // Extract the numeric ID part from wardId
                string[] parts = wardId.Split('-');
                if (parts.Length >= 1 && long.TryParse(parts[0], out long wardIdValue))
                {
                    // Find all users in demoUser table whose Ward column contains the specified ID
                    usersWithWard = _context.demoUser.Where(w => w.Ward.StartsWith(wardIdValue + "-")).ToList();

                    // Create lists to hold users with payments and without payments
                    List<demoUser> usersWithPayment = new List<demoUser>();
                    List<demoUser> usersWithoutPayment = new List<demoUser>();

                    // Check each user in usersWithWard
                    foreach (var user in usersWithWard)
                    {
                        // Check the value of statusid to set the appropriate message in the ViewBag
                        if (statusid == "1")
                        {
                            // Check if payment exists for the user
                            bool paymentExists = _context.Payment.Any(p => p.TermId == TermId && p.UserId == user.Id);
                            if (paymentExists)
                            {
                                // Add the user to the list of users with payments
                                usersWithPayment.Add(user);
                                ViewBag.UsersWithPayment = usersWithPayment;
                                ViewBag.Message = "Payment exists.";
                            }
                        }
                        else if (statusid == "2")
                        {
                            // Check if payment exists for the user
                            bool paymentExists = _context.Payment.Any(p => p.TermId == TermId && p.UserId == user.Id);
                            if (!paymentExists)
                            {
                                // Add the user to the list of users without payments
                                usersWithoutPayment.Add(user);
                                ViewBag.Message = "Payment does not exist.";
                                ViewBag.UsersWithoutPayment = usersWithoutPayment;
                            }
                        }
                        else
                        {
                            // Handle invalid statusid
                            ViewBag.Message = "Invalid statusid.";
                        }
                    }

                    // Pass the lists of users with and without payments to the view
                   
                   
                }
            }

            // Pass the list of users to the view
            return View("/Views/User/ViewWardWiseReport.cshtml", usersWithWard);
        }

    }
}
