using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;
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

        // General search endpoint
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Property>>> SearchProperties(
            string? type,
            decimal? minPrice,
            decimal? maxPrice,
            string? location,
            string? amenities)
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
                var error = new {error = "No properties found matching the search criteria."};
                return NotFound(error);
            }

            return Ok(properties);
        }

        // Filter by property type
        [HttpGet("ByType/{type}")]
        public async Task<ActionResult<IEnumerable<Property>>> FilterByType(string type)
        {
            var properties = await _context.Properties
                .Where(p => p.Type == type)
                .ToListAsync();

            if (!properties.Any())
            {
                var error = new { error = "No properties found matching the search criteria." };
                return NotFound(error);
            }

            return Ok(properties);
        }

        // Filter by price range
        [HttpGet("ByPriceRange")]
        public async Task<ActionResult<IEnumerable<Property>>> FilterByPriceRange(decimal minPrice, decimal maxPrice)
        {
            var properties = await _context.Properties
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .ToListAsync();

            if (!properties.Any())
            {
                var error = new { error = "No properties found within the specified price range." };
                return NotFound(error);
            }

            return Ok(properties);
        }

        // Filter by location
        [HttpGet("ByLocation/{location}")]
        public async Task<ActionResult<IEnumerable<Property>>> FilterByLocation(string location)
        {
            var properties = await _context.Properties
                .Where(p => p.Address.Contains(location) ||
                            p.City.Contains(location) ||
                            p.ZipCode.Contains(location))
                .ToListAsync();

            if (!properties.Any())
            {
                var error = new { error = "No properties found in the specified location." };
                return NotFound(error);
            }

            return Ok(properties);
        }

        // Filter by amenities
        [HttpGet("ByAmenities")]
        public async Task<ActionResult<IEnumerable<Property>>> FilterByAmenities([FromQuery] string amenities)
        {
            var amenitiesList = amenities.Split(',').Select(a => a.Trim()).ToList();
            var properties = await _context.Properties
                .Where(p => p.PropertyAmenities.Any(a => amenitiesList.Contains(a.Amenity.Name)))
                .ToListAsync();

            if (!properties.Any())
            {
                var error = new { error = "No properties found with the specified amenities." };
                return NotFound(error);
            }

            return Ok(properties);
        }
    }
}
