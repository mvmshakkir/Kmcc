using demo.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace demo.Controllers
{
    [Authorize]
    public class DashBoard : Controller
    {
        private readonly demoContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DashBoard(demoContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            this._context = context;
            this._webHostEnvironment = webHostEnvironment;

        }
        [Route("User/DashBoard")]
        public IActionResult Index()
        {
            {
                var productCount = _context.demoUser.Count();
                ViewBag.ProductCount = productCount;
                var paymentcount = _context.Payment.OrderBy(u => u.TermId).Count();
                ViewBag.PaymentCount = paymentcount;
            }
            return View("/Views/User/DashBoard.cshtml");
        }
        [HttpGet]
        public IActionResult GetAvailableYears()
        {
            var years = _context.Payment
                .Select(p => p.Date.Year)
                .Distinct()
                .OrderBy(y => y)
                .ToList();

            return Json(years);
        }
        [HttpGet]
        public IActionResult GetMonthlyData(int year)
        {
            // List of all 12 months
            var allMonths = Enumerable.Range(1, 12).ToList();

            // Fetch monthly data from the database
            var monthlyData = _context.Payment
                .Where(p => p.Date.Year == year)
                .GroupBy(p => p.Date.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalAmount = g.Sum(p => p.Amount)
                })
                .ToList();

            // Create a dictionary with the month number as the key and amount as the value
            var monthlyDictionary = monthlyData.ToDictionary(m => m.Month, m => m.TotalAmount);

            // Ensure every month from 1 to 12 is represented, including those with no payments
            var monthsWithAmounts = allMonths.Select(month =>
            {
                return new
                {
                    Month = month,
                    Amount = monthlyDictionary.ContainsKey(month) ? monthlyDictionary[month] : 0 // If no data for a month, set amount to 0
                };
            }).ToList();

            // Convert the data for use in the chart
            // var months = monthsWithAmounts.Select(m => m.Month.ToString("00")).ToList(); // Format month as "01", "02", ..., "12"
            var months = monthsWithAmounts
                        .Select(m => new DateTime(2020, m.Month, 1).ToString("MMM")) // Use DateTime to get the month abbreviation
                        .ToList();
            var totalAmounts = monthsWithAmounts.Select(m => m.Amount).ToList();

            return Json(new { months, totalAmounts });
        }
    }
}