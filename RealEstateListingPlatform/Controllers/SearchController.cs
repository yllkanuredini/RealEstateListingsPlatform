using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateListingPlatform.Data;
using RealEstateListingPlatform.Models;

namespace RealEstateListingPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SearchController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Search
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Property>>> SearchProperties(
            string type,
            decimal? minPrice,
            decimal? maxPrice,
            string location,
            string amenities)
        {
            var query = _context.Properties.AsQueryable();

            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(p => p.Type == type);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(p => p.Address.Contains(location) ||
                                         p.City.Contains(location) ||
                                         p.ZipCode.Contains(location));
            }

            if (!string.IsNullOrEmpty(amenities))
            {
                var amenitiesList = amenities.Split(',').Select(a => a.Trim()).ToList();
                query = query.Where(p => p.PropertyAmenities.Any(a => amenitiesList.Contains(a.Amenity.Name)));
            }

            var properties = await query.ToListAsync();

            if (!properties.Any())
            {
                return NotFound("No properties found matching the search criteria.");
            }

            return Ok(properties);
        }
    }
}


