using System.ComponentModel.DataAnnotations;

namespace EventEase_Final.Models
{
    public class Booking
    {
        [Key]
        public int Booking_ID { get; set; }

        [Required(ErrorMessage = "Booking Date is required.")]
        [Display(Name = "Booking Date")]
        [DataType(DataType.Date)]
        public DateOnly? BookingDate { get; set; }

        [Required(ErrorMessage = "Venue ID is required.")]
        [Display(Name = "Venue")]
        public int? Venue_ID { get; set; }

        [Required(ErrorMessage = "Event ID is required.")]
        [Display(Name = "Event")]
        public int? Event_ID { get; set; }

        public Venue? Venue { get; set; }
        public Event? Event { get; set; }
    }
}
