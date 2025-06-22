using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEase_Final.Models
{
    public class Booking
    {
        [Key]
        public int Booking_ID { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateOnly EndDate { get; set; }

        [Required(ErrorMessage = "Venue ID is required.")]
        [Display(Name = "Venue")]
        public int Venue_ID { get; set; }

        [Required(ErrorMessage = "Event ID is required.")]
        [Display(Name = "Event")]
        public int Event_ID { get; set; }

        // Navigation properties
        [ForeignKey("Venue_ID")]
        public virtual Venue? Venue { get; set; }

        [ForeignKey("Event_ID")]
        public virtual Event? Event { get; set; }
    }
}