using System.ComponentModel.DataAnnotations;

namespace EventEase_Final.Models
{
    public class VenueViewModel
    {
        public int Venue_ID { get; set; }

        [Required]
        [Display(Name = "Venue Name")]
        public string? VenueName { get; set; }

        [Required]
        [Display(Name = "Venue Location")]
        public string? VenueLocation { get; set; }

        [Required]
        [Display(Name = "Venue Capacity")]
        public int? VenueCapacity { get; set; }

        [Required]
        [Display(Name = "Venue Image")]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "Venue Image")]
        public string? ImageUrl { get; set; } = null!;

    }
}
