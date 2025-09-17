using demo.Areas.Identity.Data;
using demo.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stimulsoft.Base;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Mvc;
using Stimulsoft.Report.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace demo.Controllers
{
	public class DesignController : Controller
	{
		private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        public DesignController(IWebHostEnvironment env, IConfiguration configuration)
		{
			_env = env;
            _configuration = configuration;
        }

		    [Route("Design/Reports")]
        [IgnoreAntiforgeryToken]
        // GET: Design
        public IActionResult Reports(string id,string filename)
        {
            return View();
        }

        //[Route("View/Index")]
        //[IgnoreAntiforgeryToken]
        //public IActionResult Indexx(string? id="IdCard")
        //{
        //	ViewBag.url = id;
        //	return View();
        //}
        [Route("Design/GetReport")]
        [IgnoreAntiforgeryToken]
        public IActionResult GetReport(string id,string filname )
		{ 
          

            var report = new StiReport();
            var reportFileName = $"{id}.mrt";
            // Map the path using IWebHostEnvironment
            var path = Path.Combine(_env.WebRootPath, "Reports", reportFileName);

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
            //report.LoadFromString(path);
            // return StiNetCoreViewer.GetReportResult(this, report);
            return StiNetCoreDesigner.GetReportResult(this, report);


        }

		public IActionResult ViewerEvent()
		{
			return StiNetCoreViewer.ViewerEventResult(this);
		}
		//[Route("DesignerEvent")]
		public IActionResult DesignerEvent()
		{
            return StiNetCoreDesigner.DesignerEventResult(this);
            //return RedirectToAction("OpenDesign", "ReportDesigner");
        }
        //[IgnoreAntiforgeryToken]
        ////[Route("View/OpenDesign")]
        //public IActionResult OpenDesign()
        //{
        //    RefNoTrackRequest refNoTrackRequest = new RefNoTrackRequest();
        //    //refNoTrackRequest.RefNo = id;
        //    return View(refNoTrackRequest);
        //}
        public IActionResult SaveReport(string id)
        {
            var report = StiNetCoreDesigner.GetReportObject(this);
            var reportFileName = $"{id}.mrt";
            // Ensure the directory exists
            var directoryPath = Path.Combine(_env.WebRootPath, "Reports");
            Directory.CreateDirectory(directoryPath); // This creates the directory if it doesn't exist

            // Map the file path
            var path = Path.Combine(directoryPath, reportFileName);

            // Save the report
            report.Save(path);

            return StiNetCoreDesigner.SaveReportResult(this);
        }




    }
}