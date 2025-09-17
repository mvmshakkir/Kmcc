using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using demo.Models;
using Microsoft.AspNetCore.Identity;

namespace demo.Areas.Identity.Data;



// Add profile data for application users by adding properties to the demoUser class
public class demoUser : IdentityUser
{

    //
    [Required(ErrorMessage = "First Name is required")]
    [StringLength(50, ErrorMessage = "First Name must be between {2} and {1} characters", MinimumLength = 2)]
  
    public string FirstName {  get; set; } 
    public string LastName { get; set; } 

    public string Address { get; set; }


	public String Ward { get; set; }

	public string City { get; set; }
    public string Country { get; set; }
    public String Phone { get; set; }
    public String AbroadPhone { get; set; }
	public string Whatsapp { get; set; }

	[DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    public Gender Gender {  get; set; }

    public string UserImage { get; set; }

    [NotMapped]
    public IFormFile image { get; set; }


    public int Age { get; set; }
    
    public ICollection<family> family	{ get; set; }
    public Payment Payment { get; set; }
    //public String WardName { get; set; }

    //public int familyid { get; set; }
    [Key]
	public string RegistrationId { get; set; }

	[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]

	public DateTime RegDate { get; set; }
    [Required]
    public long RegId { get; set; }
	public bool IsCoordinator { get; set; }
	public string UserRole { get; set; }

    public PravasiStatus PravasiStatus { get; set; }
}
public enum Gender
{
    Male,
    Female
}
public enum PravasiStatus
{
    Pravasi,
    ExPravasi
}

