using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventEase_Final.Data;
using EventEase_Final.Models;

namespace EventEase_Final.Controllers
{
    public class BookingsController : Controller
    {
        private readonly EventEase_FinalContext _context;

        public BookingsController(EventEase_FinalContext context)
        {
            _context = context;
        }

        // GET: Bookings
        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Booking
                .Include(b => b.Venue)
                .Include(b => b.Event)
                    .ThenInclude(e => e.EventType) // Include EventType if needed
                .OrderByDescending(b => b.StartDate) // Order by most recent first
                .ToListAsync();

            return View(bookings);
        }

        // GET: Bookings/Search
        // GET: Bookings/Search
        public IActionResult Search()
        {
            var model = new BookingsViewModel
            {
                Bookings = _context.Booking
                    .Include(b => b.Event)
                        .ThenInclude(e => e.EventType)
                    .Include(b => b.Venue)
                    .ToList(),
                SearchType = "bookings"
            };

            // Populate dropdown lists for better UX
            ViewBag.Venues = new SelectList(_context.Venue.OrderBy(v => v.VenueName), "Venue_ID", "VenueName");
            ViewBag.Events = new SelectList(_context.Event.OrderBy(e => e.EventName), "Event_ID", "EventName");
            ViewBag.EventTypes = new SelectList(_context.EventType.Where(et => et.IsActive).OrderBy(et => et.TypeName), "EventType_ID", "TypeName");

            return View(model);
        }

        // POST: Bookings/Search
        [HttpPost]
        public IActionResult Search(BookingsViewModel model)
        {
            if (model.SearchType == "availability")
            {
                return SearchAvailability(model);
            }
            else
            {
                return SearchBookings(model);
            }
        }

        private IActionResult SearchBookings(BookingsViewModel model)
        {
            var query = _context.Booking
                .Include(b => b.Event)
                    .ThenInclude(e => e.EventType)
                .Include(b => b.Venue)
                .AsQueryable();

            if (model.BookingDate.HasValue)
            {
                query = query.Where(b => model.BookingDate >= b.StartDate && model.BookingDate <= b.EndDate);
            }

            if (model.Venue_ID.HasValue)
            {
                query = query.Where(b => b.Venue_ID == model.Venue_ID);
            }

            if (model.Event_ID.HasValue)
            {
                query = query.Where(b => b.Event_ID == model.Event_ID);
            }

            if (model.EventType_ID.HasValue)
            {
                query = query.Where(b => b.Event.EventType_ID == model.EventType_ID);
            }

            model.Bookings = query.ToList();
            model.SearchType = "bookings";

            // Repopulate dropdown lists
            ViewBag.Venues = new SelectList(_context.Venue.OrderBy(v => v.VenueName), "Venue_ID", "VenueName");
            ViewBag.Events = new SelectList(_context.Event.OrderBy(e => e.EventName), "Event_ID", "EventName");
            ViewBag.EventTypes = new SelectList(_context.EventType.Where(et => et.IsActive).OrderBy(et => et.TypeName), "EventType_ID", "TypeName");

            return View(model);
        }

        private IActionResult SearchAvailability(BookingsViewModel model)
        {
            if (!model.StartDate.HasValue || !model.EndDate.HasValue)
            {
                ModelState.AddModelError(string.Empty, "Both start date and end date are required for availability search.");
                model.SearchType = "availability";

                // Repopulate dropdown lists
                ViewBag.Venues = new SelectList(_context.Venue.OrderBy(v => v.VenueName), "Venue_ID", "VenueName");
                ViewBag.Events = new SelectList(_context.Event.OrderBy(e => e.EventName), "Event_ID", "EventName");
                ViewBag.EventTypes = new SelectList(_context.EventType.Where(et => et.IsActive).OrderBy(et => et.TypeName), "EventType_ID", "TypeName");

                return View(model);
            }

            if (model.StartDate > model.EndDate)
            {
                ModelState.AddModelError(string.Empty, "Start date cannot be later than end date.");
                model.SearchType = "availability";

                // Repopulate dropdown lists
                ViewBag.Venues = new SelectList(_context.Venue.OrderBy(v => v.VenueName), "Venue_ID", "VenueName");
                ViewBag.Events = new SelectList(_context.Event.OrderBy(e => e.EventName), "Event_ID", "EventName");
                ViewBag.EventTypes = new SelectList(_context.EventType.Where(et => et.IsActive).OrderBy(et => et.TypeName), "EventType_ID", "TypeName");

                return View(model);
            }

            // Get all venues
            var venues = _context.Venue.OrderBy(v => v.VenueName).ToList();

            // Get all bookings in the date range
            var bookings = _context.Booking
                .Include(b => b.Event)
                .Include(b => b.Venue).Where(b =>
                          b.StartDate <= model.EndDate &&
                          b.EndDate >= model.StartDate)
                .ToList();

            // Calculate availability for each venue
            var availableVenues = new List<VenueAvailabilityInfo>();

            foreach (var venue in venues)
            {
                var venueBookings = bookings.Where(b => b.Venue_ID == venue.Venue_ID).ToList();
                var bookedDates = new List<DateOnly>();
                foreach (var booking in venueBookings)
                {
                    for (var date = booking.StartDate; date <= booking.EndDate; date = date.AddDays(1))
                    {
                        bookedDates.Add(date);
                    }
                }


                var availableDates = new List<DateOnly>();
                var currentDate = model.StartDate.Value;

                while (currentDate <= model.EndDate.Value)
                {
                    if (!bookedDates.Contains(currentDate))
                    {
                        availableDates.Add(currentDate);
                    }
                    currentDate = currentDate.AddDays(1);
                }

                var totalDays = (model.EndDate.Value.DayNumber - model.StartDate.Value.DayNumber) + 1;

                var availabilityInfo = new VenueAvailabilityInfo
                {
                    VenueId = venue.Venue_ID,
                    VenueName = venue.VenueName,
                    VenueLocation = venue.VenueLocation,
                    VenueCapacity = venue.VenueCapacity,
                    VenueImage = venue.VenueImage,
                    AvailableDates = availableDates,
                    BookedDates = bookedDates,
                    IsCompletelyAvailable = availableDates.Count == totalDays,
                    TotalDaysInRange = totalDays,
                    AvailableDaysCount = availableDates.Count
                };

                availableVenues.Add(availabilityInfo);
            }

            model.AvailableVenues = availableVenues;
            model.SearchType = "availability";

            // Repopulate dropdown lists
            ViewBag.Venues = new SelectList(_context.Venue.OrderBy(v => v.VenueName), "Venue_ID", "VenueName");
            ViewBag.Events = new SelectList(_context.Event.OrderBy(e => e.EventName), "Event_ID", "EventName");
            ViewBag.EventTypes = new SelectList(_context.EventType.Where(et => et.IsActive).OrderBy(et => et.TypeName), "EventType_ID", "TypeName");

            return View(model);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Booking_ID,BookingDate,Venue_ID,Event_ID")] Booking booking)
        {
            // Check for existing booking with same venue and date
            bool isDoubleBooked = await _context.Booking.AnyAsync(b =>
                 b.Venue_ID == booking.Venue_ID &&
                 b.StartDate <= booking.EndDate &&
                 b.EndDate >= booking.StartDate);


            if (isDoubleBooked)
            {
                ModelState.AddModelError(string.Empty, "This venue is already booked for the selected date.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Booking_ID,BookingDate,Venue_ID,Event_ID")] Booking booking)
        {
            if (id != booking.Booking_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Booking_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .FirstOrDefaultAsync(m => m.Booking_ID == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking != null)
            {
                _context.Booking.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.Booking_ID == id);
        }
    }
}