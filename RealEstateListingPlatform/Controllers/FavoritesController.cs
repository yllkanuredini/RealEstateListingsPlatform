using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateListingPlatform.Models;
using RealEstateListingPlatform.Data;
using Microsoft.EntityFrameworkCore;

namespace RealEstateListingPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoritesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FavoritesController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/Favorites
        [HttpPost]
        public async Task<ActionResult<FavoriteListing>> PostFavoriteListing(FavoriteListing favoriteListing)
        {
            _context.FavoriteListings.Add(favoriteListing);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFavoriteListing", new { id = favoriteListing.Id }, favoriteListing);
        }

        // GET: api/Favorites/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<Listing>>> GetFavoriteListings(int userId)
        {
            var favoriteListings = await _context.FavoriteListings
                .Where(f => f.UserId == userId)
                .Select(f => f.Listing)
                .ToListAsync();

            return Ok(favoriteListings);
        }

    }
}


