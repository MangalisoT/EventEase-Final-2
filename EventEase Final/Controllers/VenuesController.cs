using EventEase_Final.Data;
using EventEase_Final.Models;
using EventEase_Final.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEase_Final.Controllers
{
    public class VenuesController : Controller
    {
        private readonly EventEase_FinalContext _context;
        private readonly BlobService _service;

        public VenuesController(EventEase_FinalContext context, IConfiguration configuration)
        {
            _context = context;
            _service = new BlobService(configuration);
        }

        // GET: Venues
        // GET: Venues
        public async Task<IActionResult> Index(string searchString)
        {
            if (_context.Venue == null)
            {
                return Problem("Entity set 'EventEase_FinalContext.Venue'  is null.");
            }

            var venues = from v in _context.Venue
                         select v;

            if (!String.IsNullOrEmpty(searchString))
            {
                venues = venues.Where(v => v.VenueLocation.ToUpper().Contains(searchString.ToUpper()) ||
                                            v.VenueName.ToUpper().Contains(searchString.ToUpper()));
            }

            return View(await venues.ToListAsync());
        }


        // GET: Venues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venue
                .FirstOrDefaultAsync(m => m.Venue_ID == id);
            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        // GET: Venues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Venues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VenueViewModel venue, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var imageUrl = await _service.UploadFileAsync(venue.ImageFile, cancellationToken);

                var entity = new Venue
                {
                    VenueName = venue.VenueName,
                    VenueCapacity = venue.VenueCapacity,
                    VenueLocation = venue.VenueLocation,
                    VenueImage = imageUrl,
                };

                _context.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        // GET: Venues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venue.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }
            var vm = new VenueViewModel
            {
                Venue_ID = venue.Venue_ID,
                VenueName = venue.VenueName,
                VenueCapacity = venue.VenueCapacity,
                VenueLocation = venue.VenueLocation,
                ImageUrl = venue.VenueImage
            };
            return View(vm);
        }

        // POST: Venues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VenueViewModel venue, CancellationToken cancellationToken)
        {
            if (id != venue.Venue_ID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(venue);
            }

            try
            {
                var imageUrl = await _service.UploadFileAsync(venue.ImageFile, cancellationToken);
                var entity = await _context.Venue.FindAsync(new object?[] { id }, cancellationToken: cancellationToken);

                entity.VenueImage = imageUrl;
                entity.VenueName = venue.VenueName;
                entity.VenueCapacity = venue.VenueCapacity;
                entity.VenueLocation = venue.VenueLocation;

                _context.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VenueExists(venue.Venue_ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Venues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venue
                .FirstOrDefaultAsync(m => m.Venue_ID == id);
            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        // POST: Venues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool hasBooked = await _context.Booking.AnyAsync(bk => bk.Venue_ID == id);

            if (hasBooked)
            {
                var venue = await _context.Venue.FindAsync(id);
                ModelState.AddModelError("", "Cannot delete this venue; there are existing venue records.");

                return View(venue);
            }
            var venueToDelete = await _context.Venue.FindAsync(id);
            _context.Venue.Remove(venueToDelete);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VenueExists(int id)
        {
            return _context.Venue.Any(e => e.Venue_ID == id);
        }
    }
}
