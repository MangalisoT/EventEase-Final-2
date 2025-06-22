using System.ComponentModel.DataAnnotations;

namespace EventEase_Final.Models
{
    public class BookingsViewModel
    {
        [Display(Name = "Booking Date")]
        [DataType(DataType.Date)]
        public DateOnly? BookingDate { get; set; }

        [Display(Name = "Venue ID")]
        public int? Venue_ID { get; set; }

        [Display(Name = "Event ID")]
        public int? Event_ID { get; set; }

        [Display(Name = "Event Type")]
        public int? EventType_ID { get; set; }

        // New properties for availability search
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateOnly? StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateOnly? EndDate { get; set; }

        [Display(Name = "Search Type")]
        public string SearchType { get; set; } = "bookings"; // "bookings" or "availability"

        public List<Booking>? Bookings { get; set; }
        public List<VenueAvailabilityInfo>? AvailableVenues { get; set; }
    }

    public class VenueAvailabilityInfo
    {
        public int VenueId { get; set; }
        public string VenueName { get; set; }
        public string VenueLocation { get; set; }
        public int? VenueCapacity { get; set; }
        public string VenueImage { get; set; }
        public List<DateOnly> AvailableDates { get; set; } = new List<DateOnly>();
        public List<DateOnly> BookedDates { get; set; } = new List<DateOnly>();
        public bool IsCompletelyAvailable { get; set; }
        public int TotalDaysInRange { get; set; }
        public int AvailableDaysCount { get; set; }
    }
}