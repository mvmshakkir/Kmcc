using demo.Areas.Identity.Data;
using System;
using System.ComponentModel;

namespace demo.Models
{
    public class userview
    {
        [DisplayName("first name")]
        public string FirstName { get; set; }
        [DisplayName("last name")]
        public string LastName { get; set; }
        [DisplayName("address")]
        public string Address { get; set; }
        [DisplayName("city")]
        public string City { get; set; }
        [DisplayName("country")]
        public string Country { get; set; }
        [DisplayName("phone")]
        public String Phone { get; set; }
        [DisplayName("gender")]
        public String Gender { get; set; }

        [DisplayName("userimage")]
        public string userimage { get; set; }

        [DisplayName("image")]
        public IFormFile Image { get; set; }

        [DisplayName("age")]
        public int age {  get; set; }
        
        

    }
    public class AppFile
    {
        public int Id { get; set; }
        public byte[] Content { get; set; }
    }

    public class ViewModel
    {
        public string? UserPicture { get; set; }

        public IFormFile file { get; set; }
    }
}
