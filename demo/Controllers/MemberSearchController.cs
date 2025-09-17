using demo.Areas.Identity.Data;
using demo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stimulsoft.Base;
using Stimulsoft.Data.Extensions;
using static QRCoder.PayloadGenerator;

namespace demo.Controllers
{
    public class MemberSearchController : Controller
    {
        private readonly demoContext _context;
        public MemberSearchController(demoContext context, UserManager<demoUser> userManager, ILogger<regModel> logger, IOptions<EmailSettings> emailSettings)
        {
            _context = context;

        }
        [Route("MemberSearch")]
        public IActionResult Index()
        {
            return View("/Views/User/MemberSearch.cshtml");
        }
        [Route("ViewSearch")]
        public IActionResult Search()
        {
            return View("/Views/User/ViewSearchdata.cshtml");
        }

        [Route("Search")]
        public ActionResult PostRegId(MembershipsSearch model)
        {
            var regno = model.RegNo;
            var dob = model.Dob.Date;
            var phone= model.Phone;

            var search = _context.demoUser
                .FirstOrDefault(i => i.Phone== phone && i.DateOfBirth.Date == dob);
			// var searchs = _context.demoUser.Where(i => i.RegistrationId == regno && i.DateOfBirth.Date == dob).ToList();
			var searchs = _context.demoUser.Where(i => i.Phone == phone && i.DateOfBirth.Date == dob).ToList();
			if (search != null)
            {
                var family = _context.family.Where(f => f.demoUserId == search.Id).ToList();
                long wardId = Convert.ToInt64(search.Ward);
                var ward=_context.Ward.Where(w=>w.Id== wardId).ToList();
                ViewBag.ward = ward;
                ViewBag.data = searchs;
                ViewBag.family = family;


                var userid = search.Id;

                var lastTermId = _context.Terms.OrderByDescending(t => t.TermId).Select(t => t.TermId).FirstOrDefault();
                bool paymentExist = _context.Payment.Any(p => p.TermId == lastTermId && p.UserId == userid && p.Varifiedby != null);
                if (paymentExist) { 
                    ViewBag.PaymentExist = paymentExist;
                    ViewBag.userid= userid;
                }
                else
                {
                    ViewBag.PaymentExist=false;
                }
                
                return View("/Views/User/ViewSearchdata.cshtml");
            }
            else
            {
                ViewBag.ErrorMessage = "No records found.Please enter correct data";
                return View("/Views/User/MemberSearch.cshtml");
            }

        }

    }
}