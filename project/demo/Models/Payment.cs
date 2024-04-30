
using System.ComponentModel.DataAnnotations;
using demo.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace demo.Models
{
    public class Payment
    {
        [Key]
        public long PaymentId { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public demoUser demoUser { get; set; }
        public long TermId { get; set; }
        [ForeignKey("TermId")]
        public Terms Terms { get; set; }
        public double Amount {  get; set; }
        public int Type {  get; set; }
        public DateTime Date { get; set; }
        public string Term { get; set; }

        public string referenceid {  get; set; }

        public DateTime paymentdate {  get; set; }

        public DateTime Varifieddate { get; set; }

        public string Varifiedby { get; set; }



    }
}