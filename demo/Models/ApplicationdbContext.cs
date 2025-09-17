using demo.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace demo.Models;
    public class Applicatindbcontext : IdentityDbContext

    {
        public Applicatindbcontext(DbContextOptions<Applicatindbcontext> options)
            : base(options)
    { 
        }
        public DbSet<demoUser> UserDetail { get; set; }
    }
