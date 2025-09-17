namespace demo.Models
{
    public class QRCodes
    {
        public int Id { get; set; }
        public String UserId { get; set; }
        public byte[] ImageData { get; set; }
        public string ImagePath { get; set; }
    }
}
