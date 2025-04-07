using System.ComponentModel.DataAnnotations;

namespace EventEase_Final.Models
{
    public class Venue
    {
        [Key]
        public int Venue_ID { get; set; }

        [Display(Name = "Venue Name")]
        public string? VenueName { get; set; }

        [Display(Name = "Venue Location")]
        public string? VenueLocation { get; set; }

        [Display(Name = "Venue Capacity")]
        public int? VenueCapacity { get; set; }

        [Display(Name = "Venue Image")]
        public string? VenueImage { get; set; }

        public List<Booking>? Bookings { get; set; }
    }
}
