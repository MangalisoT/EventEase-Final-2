using Microsoft.EntityFrameworkCore;
using EventEase_Final.Models;

namespace EventEase_Final.Data
{
    public class EventEase_FinalContext : DbContext
    {
        public EventEase_FinalContext(DbContextOptions<EventEase_FinalContext> options)
            : base(options)
        {
        }

        public DbSet<Booking> Booking { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<EventType> EventType { get; set; }
        public DbSet<Venue> Venue { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Booking relationships explicitly
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Venue)
                .WithMany(v => v.Bookings)
                .HasForeignKey(b => b.Venue_ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Event)
                .WithMany(e => e.Bookings)
                .HasForeignKey(b => b.Event_ID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Event-EventType relationship
            modelBuilder.Entity<Event>()
                .HasOne(e => e.EventType)
                .WithMany(et => et.Events)
                .HasForeignKey(e => e.EventType_ID)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}