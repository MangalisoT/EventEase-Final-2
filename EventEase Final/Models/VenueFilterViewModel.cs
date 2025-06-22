using System.ComponentModel.DataAnnotations;

namespace EventEase_Final.Models
{
    public class VenueFilterViewModel
    {
        [Display(Name = "Search Name or Location")]
        public string? SearchString { get; set; }

        [Display(Name = "Event Type")]
        public string? EventType { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Show Available Only")]
        public bool ShowAvailableOnly { get; set; }

        [Display(Name = "Availability Status")]
        public bool? AvailabilityStatus { get; set; } // true = Available, false = Booked, null = All
    }

    public class VenueIndexViewModel
    {
        public List<Venue> Venues { get; set; } = new List<Venue>();
        public VenueFilterViewModel Filter { get; set; } = new VenueFilterViewModel();
    }
}