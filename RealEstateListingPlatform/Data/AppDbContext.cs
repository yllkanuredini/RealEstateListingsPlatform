using Microsoft.EntityFrameworkCore;
using RealEstateListingPlatform.Models;

namespace RealEstateListingPlatform.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        { 
            
        }

        public DbSet<Property> Properties { get; set; }
    }
}
