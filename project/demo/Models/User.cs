using System.Security.Cryptography.X509Certificates;
using demo.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace demo.Models
{
    public class User : DbContext
    {
        public User()
        {
        }

        public User(DbContextOptions<User> options) : base(options)
        {
            {
            }
        }




        public DbSet<demoUser> demoUser { get; set; }
    }

}



    
