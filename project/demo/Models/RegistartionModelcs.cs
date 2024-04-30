using System.ComponentModel.DataAnnotations;
using demo.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MessagePack;
using demo.Migrations;

namespace demo.Models
{
    public class regModel
    {

        public string Id { get; set; }



        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, ErrorMessage = "First Name must be between {2} and {1} characters", MinimumLength = 2)]
        [Display(Name = "FirstName")]
        public String FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Max 50 characters allowed")]
        [Display(Name = "LastName")]
        public String LastName { get; set; }

        [Required]
        [Display(Name = "profile photo")]
        public String UserImage { get; set; }
        [NotMapped]
        public IFormFile image { get; set; }


        [Required]
        //[StringLength(50, ErrorMessage = "Max 50 characters allowed")]
        [Display(Name = "age")]
        public int Age { get; set; }
      
        [Required]
        [StringLength(100, ErrorMessage = "Max 50 characters allowed")]
        [Display(Name = "Address")]
        public String Address { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Max 50 characters allowed")]
        [Display(Name = "City")]
        public String City { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Max 50 characters allowed")]
        [Display(Name = "Country")]
        public String Country { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Max 50 characters allowed")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }



        public string AbroadPhone { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }


        [Required(ErrorMessage = "Max 50 characters allowed")]
        [Display(Name = "Gender")]

        public string Gender { get; set; }





        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        //[Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        // [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        //[System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
		//public ICollection<Ward> Wards { get; set; }

		public ICollection<family> family { get; set; }
        public ICollection<Payment> Payment { get; set; }


        public string RegistrationId { get; set; }

		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]

		public DateTime RegDate { get; set; }

		public String Ward { get; set; }
        [Required]
        public long RegId { get; set; }
       /* public List<Terms> listOfTm { get; internal set; */}
        //public List<Terms> Terms { get; set; } = new List<Terms>();


    }
    public class family
    {
        //[Key]
        //public int Id { get; set; }


        [Required]

        [StringLength(50, ErrorMessage = "Max 50 characters allowed")]

        public String Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "50 characters allowed")]

        public string Relation { get; set; }


        [Required]
       // [StringLength(50, ErrorMessage = "50 characters allowed")]

        public int Age { get; set; }
        //[Key]
        //public int fid { get; set; }
        //List<Terms> listOfTm = new List<Terms>();

        [Required]
		public long familyId { get; set; }
		[JsonIgnore]
		public virtual  demoUser demoUser { get; set; }
        public string demoUserId { get; set; }

        public string RegistrationId { get; set; }
    }


   

