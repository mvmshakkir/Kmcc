using System;
using System.IO;
using System.Linq;
using demo.Areas.Identity.Data;
using demo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace demo.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    public class IdCard : Controller
    {
        private readonly QRCodeService _qrCodeService;
        private readonly demoContext _context;

        public IdCard(demoContext context, QRCodeService qrCodeService)
        {
            _context = context;
            _qrCodeService = qrCodeService;
        }

        [Route("IdCard")]
        public IActionResult Index(string id)
        {
            var userData = _context.demoUser.FirstOrDefault(w => w.Id == id);
            if (userData != null)
            {
                var ud = "3e8ce29e-c560-4b57-b697-9be7bc13c1ff";
                var qrCodeUrl = $"http://kmccnaushad-001-site1.mysitepanel.net/IdCard?id={id}";

                // Check if QR code already exists for the user
                var existingQRCode = _context.QRCodes.FirstOrDefault(q => q.UserId == id);

                if (existingQRCode == null)
                {
                    // Generate QR code using the URL
                    var qrCodeImageData = _qrCodeService.GenerateQRCode(qrCodeUrl);

                    // Save QR code image and get its path
                    var qrCodeImagePath = _qrCodeService.SaveQRCodeImage(id, qrCodeImageData);

                    // Save QR code image path to the database
                    _context.QRCodes.Add(new QRCodes { UserId = id});
                    _context.SaveChanges();
                }

                var userModel = new demoUser
                {
                    Id = userData.Id,
                    RegistrationId = userData.RegistrationId,
                    FirstName = userData.FirstName,
                    LastName = userData.LastName,
                    Address = userData.Address,
                    AbroadPhone = userData.AbroadPhone,
                    UserImage = userData.UserImage,
                    Ward = userData.Ward,
                    Country = userData.Country,
                };

                var a = _context.family.Where(f => f.demoUserId == id).ToList();
				var familyData = _context.family.Where(f => f.demoUserId == id).Take(10).ToList();
                userModel.family = familyData;

                var ImagePath = _context.QRCodes.Where(q => q.UserId == id).Select(q => q.ImagePath).FirstOrDefault();
                //ViewBag.QRCodeImagePath = ImagePath;
                var relativeImagePath = Path.GetFileName(ImagePath);

                ViewBag.QRCodeImagePath = relativeImagePath;
                return View("/Views/User/IdCard.cshtml", userModel);
            }

            return NotFound();
        }
    }
}
