using System;
using System.IO;
using System.Linq;
using demo.Areas.Identity.Data;
using demo.Models;
using demo.Views.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace demo.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    public class IdCard : Controller
    {
        private readonly QRCodeService _qrCodeService;
        private readonly demoContext _context;
		private readonly IdSettings _idSettings;
        private readonly ILogger<IdCard> _logger;
        public IdCard(demoContext context, QRCodeService qrCodeService, IOptions<IdSettings> idSettings, ILogger<IdCard> logger)
        {
            _context = context;
            _qrCodeService = qrCodeService;
			_idSettings = idSettings.Value;
            _logger = logger;
        }
        //[Route("IdCard/ClearCookies")]
        //public IActionResult ClearCookies(string id)
        //{
        //    foreach (var cookie in Request.Cookies.Keys)
        //    {
        //        _logger.LogInformation($"Deleting cookie: {cookie}");
        //        Response.Cookies.Delete(cookie);
        //    }

        //    var remainingCookies = Request.Cookies.Keys.ToList();
        //    if (remainingCookies.Any())
        //    {
        //        _logger.LogWarning("Not all cookies were deleted:");
        //        foreach (var cookie in remainingCookies)
        //        {
        //            _logger.LogWarning($"Remaining cookie: {cookie}");
        //        }
        //    }
        //    else
        //    {
        //        _logger.LogInformation("All cookies deleted successfully.");
        //    }

        //    return RedirectToAction("Index", new { id = id });
        //}

        //    [Route("IdCard")]
        //    public IActionResult Index(string id)
        //    {
        //        var userData = _context.demoUser.FirstOrDefault(w => w.Id == id);
        //        if (userData != null)
        //        {
        ////var ud = "3e8ce29e-c560-4b57-b697-9be7bc13c1ff";
        //// var urls = _idSettings.idurl;
        ////var qrCodeUrl = $"http://kmccnaushad-001-site1.mysitepanel.net/IdCard?id={id}";
        //var urls = _idSettings.idurl;
        //var qrCodeUrl = $"{urls}{id}";

        //// Check if QR code already exists for the user
        //var existingQRCode = _context.QRCodes.FirstOrDefault(q => q.UserId == id);

        //            if (existingQRCode == null)
        //            {
        //                // Generate QR code using the URL
        //                var qrCodeImageData = _qrCodeService.GenerateQRCode(qrCodeUrl);

        //                // Save QR code image and get its path
        //                var qrCodeImagePath = _qrCodeService.SaveQRCodeImage(id, qrCodeImageData);

        //                // Save QR code image path to the database
        //                _context.QRCodes.Add(new QRCodes { UserId = id});
        //                _context.SaveChanges();
        //            }

        //            var war = long.TryParse(userData.Ward, out long wardId);
        //            var ward = _context.Ward.FirstOrDefault(w => w.Id == wardId);

        //            var con= long.TryParse(userData.Country, out long conid);
        //            var contry = _context.ListCountrie.FirstOrDefault(c => c.Id == conid);
        //            //var wd = _context.Ward.FirstOrDefault(w => w.Id == war);
        //            var userModel = new demoUser
        //            {
        //                Id = userData.Id,
        //                RegistrationId = userData.RegistrationId,
        //                FirstName = userData.FirstName,
        //                LastName = userData.LastName,
        //                Address = userData.Address,
        //                AbroadPhone = userData.AbroadPhone,
        //                UserImage = userData.UserImage,

        //                //var wd=_context.Ward.FirstOrDefault(w=>w.Id==userData.Ward),

        //                Ward =  ward.Wardno+ "-" + ward.Wardname ,
        //                Country = contry.Countrie,
        //            };

        //            //var ward=_context.Ward.ToList();
        //            //ViewBag.Ward = ward;

        //            var a = _context.family.Where(f => f.demoUserId == id).ToList();
        //var familyData = _context.family.Where(f => f.demoUserId == id).Take(10).ToList();
        //            userModel.family = familyData;

        //            var ImagePath = _context.QRCodes.Where(q => q.UserId == id).Select(q => q.ImagePath).FirstOrDefault();
        //            //ViewBag.QRCodeImagePath = ImagePath;
        //            var relativeImagePath = Path.GetFileName(ImagePath);

        //            ViewBag.QRCodeImagePath = relativeImagePath;
        //            //return View("/Views/User/IdCard.cshtml", userModel);
        //            var jsonSettings = new JsonSerializerSettings
        //            {
        //                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //            };

        //            // Store data in TempData
        //            TempData["UserModel"] = JsonConvert.SerializeObject(userModel, jsonSettings);
        //            TempData["QRCodeImagePath"] = relativeImagePath;

        //            // Redirect to the GetReport method
        //            return RedirectToAction("GetReport", "View", new { id = userModel.Id });
        //        }

        //        return NotFound();
        //    }
        [Route("IdCard")]
        public IActionResult Index(string id,string filenames)
        {
            var filename = filenames;
            var userData = _context.demoUser.FirstOrDefault(w => w.Id == id);
            if (userData != null)
            {
                var urls = _idSettings.idurl;
                var qrCodeUrl = $"{urls}{id}";

                var existingQRCode = _context.QRCodes.FirstOrDefault(q => q.UserId == id);
                if (existingQRCode == null)
                {
                    var qrCodeImageData = _qrCodeService.GenerateQRCode(qrCodeUrl);
                    var qrCodeImagePath = _qrCodeService.SaveQRCodeImage(id, qrCodeImageData);
                    _context.QRCodes.Add(new QRCodes { UserId = id, ImagePath = qrCodeImagePath });
                    _context.SaveChanges();
                }

                var ward = _context.Ward.FirstOrDefault(w => w.Id == long.Parse(userData.Ward));
                var country = _context.ListCountrie.FirstOrDefault(c => c.Id == long.Parse(userData.Country));

                var userModel = new demoUser
                {
                    Id = userData.Id,
                    RegistrationId = userData.RegistrationId,
                    FirstName = userData.FirstName,
                    LastName = userData.LastName,
                    Address = userData.Address,
                    AbroadPhone = userData.AbroadPhone,
                    UserImage = userData.UserImage,
                    Ward = ward.Wardno + "-" + ward.Wardname,
                    Country = country.Countrie,
                    family = _context.family.Where(f => f.demoUserId == id).Take(10).ToList()
                };

                var imagePath = _context.QRCodes.Where(q => q.UserId == id).Select(q => q.ImagePath).FirstOrDefault();
                var relativeImagePath = Path.GetFileName(imagePath);
                ViewBag.QRCodeImagePath = relativeImagePath;

                //var jsonSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

                //TempData["UserModel"] = JsonConvert.SerializeObject(userModel, jsonSettings);
                //TempData["QRCodeImagePath"] = relativeImagePath;

                return RedirectToAction("Index", "View", new { id = userModel.Id , filename = filename });
            }
            return RedirectToAction("Index", "View", new { id =id, filename = filename });
            //return NotFound();
        }


    }
}
