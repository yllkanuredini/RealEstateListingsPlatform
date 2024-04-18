using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateListingPlatform.Data;
using RealEstateListingPlatform.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RealEstateListingPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: HomeController
        /*public ActionResult Index()
        {
            // Fetching properties that are marked as available
            var availableProperties = _context.Properties
                .Where(p => p.IsAvailable)
                .Include(p => p.Photos) // Assuming there is a navigation property for Photos in the Property model
                .ToList();
            return View(availableProperties);
        }*/

        // GET: HomeController/Details/5
        /*public ActionResult Details(int id)
        {
            var property = _context.Properties
                .Include(p => p.Photos) // Include photos of the property
                .FirstOrDefault(p => p.Id == id);

            if (property == null)
            {
                return NotFound();
            }

            return View(property);
        }*/
        public ActionResult Details(int id)
        {
            var property = _context.Properties
                .Include(p => p.PropertyImages) // Include images of the property
                .FirstOrDefault(p => p.Id == id);

            if (property == null)
            {
                return NotFound();
            }

            return View(property);
        }

        // GET: HomeController/Contact
        public ActionResult Contact()
        {
            return View();
        }

        // POST: HomeController/Contact
        [HttpPost]
        [ValidateAntiForgeryToken]
        /*public ActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Logic to handle contact form submission
                // This could involve sending an email or storing inquiry details in the database
                return RedirectToAction(nameof(ThankYou));
            }

            return View(model);
        }*/

        // GET: HomeController/ThankYou
        public ActionResult ThankYou()
        {
            return View();
        }
    }
}
