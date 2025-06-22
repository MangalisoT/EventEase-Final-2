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
                // Clear existing data in correct order (child tables first)
                if (context.Booking.Any())
                {
                    context.Booking.RemoveRange(context.Booking);
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Booking', RESEED, 0)");
                }

                if (context.Event.Any())
                {
                    context.Event.RemoveRange(context.Event);
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Event', RESEED, 0)");
                }

                if (context.Venue.Any())
                {
                    context.Venue.RemoveRange(context.Venue);
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Venue', RESEED, 0)");
                }

                if (context.EventType.Any())
                {
                    context.EventType.RemoveRange(context.EventType);
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('EventType', RESEED, 0)");
                }

                // 🌱 Seed EventTypes first
                var eventTypes = new[]
                {
                    new EventType
                    {
                        TypeName = "Music",
                        Description = "Musical events and concerts",
                        IsActive = true
                    },
                    new EventType
                    {
                        TypeName = "Cultural",
                        Description = "Cultural and arts events",
                        IsActive = true
                    },
                    new EventType
                    {
                        TypeName = "Sports",
                        Description = "Sports and fitness events",
                        IsActive = true
                    },
                    new EventType
                    {
                        TypeName = "Business",
                        Description = "Business and networking events",
                        IsActive = true
                    },
                    new EventType
                    {
                        TypeName = "Educational",
                        Description = "Educational workshops and seminars",
                        IsActive = true
                    }
                };
                context.EventType.AddRange(eventTypes);
                context.SaveChanges();

                // Get the EventTypes after they've been saved (they now have IDs)
                var musicEventType = context.EventType.First(et => et.TypeName == "Music");
                var culturalEventType = context.EventType.First(et => et.TypeName == "Cultural");

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
                        VenueImage = "https://kznweddingdj.co.za/wp-content/uploads/2020/08/kzn-wedding-dj-in-durban-music-for-weddings-in-kzn-001.jpg"
                    }
                };
                context.Venue.AddRange(venues);
                context.SaveChanges();

                // 🌱 Seed Events with valid EventType_ID references
                var events = new[]
                {
                    new Event
                    {
                        EventName = "House Revival",
                        EventDate = DateOnly.FromDateTime(DateTime.Parse("2025-06-15")),
                        EventDescription = "Bringing back old school house music with top DJs",
                        EventType_ID = musicEventType.EventType_ID
                    },
                    new Event
                    {
                        EventName = "Jazz Night",
                        EventDate = DateOnly.FromDateTime(DateTime.Parse("2025-07-20")),
                        EventDescription = "Live jazz music evening with cocktails and smooth vibes",
                        EventType_ID = musicEventType.EventType_ID
                    },
                    new Event
                    {
                        EventName = "Art Gallery Opening",
                        EventDate = DateOnly.FromDateTime(DateTime.Parse("2025-08-05")),
                        EventDescription = "Contemporary art exhibition featuring local artists",
                        EventType_ID = culturalEventType.EventType_ID
                    }
                };
                context.Event.AddRange(events);
                context.SaveChanges();

                // Get the venues and events after they've been saved (they now have IDs)
                var savedVenues = context.Venue.ToList();
                var savedEvents = context.Event.ToList();

                // 🌱 Seed Bookings
                var bookings = new[]
{
    new Booking
    {
        StartDate = DateOnly.FromDateTime(DateTime.Today),
        EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(2)), // 3-day booking
        Venue_ID = savedVenues[0].Venue_ID,
        Event_ID = savedEvents[0].Event_ID
    },
    new Booking
    {
        StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
        EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)), // single-day booking
        Venue_ID = savedVenues[1].Venue_ID,
        Event_ID = savedEvents[1].Event_ID
    }
};

                context.Booking.AddRange(bookings);
                context.SaveChanges();

                Console.WriteLine("Database seeded successfully!");
                Console.WriteLine($"- Created {eventTypes.Length} Event Types");
                Console.WriteLine($"- Created {venues.Length} Venues");
                Console.WriteLine($"- Created {events.Length} Events");
                Console.WriteLine($"- Created {bookings.Length} Bookings");
            }
        }
    }
}