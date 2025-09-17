using demo.Models;
using Microsoft.AspNetCore.Mvc;
using Stimulsoft.Base;
using Stimulsoft.Report.Mvc;
using Stimulsoft.Report;
using System.Data;
using System.Text;
using Stimulsoft.Report.Dictionary;
using demo.Areas.Identity.Data;
using Microsoft.Extensions.Options;

namespace demo.Controllers
{
    public class ViewController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly demoContext _context;
        private readonly IdSettings _idSettings;
        private readonly IConfiguration _configuration;
        public ViewController(IWebHostEnvironment env, demoContext context, IOptions<IdSettings> idSettings, IConfiguration configuration)
        {
            _context = context;
            _env = env;
            _idSettings = idSettings.Value;
            _configuration = configuration;
        }
        [Route("View/Index")]
        [IgnoreAntiforgeryToken]
        // GET: View
        public IActionResult Index(string? id, string? filename)
        {
            ViewBag.url = id;
            return View();
        }
      
        [Route("View/GetReport")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> GetReport(string? id,string filename)
        {
            var report = new StiReport();
            var reportFileName = $"{filename}.mrt";

            // Map the path using IWebHostEnvironment
            var path = Path.Combine(_env.WebRootPath, "Reports", reportFileName);

            // Map the path using IWebHostEnvironment
            //var path = Path.Combine(_env.WebRootPath, "Reports", "Report.mrt");

            // Check if the file exists
            if (!System.IO.File.Exists(path))
            {
                return NotFound(); // Return 404 if the file does not exist
            }

            // Load the report template
            report.Load(path);
            string newConnectionStrings = _configuration.GetConnectionString("demoContextConnection");
            string newConnectionString = "Integrated Security=False; Data Source=SQL5108.site4now.net; Initial Catalog=db_aa391e_kmcc; User ID=db_aa391e_kmcc_admin; Password=admin123; Trusted_Connection=false; MultipleActiveResultSets=false;";

            foreach (var database in report.Dictionary.Databases)
            {
                if (database is Stimulsoft.Report.Dictionary.StiSqlDatabase sqlDatabase)
                {
                    // Update the connection string
                    sqlDatabase.ConnectionString = newConnectionStrings;
                    break; // Exit the loop after updating the connection string
                }
            }
            //foreach (var dataSource in report.Dictionary.Databases)
            //{
            //    if (dataSource is Stimulsoft.Report.Dictionary.StiSqlDatabase sqlDatabase)
            //    {
            //        // Fetch the connection string
            //        string currentConnectionString = sqlDatabase.ConnectionString;
            //        // You can break the loop if you only need one connection string
            //        break;
            //    }
            //}

            var userData = _context.demoUser.FirstOrDefault(w => w.Id == id);
            string userimage=userData.UserImage;
            var qr = _context.QRCodes.FirstOrDefault(w => w.UserId == id);
            string qrcode = qr.ImagePath;
            //string qrcode = "C:\\Users\\user\\Desktop\\kmccproject\\kmccproject\\project\\demo\\wwwroot\\qrcodes\\b3325a1d-9e79-44b4-9512-8de7c6bb67be_638539581226970222.png";
            string qrfilename = Path.GetFileName(qrcode);
            var WardId = (int.TryParse(userData.Ward, out int wardId));
            var ConId= (int.TryParse(userData.Country, out int conId));
            var aa = "be669453-dbaa-4941-8dc3-abddb669f08f-56";
            report["id"] = id;
            report["WardId"] = wardId;
            report["ConId"] = conId;
            report["UserId"] = id;
            //var urls = _idSettings.imgurl;
            //report["imgurl"] = "https://globalkmccvazhakkad.org/uploads/75815572-3b62-4bc1-9bf5-8c8591ca752c.jpg";
            report["QRCode"] = qrfilename;

            
            report["imgurl"] = userimage;
            
            report.Render();
            // Return the report result to be displayed in the viewer
            return StiNetCoreViewer.GetReportResult(this, report);
        }
        [Route("View/ViewerEvent")]
        [IgnoreAntiforgeryToken]
        public IActionResult ViewerEvent()
        {
            var k = StiNetCoreViewer.ViewerEventResult(this);
            var m = k.ToString();

            return k;
        }
        [IgnoreAntiforgeryToken]
        [Route("View/Design")]
        public IActionResult Design(string? id,string filename)
        {

            return RedirectToAction("OpenDesign", "View", new { id , filename });
        }
        [IgnoreAntiforgeryToken]
        [Route("View/OpenDesign")]
        public IActionResult OpenDesign(string? id,string filename)
        {
            RefNoTrackRequest refNoTrackRequest = new RefNoTrackRequest();
            refNoTrackRequest.RefNo = filename;
            return View(refNoTrackRequest);
        }
     

    }
}
