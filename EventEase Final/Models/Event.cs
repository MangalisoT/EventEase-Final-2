using System.ComponentModel.DataAnnotations;

namespace EventEase_Final.Models
{
    public class Event
    {
        [Key]
        public int Event_ID { get; set; }

        [Required(ErrorMessage = "Event Name is required.")]
        [Display(Name = "Event Name")]
        public string? EventName { get; set; }

        [Required(ErrorMessage = "Event Date is required.")]
        [Display(Name = "Event Date")]
        [DataType(DataType.Date)]
        public DateOnly? EventDate { get; set; }

        [Required(ErrorMessage = "Event Description is required.")]
        [Display(Name = "Event Description")]
        public string? EventDescription { get; set; }

        public List<Booking>? Bookings { get; set; }
    }
}
