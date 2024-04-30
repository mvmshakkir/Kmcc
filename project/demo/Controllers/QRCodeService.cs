using System;
using System.IO;
using System.Linq;
using demo.Areas.Identity.Data;
using demo.Models;
using QRCoder;

namespace demo.Controllers
{
    public class QRCodeService
    {
        private readonly demoContext _context;
        private readonly string _qrCodesFolder;

        public QRCodeService(demoContext context, string contentRootPath)
        {
            _context = context;
            //_qrCodesFolder = Path.Combine(contentRootPath, "qrcodes");
             _qrCodesFolder = Path.Combine(contentRootPath, "wwwroot", "qrcodes");

            // Create the "qrcodes" folder if it doesn't exist
            if (!Directory.Exists(_qrCodesFolder))
            {
                Directory.CreateDirectory(_qrCodesFolder);
            }
        }

        public byte[] GenerateQRCode(string url)
        {
            // Create QRCodeGenerator object
            QRCodeGenerator qrGenerator = new QRCodeGenerator();

            // Create QR code data from the URL
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);

            // Create QR code using the data from QRCoder
            QRCoder.QRCode qrCode = new QRCoder.QRCode(qrCodeData);

            // Generate QR code image as Bitmap
            using (var qrCodeImage = qrCode.GetGraphic(20))
            {
                // Convert Bitmap to byte array
                using (MemoryStream stream = new MemoryStream())
                {
                    qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    return stream.ToArray();
                }
            }
        }

        public string SaveQRCodeImage(string userId, byte[] qrCodeImageData)
        {
            var fileName = $"{userId}_{DateTime.Now.Ticks}.png"; // Example file name with user ID and timestamp
            var filePath = Path.Combine(_qrCodesFolder, fileName);

            // Save the QR code image to the "qrcodes" folder
            File.WriteAllBytes(filePath, qrCodeImageData);



            // Save the file path to the database
            var qrCode = new QRCodes // Assuming QRCodes is your entity class
            {
                UserId = userId,
                ImagePath = filePath // Save the file path instead of the image data
            };
            _context.QRCodes.Add(qrCode);
            _context.SaveChanges();

            return filePath;
        }

        public string GetQRCodeImagePathForUser(string userId)
        {
            var qrCode = _context.QRCodes.FirstOrDefault(q => q.UserId == userId);
            return qrCode?.ImagePath;
        }
    }
}
