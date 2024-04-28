using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateListingPlatform.Data;
using RealEstateListingPlatform.DTOs;
using RealEstateListingPlatform.Models;
using System.Data;

namespace RealEstateListingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenitiesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AmenitiesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AmenityDto>>> GetAmenities()
        {
            if (_context.Amenities == null)
            {
                var error = new { error = "No properties found matching the search criteria" };
                return NotFound(error);
            }

            var amenities = await _context.Amenities
                .Select(a => new AmenityDto
                {
                    Id = a.Id,
                    Name = a.Name
                })
                .ToListAsync();

            return Ok(amenities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AmenityDto>> GetAmenity(int id)
        {
            var amenity = await _context.Amenities
                .Select(a => new AmenityDto
                {
                    Id = a.Id,
                    Name = a.Name
                })
                .FirstOrDefaultAsync(a => a.Id == id);

            if (amenity == null)
            {
                var error = new { error = "No properties found matching the search criteria" };
                return NotFound(error);
            }

            return amenity;
        }

        //[Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public async Task<ActionResult<AmenityDto>> CreateAmenity(AmenityDto amenityDto)
        {
            var amenity = new Amenity
            {
                Id = amenityDto.Id,
                Name = amenityDto.Name,
            };

            _context.Amenities.Add(amenity);
            await _context.SaveChangesAsync();

            var createdAmenityDto = new AmenityDto
            {
                Name = amenityDto.Name,
            };

            return CreatedAtAction(nameof(GetAmenity), new { id = createdAmenityDto }, createdAmenityDto);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAmenity(int id, AmenityDto amenityDto)
        {
            if (id != amenityDto.Id)
            {
                return BadRequest();
            }


            var amenity = await _context.Amenities.FindAsync(id);
            if (amenity == null)
            {
                var error = new { error = "No properties found matching the search criteria" };
                return NotFound(error);
            }

            amenity.Name = amenityDto.Name;

            _context.Entry(amenity).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AmenityExists(id))
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

        // [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAmenity(int id)
        {
            var amenity = await _context.Amenities.FindAsync(id);
            if (amenity == null)
            {
                var error = new { error = "No properties found matching the search criteria" };
                return NotFound(error);
            }

            _context.Amenities.Remove(amenity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AmenityExists(int id)
        {
            return _context.Amenities.Any(e => e.Id == id);
        }
    }
}