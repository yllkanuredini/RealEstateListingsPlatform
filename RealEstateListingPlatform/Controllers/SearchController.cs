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

        // GET: api/Search?query=zipcode or city or address
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Property>>> SearchProperties(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Search query is required.");
            }

            var properties = await _context.Properties
                .Where(p => p.Address.Contains(query) || p.City.Contains(query) || p.ZipCode.Contains(query))
                .ToListAsync();

            if (!properties.Any())
            {
                return NotFound("No properties found matching the search criteria.");
            }

            return Ok(properties);
        }
    }
}


