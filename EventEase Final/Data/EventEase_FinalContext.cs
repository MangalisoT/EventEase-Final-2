using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EventEase_Final.Models;

namespace EventEase_Final.Data
{
    public class EventEase_FinalContext : DbContext
    {
        public EventEase_FinalContext (DbContextOptions<EventEase_FinalContext> options)
            : base(options)
        {
        }

        public DbSet<EventEase_Final.Models.Venue> Venue { get; set; } = default!;
        public DbSet<EventEase_Final.Models.Event> Event { get; set; } = default!;
        public DbSet<EventEase_Final.Models.Booking> Booking { get; set; } = default!;
    }
}
