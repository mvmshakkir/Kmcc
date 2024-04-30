using System.Security.Cryptography;
using demo.Areas.Identity.Data;
using demo.Migrations;
using demo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace demo.Controllers
{
    [Authorize]
    public class userloginController : Controller
    {
        private readonly demoContext _context;

        public userloginController(demoContext context)
        {
            this._context = context;
        }

        [Route("Index/userlogin")]
        public IActionResult Index(long? Tid)
        {
            var A=_context.ListCountrie.ToList();
            var userIdClaim = User.FindFirst("UserId");
            string actualUserId = userIdClaim?.Value ?? "";
            string uid = actualUserId;
            var termsList = _context.Terms.ToList();
            ViewBag.TermsList = termsList;
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


                return View("/Views/User/userlogin.cshtml", new List<regModel> { obj });
            }
            else
            {
                // Handle the case where user with specified ID is not found


            }
           
            return View("/Views/User/userlogin.cshtml");
        }

        public IActionResult Redirect(string userId)
        {
            string pid = userId;
            return View("/Views/User/userlogin.cshtml");
        }
    }
}




