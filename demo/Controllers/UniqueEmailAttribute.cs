//namespace demo.Controllers
//{
//	public class UniqueEmailAttribute
//	{
//	}
//}
using demo.Areas.Identity.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace demo.Controllerse
{
	public class UniqueEmailAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var dbContext = (demoContext)validationContext.GetService(typeof(demoContext));
			var email = value as string;

			// Check if the email already exists in the database
			if (dbContext.Cordinator.Any(c => c.Email == email))
			{
				return new ValidationResult("Email address already exists.");
			}

			return ValidationResult.Success;
		}
	}
}
