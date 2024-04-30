using System.ComponentModel.DataAnnotations;

namespace demo.Models
{
    public class Ward
    {
       

            [Key]
            public long Id { get; set; }

            public long Wardno { get; set; }
            public string Wardname { get; set; }
	
	}
}
