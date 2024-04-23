using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstateListingPlatform.Models;

namespace RealEstateListingPlatform.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        { 
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PropertyAmenity>().HasKey(pa => new
            {
                pa.PropertyId,
                pa.AmenityId
            });

            modelBuilder.Entity<PropertyAmenity>().HasOne(p => p.Property).WithMany(pa => pa.PropertyAmenities).HasForeignKey(p => p.PropertyId);
            modelBuilder.Entity<PropertyAmenity>().HasOne(a => a.Amenity).WithMany(pa => pa.PropertyAmenities).HasForeignKey(a => a.AmenityId);

            // Ignore ConfirmPassword property
            modelBuilder.Entity<User>()
                .Ignore(u => u.ConfirmPassword);


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<PropertyAmenity> PropertyAmenities { get; set; }

        public DbSet<Inquiry> Inquiries { get; set; }
        public DbSet<Viewing> Viewings { get; set; }

    }
}
