using Microsoft.AspNetCore.Mvc;
using RealEstateListingPlatform.Data;
using System.Linq;
using RealEstateListingPlatform.Models;

namespace RealEstateListingPlatform.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // Endpoint to get properties for sale
        [HttpGet("ForSale")]
        public IActionResult GetPropertiesForSale()
        {
            var propertiesForSale = _context.Properties
                .Where(p => p.Status.ToLower() == "for sale")
                .ToList();

            if (!propertiesForSale.Any())
             {
                var error = new { error = "No properties found matching the search criteria" };
                return NotFound(error);
             }

            return Ok(propertiesForSale);
        }

        // Endpoint to get properties for rent
        [HttpGet("ForRent")]
        public IActionResult GetPropertiesForRent()
        {
            var propertiesForRent = _context.Properties
                .Where(p => p.Status.ToLower() == "for rent")
                .ToList();

            if (!propertiesForRent.Any())
            {
                var error = new { error = "No properties found matching the search criteria" };
                return NotFound(error);
            }

            return Ok(propertiesForRent);
        }
    }
}