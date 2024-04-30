using demo.Areas.Identity.Data;
using demo.Areas.Identity.Pages.Account;
using demo.Views.reg;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static demo.Views.reg.reg;
using demo.Controllers;
using System.Reflection.Emit;
using demo.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace demo.Areas.Identity.Data;

public class demoContext : IdentityDbContext<demoUser>


{
    public demoContext(DbContextOptions<demoContext> options)
        : base(options)
    { 
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.ApplyConfiguration(new ApplicationUserEntityCofiguration());
        builder.Entity<family>()
			.HasOne(d => d.demoUser)
			.WithMany(f => f.family)
			.HasForeignKey(f => f.demoUserId)
            ;
       
            builder.Entity<demoUser>()
                .HasMany(d => d.family)
                .WithOne(d => d.demoUser)
                .OnDelete(DeleteBehavior.Cascade);

        //    builder.Entity<Payment>()
        //.HasOne(p => p.demoUser)    
        //.WithMany(u => u.Payments)  
        //.HasForeignKey(p => p.UserId)  
        //.IsUnique(false);

          builder.Entity<Payment>()
          .HasIndex(p => p.UserId)  
            .IsUnique(false);

           builder.Entity<Payment>()
             .HasIndex(p => p.TermId)  
              .IsUnique(false);





    }
    public DbSet<demoUser> demoUser { get; set; }
    public DbSet<regModel> regModel { get; set; }
    
    public DbSet<Ward> Ward { get; set; }
	public DbSet<family> family { get; set; }
	public DbSet<Terms> Terms { get; set; }
    public DbSet<Payment> Payment { get; set; }
	public DbSet<ForegetPassword> ForegetPassword { get; set; }
    public DbSet<ResetPasswordViewModel> ResetPasswordViewModel { get; set; }
    public DbSet<QRCodes> QRCodes { get; set; }
	

    public DbSet<CountrieList> CountrieList { get; set; }
	public DbSet<ListCountrie> ListCountrie { get; set; }
	
}

public class ApplicationUserEntityCofiguration : IEntityTypeConfiguration<demoUser>
{


    public void Configure(EntityTypeBuilder<demoUser> builder)
    {
        builder.Property(x => x.FirstName).HasMaxLength(100);
        builder.Property(x => x.LastName).HasMaxLength(100);
        builder.Property(x => x.Address).HasMaxLength(500);
        builder.Property(x => x.City).HasMaxLength(500);
        builder.Property(x => x.Country).HasMaxLength(100);
        builder.Property(x => x.Phone).HasMaxLength(500);
        builder.Property(x => x.Gender).HasMaxLength(500);
        builder.Property(x => x.UserImage).HasMaxLength(500);
        builder.Property(x => x.Age).HasMaxLength(500);
        builder.Property(x => x.AbroadPhone).HasMaxLength(500);
        builder.Property(x => x.DateOfBirth).HasMaxLength(500);
        builder.Property(x=> x.Ward).HasMaxLength(500);


    }
    public void Configure(EntityTypeBuilder<family> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Relation).HasMaxLength(100);
        builder.Property(x => x.Age).HasMaxLength(100);
    }
    public void Configure(EntityTypeBuilder<Ward> builder)
    {
        builder.Property(x => x.Wardno).HasMaxLength(100);
        builder.Property(x => x.Wardname).HasMaxLength(100);
    }
	public void Configure(EntityTypeBuilder<ListCountrie> builder)
	{
		
		builder.Property(x => x.Countrie).HasMaxLength(100);
	}
}