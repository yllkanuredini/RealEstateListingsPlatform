using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateListingPlatform.Data;
using RealEstateListingPlatform.Models;

namespace RealEstateListingPlatform.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PropertiesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Property>>> GetProperties()
        {
            if (_context.Properties == null)
            {
                return NotFound();
            }

            return await _context.Properties.Include(p => p.PropertyAmenities).Include(p => p.PropertyImages).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Property>> GetProperty(int id)
        {
            var property = await _context.Properties
                .Include(p => p.PropertyImages)
                .Include(p => p.PropertyAmenities)
                .ThenInclude(pa => pa.Amenity)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (property == null)
            {
                return NotFound();
            }

            property.PropertyAmenities = property.PropertyAmenities?.Where(pa => pa.Amenity != null).ToList();

            return property;
        }

        [HttpGet("{id}/amenities")]
        public async Task<ActionResult<IEnumerable<Amenity>>> GetPropertyAmenities(int id)
        {
            var property = await _context.Properties.FindAsync(id);

            if (property == null)
            {
                return NotFound();
            }

            return await _context.Properties
                .Where(p => p.Id == id)
                .Include(p => p.PropertyAmenities)
                .SelectMany(p => p.PropertyAmenities.Select(pa => pa.Amenity))
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Property>> CreateProperty(Property property)
        {
            _context.Properties.Add(property);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProperty), new { id = property.Id }, property);
        }

        [HttpPost("AddAmenityToProperty")]
        public async Task<ActionResult<PropertyAmenity>> AddAmenityToProperty(int propertyId, int amenityId)
        {
            var property = await _context.Properties.FindAsync(propertyId);
            if (property == null)
            {
                return NotFound("Property not found.");
            }

            var amenity = await _context.Amenities.FindAsync(amenityId);
            if (amenity == null)
            {
                return NotFound("Amenity not found.");
            }

            if (property.PropertyAmenities == null)
            {
                property.PropertyAmenities = new List<PropertyAmenity>();
            }

            property.PropertyAmenities.Add(new PropertyAmenity { PropertyId = propertyId, AmenityId = amenityId });
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProperty(int id, Property property)
        {
            if (id != property.Id)
            {
                return BadRequest();
            }

            _context.Entry(property).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PropertyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property == null)
            {
                return NotFound();
            }

            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("RemoveAmenityFromProperty")]
        public async Task<IActionResult> RemoveAmenityFromProperty(int propertyId, int amenityId)
        {
            var property = await _context.Properties.Include(p => p.PropertyAmenities)
                                                    .FirstOrDefaultAsync(p => p.Id == propertyId);

            if (property == null)
            {
                return NotFound("Property not found.");
            }

            var propertyAmenity = property.PropertyAmenities.FirstOrDefault(pa => pa.AmenityId == amenityId);

            if (propertyAmenity == null)
            {
                return NotFound("Amenity not found for this property.");
            }

            _context.PropertyAmenities.Remove(propertyAmenity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("GetFiltered")]
        public async Task<IActionResult> GetFilteredProducts([FromQuery] string status)
        {
            try
            {
              
                var properties = await _context.Properties.Include(p => p.PropertyAmenities).
             Where(p => status.Equals("ALL") || p.Status == status).ToListAsync();

                if(!properties.Any())
                {
                    return NotFound();
                }
                return Ok(properties);

            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        
        }
        private bool PropertyExists(int id)
        {
            return _context.Properties.Any(e => e.Id == id);
        }
    }
}
