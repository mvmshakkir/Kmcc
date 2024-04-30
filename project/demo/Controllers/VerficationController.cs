using demo.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demo.Models;

namespace demo.Controllers
{
    public class VerficationController : Controller
    {
        private readonly demoContext _context;
        //this._context = context;
        public VerficationController(demoContext context)
        {
            this._context = context;


            //	this._emailStore = emailStore;

        }
        [Route("Verification/Verify")]
        [HttpGet]
        public ActionResult Verify(string id, int tid)
        {

            var admn = _context.demoUser
    .Where(u => u.UserRole == "Admin")
    .Select(u => u.FirstName); // Selecting only the first names

            ViewBag.admin = admn.ToList();

            var payment = _context.Payment.FirstOrDefault(p => p.TermId == tid && p.UserId == id);
            if (payment == null)
            {
                return NotFound();
            }
            return View("Verifications", payment);
            //return View(payment);
        }



        [Route("Verification/PostAsync")]
        [HttpPost]
        public async Task<IActionResult> PostAsync(Payment Payment)
        {
            var paymentToUpdate = await _context.Payment.FirstOrDefaultAsync(p => p.UserId == Payment.UserId && p.TermId == Payment.TermId);

            if (paymentToUpdate != null)
            {
                // Update the VerifiedBy and VerifiedDate fields
                paymentToUpdate.Varifiedby = Payment.Varifiedby;
                paymentToUpdate.Varifieddate = DateTime.Now;

                // Save changes to the database
                try
                {
                    await _context.SaveChangesAsync();
                    // Redirect to the specific view
                    return RedirectToAction("viewuser", "User");
                }
                catch (DbUpdateException ex)
                {
                    // Log the exception or handle it accordingly
                    return BadRequest("Failed to update payment: " + ex.Message);
                }
            }
            else
            {
                return NotFound(); // or any other appropriate action
            }
        }
    }
    
}

