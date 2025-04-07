using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using EventEase_Final.Models;

namespace EventEase_Final.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new EventEase_FinalContext(
                serviceProvider.GetRequiredService<DbContextOptions<EventEase_FinalContext>>()))
            {
                // 🧹 Delete existing data (in correct order due to foreign keys)
                context.Booking.RemoveRange(context.Booking);
                context.Event.RemoveRange(context.Event);
                context.Venue.RemoveRange(context.Venue);
                context.SaveChanges();

                // 🔁 Reset identity columns (only works for SQL Server)
                context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Booking', RESEED, 0)");
                context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Event', RESEED, 0)");
                context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Venue', RESEED, 0)");

                // 🌱 Seed Venues
                var venues = new[]
                {
                    new Venue
                    {
                        VenueName = "Grand Ballroom",
                        VenueLocation = "Durban City Center",
                        VenueCapacity = 300,
                        VenueImage = "https://www.radia.io/blog/house.jpg"
                    },
                    new Venue
                    {
                        VenueName = "Seaside Pavillion",
                        VenueLocation = "Cliffton Beach, CapeTown",
                        VenueCapacity = 150,
                        VenueImage = "https://visitdurban.travel/wp-content/uploads/2024/12/BEAN-BAG-149-H.jpg"
                    }
                };
                context.Venue.AddRange(venues);
                context.SaveChanges();

                // 🌱 Seed Events
                var events = new[]
                {
                    new Event
                    {
                        EventName = "House Revival",
                        EventDate = DateOnly.FromDateTime(DateTime.Parse("2025-06-15")),
                        EventDescription = "Bringing back old school"
                    },
                    new Event
                    {
                        EventName = "Jazz Night",
                        EventDate = DateOnly.FromDateTime(DateTime.Parse("2025-07-20")),
                        EventDescription = "Live jazz music evening with cocktails."
                    }
                };
                context.Event.AddRange(events);
                context.SaveChanges();

                // 🌱 Seed Bookings
                var bookings = new[]
                {
                    new Booking
                    {
                        BookingDate = DateOnly.FromDateTime(DateTime.Today),
                        Venue_ID = venues[0].Venue_ID,
                        Event_ID = events[0].Event_ID
                    },
                    new Booking
                    {
                        BookingDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                        Venue_ID = venues[1].Venue_ID,
                        Event_ID = events[1].Event_ID
                    }
                };
                context.Booking.AddRange(bookings);
                context.SaveChanges();
            }
        }
    }
}
