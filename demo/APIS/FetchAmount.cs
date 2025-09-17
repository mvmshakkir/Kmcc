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

        public FetchAmountController(demoContext context, IWebHostEnvironment webHostEnvironment)
        {
            this._context = context;
            _webHostEnvironment = webHostEnvironment;

            //	this._emailStore = emailStore;

        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task PostAsync([FromForm] Payment Payment)
        {
            if (Payment.TermPhotoFile != null)
            {
                string folder = "Payments"; // Relative folder path

                // Generate a unique file name for the uploaded photo
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Payment.TermPhotoFile.FileName);

                // Combine the folder path with the unique file name
                string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                // Ensure the directory exists, create if not
                if (!Directory.Exists(serverFolder))
                {
                    Directory.CreateDirectory(serverFolder);
                }

                // Combine the folder path with the unique file name to get the absolute path where the file will be saved on the server
                string filePath = Path.Combine(serverFolder, uniqueFileName);

                // Copy the uploaded photo to the specified folder
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Payment.TermPhotoFile.CopyToAsync(fileStream);
                }

                // Save the file path to the database or wherever needed
                // For example, you can save it to a property in your Terms model
                Payment.TermPhoto = Path.Combine(uniqueFileName);
            }

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
                Varifiedby = Payment.Varifiedby,
                TermPhoto = Payment.TermPhoto
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