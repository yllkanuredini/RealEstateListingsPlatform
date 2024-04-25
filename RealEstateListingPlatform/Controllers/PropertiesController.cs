﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class PropertiesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public PropertiesController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetProperties()
        {
            if (_context.Properties == null)
            {
                return NotFound();
            }
            var properties = await _context.Properties
                .Select(p => new PropertyDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Type = p.Type,
                    Status = p.Status,
                    Price = p.Price,
                    Address = p.Address,
                    City = p.City,
                    Country = p.Country,
                    ZipCode = p.ZipCode,
                    Bedrooms = p.Bedrooms,
                    Bathrooms = p.Bathrooms,
                    SquareMeters = p.SquareMeters,
                    CreatedDate = p.CreatedDate,
                    PropertyImages = p.PropertyImages.Select(pi => new PropertyImageDto
                    {
                        Id = pi.Id,
                        Url = pi.Url,
                        IsMain = pi.IsMain
                    }).ToList(),
                    PropertyAmenities = p.PropertyAmenities.Select(pa => new PropertyAmenityDto
                    {
                        PropertyId = pa.PropertyId,
                        AmenityId = pa.AmenityId,
                        AmenityName = pa.Amenity.Name
                    }).ToList()
                })
                .ToListAsync();

            return Ok(properties);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyDto>> GetProperty(int id)
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
            var propertyDto = new PropertyDto
            {
                Id = property.Id,
                Title = property.Title,
                Description = property.Description,
                Type = property.Type,
                Status = property.Status,
                Price = property.Price,
                Address = property.Address,
                City = property.City,
                Country = property.Country,
                ZipCode = property.ZipCode,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                SquareMeters = property.SquareMeters,
                CreatedDate = property.CreatedDate,
                PropertyImages = property.PropertyImages.Select(pi => new PropertyImageDto
                {
                    Id = pi.Id,
                    Url = pi.Url,
                    IsMain = pi.IsMain
                }).ToList(),
                PropertyAmenities = property.PropertyAmenities.Select(pa => new PropertyAmenityDto
                {
                    PropertyId = pa.PropertyId,
                    AmenityId = pa.AmenityId,
                    AmenityName = pa.Amenity.Name
                }).ToList()
            };

            return propertyDto;
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


        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<PropertyDto>> CreateProperty(PropertyDto propertyDto)
        {
            var property = new Property
            {
                Title = propertyDto.Title,
                Description = propertyDto.Description,
                Type = propertyDto.Type,
                Status = propertyDto.Status,
                Price = propertyDto.Price,
                Address = propertyDto.Address,
                City = propertyDto.City,
                Country = propertyDto.Country,
                ZipCode = propertyDto.ZipCode,
                Bedrooms = propertyDto.Bedrooms,
                Bathrooms = propertyDto.Bathrooms,
                SquareMeters = propertyDto.SquareMeters,
                CreatedDate = DateTime.Now,
            };

            _context.Properties.Add(property);
            await _context.SaveChangesAsync();


            var createdPropertyDto = new PropertyDto
            {
                Id = property.Id,
                Title = property.Title,
                Description = property.Description,
                Type = property.Type,
                Status = property.Status,
                Price = property.Price,
                Address = property.Address,
                City = property.City,
                Country = property.Country,
                ZipCode = property.ZipCode,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                SquareMeters = property.SquareMeters,
                CreatedDate = property.CreatedDate,
            };

            return CreatedAtAction(nameof(GetProperty), new { id = createdPropertyDto.Id }, createdPropertyDto);
        }


        //[Authorize(Roles = "Admin")]
        [HttpPost("AddAmenityToProperty")]
        public async Task<ActionResult<PropertyDto>> AddAmenityToProperty(int propertyId, int amenityId)
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


            var updatedPropertyDto = new PropertyDto
            {
                Id = property.Id,
                Title = property.Title,
                Description = property.Description,
                Type = property.Type,
                Status = property.Status,
                Price = property.Price,
                Address = property.Address,
                City = property.City,
                Country = property.Country,
                ZipCode = property.ZipCode,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                SquareMeters = property.SquareMeters,
                CreatedDate = property.CreatedDate,
            };

            return updatedPropertyDto;
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProperty(int id, PropertyDto propertyDto)
        {
            if (id != propertyDto.Id)
            {
                return BadRequest();
            }

            var property = await _context.Properties.FindAsync(id);
            if (property == null)
            {
                return NotFound();
            }

            property.Title = propertyDto.Title;
            property.Description = propertyDto.Description;
            property.Type = propertyDto.Type;
            property.Status = propertyDto.Status;
            property.Price = propertyDto.Price;
            property.Address = propertyDto.Address;
            property.City = propertyDto.City;
            property.Country = propertyDto.Country;
            property.ZipCode = propertyDto.ZipCode;
            property.Bedrooms = propertyDto.Bedrooms;
            property.Bathrooms = propertyDto.Bathrooms;
            property.SquareMeters = propertyDto.SquareMeters;

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



        //[Authorize(Roles = "Admin")]
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

     

        //[Authorize(Roles = "Admin")]
        [HttpDelete("RemoveAmenityFromProperty")]
        public async Task<ActionResult<PropertyDto>> RemoveAmenityFromProperty(int propertyId, int amenityId)
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

    
            var updatedPropertyDto = new PropertyDto
            {
                Id = property.Id,
                Title = property.Title,
                Description = property.Description,
                Type = property.Type,
                Status = property.Status,
                Price = property.Price,
                Address = property.Address,
                City = property.City,
                Country = property.Country,
                ZipCode = property.ZipCode,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                SquareMeters = property.SquareMeters,
                CreatedDate = property.CreatedDate,
                PropertyImages = property.PropertyImages.Select(pi => new PropertyImageDto
                {
                    Id = pi.Id,
                    Url = pi.Url,
                    IsMain = pi.IsMain
                }).ToList(),
                PropertyAmenities = property.PropertyAmenities.Select(pa => new PropertyAmenityDto
                {
                    PropertyId = pa.PropertyId,
                    AmenityId = pa.AmenityId,
                    AmenityName = pa.Amenity.Name
                }).ToList()
            };

            return updatedPropertyDto;
        }


        private bool PropertyExists(int id)
        {
            return _context.Properties.Any(e => e.Id == id);
        }
    }
}