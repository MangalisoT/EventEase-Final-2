﻿using System;
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
    public class EventsController : Controller
    {
        private readonly EventEase_FinalContext _context;

        public EventsController(EventEase_FinalContext context)
        {
            _context = context;
        }

        // GET: Events
        // GET: Events
        public async Task<IActionResult> Index(string searchString)
        {
            if (_context.Event == null)
            {
                return Problem("Entity set 'EventEase_FinalContext.Event'  is null.");
            }

            var events = from e in _context.Event
                         select e;

            if (!String.IsNullOrEmpty(searchString))
            {
                events = events.Where(e => e.EventName.ToUpper().Contains(searchString.ToUpper()));
            }

            return View(await events.ToListAsync());
        }


        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .FirstOrDefaultAsync(m => m.Event_ID == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Event_ID,EventName,EventDate,EventDescription")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Event_ID,EventName,EventDate,EventDescription")] Event @event)
        {
            if (id != @event.Event_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Event_ID))
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
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .FirstOrDefaultAsync(m => m.Event_ID == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Check for any gameplay referencing this game
            bool hasBooked = await _context.Booking.AnyAsync(bk => bk.Event_ID == id);

            if (hasBooked)
            {
                var events = await _context.Event.FindAsync(id);
                ModelState.AddModelError("", "Cannot delete this event; there are existing event records.");

                return View(events);
            }
            var eventsToDelete = await _context.Event.FindAsync(id);
            _context.Event.Remove(eventsToDelete);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.Event_ID == id);
        }
    }
}
