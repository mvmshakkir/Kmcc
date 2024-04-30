using System.Globalization;
using demo.Areas.Identity.Data;
using demo.Migrations;
using demo.Models;
using demo.Views.reg;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace demo.APIS
{
    [Route("api/[controller]")]
    [ApiController]
    public class FetchAmountController : ControllerBase
    {
        IWebHostEnvironment _webHostEnvironment;

        private readonly demoContext _context;

        public FetchAmountController(demoContext context)
        {
            this._context = context;


            //	this._emailStore = emailStore;

        }
        [HttpPost]
        public async Task PostAsync(Payment Payment)
        {


            Payment obj = new Payment
            {

                Amount = Payment.Amount,
                Term = Payment.Term,
                Date = DateTime.Now,
                Type = Payment.Type,
                TermId = Payment.TermId,
                UserId = Payment.UserId,
                referenceid = Payment.referenceid,
                paymentdate = Payment.paymentdate,
                Varifieddate = Payment.Varifieddate,
                Varifiedby = Payment.Varifiedby

            };


            _context.Payment.Add(obj); // Add Payment object to the context

            try
            {

                _context.SaveChanges();

                RedirectToAction("Index", "SuccessfulPayment");
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                BadRequest("Failed to save: " + ex.Message);
            }// Save changes to the database asynchronously

            // Redirect to a specific URL after processing the request
            //return RedirectToPage("viewuser", "User");
        }

        [Route("FetchAmount/Payment")]
        [HttpGet]
        public async Task GetPayment(String userId,int termid)
        {

            if (userId != null)
            {
                var payment = _context.Payment.Where(p=>p.UserId==userId && p.TermId==termid);
            }
                Payment obj = new Payment
            {

                //Amount = Payment.Amount,
                //Term = Payment.Term,
                //Date = DateTime.Now,
                //Type = Payment.Type,
                //TermId = Payment.TermId,
                //UserId = Payment.UserId,
                //referenceid = Payment.referenceid,
                //paymentdate = Payment.paymentdate,

            };

        }
    }
}