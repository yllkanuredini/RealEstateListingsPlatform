using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateListingPlatform.Data;
using RealEstateListingPlatform.Models;
using System.Data;
using System.Linq;

namespace RealEstateListingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyImagesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PropertyImagesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyImage>>> GetPropertyImages()
        {
            var propertyImages = await _context.PropertyImages
                .Include(pi => pi.Property)
                .ToListAsync();

            return Ok(propertyImages);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyImage>> GetPropertyImageById(int id)
        {
            var propertyImage = await _context.PropertyImages
                .Include(pi => pi.Property)
                .FirstOrDefaultAsync(pi => pi.Id == id);

            if (propertyImage == null)
            {
                return NotFound();
            }

            return propertyImage;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<PropertyImage>> AddImageToProperty(int propertyId, string url, bool isMain)
        {
            var property = await _context.Properties.FindAsync(propertyId);
            if (property == null)
            {
                return NotFound("Property not found.");
            }

            var propertyImage = new PropertyImage
            {
                Url = url,
                IsMain = isMain,
                PropertyId = propertyId
            };

            _context.PropertyImages.Add(propertyImage);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPropertyImageById), new { id = propertyImage.Id }, propertyImage);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePropertyImage(int id, PropertyImage propertyImage)
        {
            if (id != propertyImage.Id)
            {
                return BadRequest();
            }

            _context.Entry(propertyImage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PropertyImageExists(id))
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

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePropertyImage(int id)
        {
            var propertyImage = await _context.PropertyImages.FindAsync(id);

            if (propertyImage == null)
            {
                return NotFound();
            }

            _context.PropertyImages.Remove(propertyImage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PropertyImageExists(int id)
        {
            return _context.PropertyImages.Any(e => e.Id == id);
        }
    }
}