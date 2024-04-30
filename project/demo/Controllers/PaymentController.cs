using demo.Areas.Identity.Data;
using demo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace demo.Controllers
{
    public class PaymentController : Controller
    {
        private readonly demoContext _context;
        //this._context = context;
        public PaymentController(demoContext context)
        {
            this._context = context;


            //	this._emailStore = emailStore;

        }

        // GET: PaymentController
  

        //[Route("verification")]
        public ActionResult Index(string id, int tid)
        {
            var payment = _context.Payment.FirstOrDefault(p => p.TermId == tid && p.UserId == id);
            if (payment == null)
            {
                return NotFound();
            }
            return View("Verification", payment);
            //return View(payment);
        }
        [HttpGet]
        public ActionResult Verify(string id, int tid)
        {
            var payment = _context.Payment.FirstOrDefault(p => p.TermId == tid && p.UserId == id);
            if (payment == null)
            {
                return NotFound();
            }
            return View("Verification", payment);
            //return View(payment);
        }

        // GET: PaymentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PaymentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaymentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PaymentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PaymentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PaymentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PaymentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
