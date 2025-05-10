
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventEase_Final.Models
{
    public class BookingsViewModel
    {
        [Display(Name = "Booking Date")]
        [DataType(DataType.Date)]
        public DateOnly? BookingDate { get; set; }

        [Display(Name = "Venue")]
        public int? Venue_ID { get; set; }

        [Display(Name = "Event")]
        public int? Event_ID { get; set; }

        public List<Booking>? Bookings { get; set; }
    }
}
