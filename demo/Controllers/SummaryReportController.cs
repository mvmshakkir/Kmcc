using demo.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace demo.Controllers
{
	public class SummaryReportController : Controller
	{

		private readonly demoContext _context; // Replace YourDbContext with your actual DbContext

		public SummaryReportController(demoContext context)
		{
			_context = context;
		}

		[Route("Summary Report")]
		public IActionResult Index()


		{
			var terms = _context.Terms.ToList();
			ViewBag.Terms = terms;
			//var ward = _context.Ward.ToList();
			//ViewBag.Ward = ward;
			return View("/Views/User/SummaryReport.cshtml");
		}
        [Route("ViewSummary")]
        public IActionResult PostSummary(long TermId)
        {




            var userCountsByWardWithTermId = _context.demoUser
                .Join(_context.Payment, u => u.Id, p => p.UserId, (u, p) => new { u.Ward, p.TermId })
                .Where(x => x.TermId == TermId)
                .GroupBy(x => x.Ward)
                .Select(g => new { Ward = g.Key, UserCount = g.Count() })
                .ToList();

            //var userCountsByWard = _context.demoUser
            //    .GroupBy(u => u.Ward)
            //    .Select(g => new { Ward = g.Key, UserCount = g.Count() })
            //    .ToList();
            var users = _context.demoUser.ToList();  // Load the demoUser data into memory
            var wards = _context.Ward.ToList();  // Load the Ward data into memory

            var userCountsByWard = (from u in users
                                    let wardId = long.TryParse(u.Ward, out long result) ? result : 0
                                    join w in wards on wardId equals w.Id
                                    group u by new { u.Ward, w.Wardno, w.Wardname } into g
                                    select new
                                    {
                                        Ward = g.Key.Ward,
                                        WardNo = g.Key.Wardno,
                                        WardName = g.Key.Wardname,
                                        UserCount = g.Count()
                                    }).ToList();


            var combinedUserCounts = userCountsByWard
     .Select(uc => new UserSummary
     {
         Ward = uc.Ward,
         Wardno=uc.WardNo, Wardname=uc.WardName,
         UserCount_with_TermId_ = userCountsByWardWithTermId
             .FirstOrDefault(x => x.Ward == uc.Ward)?.UserCount ?? 0,
         UserCount = uc.UserCount
     }).OrderBy(uc => uc.Wardno)
     .ToList();

            ViewBag.Wardwiselist = combinedUserCounts;
            return View("/Views/User/ViewSummaryReport.cshtml", combinedUserCounts);
        }

    }
 
}
public class UserSummary
{
    public string Ward { get; set; }
    public long Wardno { get; set; }
    public string Wardname { get; set; }
    public int UserCount_with_TermId_ { get; set; }
    public int UserCount { get; set; }
}