using System.ComponentModel.DataAnnotations;

namespace EventEase_Final.Models
{
    public class Venue
    {
        [Key]
        public int Venue_ID { get; set; }

        [Required(ErrorMessage = "Venue Name is required.")]
        [Display(Name = "Venue Name")]
        public string? VenueName { get; set; }

        [Required(ErrorMessage = "Venue Location is required.")]
        [Display(Name = "Venue Location")]
        public string? VenueLocation { get; set; }

        [Required(ErrorMessage = "Venue Capacity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Venue capacity must be greater than 0.")]
        [Display(Name = "Venue Capacity")]
        public int? VenueCapacity { get; set; }

        [Display(Name = "Venue Image")]
        public string? VenueImage { get; set; }

        public List<Booking>? Bookings { get; set; }
    }
}
