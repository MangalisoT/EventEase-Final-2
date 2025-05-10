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
        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Booking
                .Include(b => b.Venue)
                .Include(b => b.Event)
                .ToListAsync();

            return View(bookings);
        }

        // GET: Bookings/Search
        public IActionResult Search()
        {
            var model = new BookingsViewModel
            {
                Bookings = _context.Booking
                    .Include(b => b.Event)
                    .Include(b => b.Venue)
                    .ToList()
            };
            return View(model);
        }

        // POST: Bookings/Search
        [HttpPost]
        public IActionResult Search(BookingsViewModel model)
        {
            var query = _context.Booking
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .AsQueryable();

            if (model.BookingDate.HasValue)
            {
                query = query.Where(b => b.BookingDate == model.BookingDate);
            }

            if (model.Venue_ID.HasValue)
            {
                query = query.Where(b => b.Venue_ID == model.Venue_ID);
            }

            if (model.Event_ID.HasValue)
            {
                query = query.Where(b => b.Event_ID == model.Event_ID);
            }

            model.Bookings = query.ToList();

            return View(model);
        }



        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
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
