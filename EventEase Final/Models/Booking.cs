using System.ComponentModel.DataAnnotations;

namespace EventEase_Final.Models
{
    public class Booking
    {
        [Key]
        public int Booking_ID { get; set; }
        [Display(Name = "Booking Date")]
        [DataType(DataType.Date)]

        public DateOnly? BookingDate { get; set; }
        [Display(Name = "Venue")]

        public int? Venue_ID { get; set; }
        [Display(Name = "Event")]
        public int? Event_ID { get; set; }

        public Venue? Venue { get; set; }
        public Event? Event { get; set; }
    }
}
