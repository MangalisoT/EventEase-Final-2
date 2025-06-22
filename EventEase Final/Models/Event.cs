using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEase_Final.Models
{
    public class Event
    {
        [Key]
        public int Event_ID { get; set; }

        [Required(ErrorMessage = "Event Name is required.")]
        [Display(Name = "Event Name")]
        [StringLength(100, ErrorMessage = "Event Name cannot exceed 100 characters.")]
        public string? EventName { get; set; }

        [Required(ErrorMessage = "Event Date is required.")]
        [Display(Name = "Event Date")]
        [DataType(DataType.Date)]
        public DateOnly? EventDate { get; set; }

        [Required(ErrorMessage = "Event Description is required.")]
        [Display(Name = "Event Description")]
        [StringLength(500, ErrorMessage = "Event Description cannot exceed 500 characters.")]
        public string? EventDescription { get; set; }

        [Required(ErrorMessage = "Event Type is required.")]
        [Display(Name = "Event Type")]
        public int EventType_ID { get; set; }

        // Navigation properties
        [ForeignKey("EventType_ID")]
        public EventType? EventType { get; set; }

        public List<Booking>? Bookings { get; set; }
    }
}