using System.ComponentModel.DataAnnotations;

namespace EventEase_Final.Models
{
    public class EventType
    {
        [Key]
        public int EventType_ID { get; set; }

        [Required(ErrorMessage = "Event Type Name is required.")]
        [Display(Name = "Event Type")]
        [StringLength(50, ErrorMessage = "Event Type Name cannot exceed 50 characters.")]
        public string TypeName { get; set; } = string.Empty;

        [Display(Name = "Description")]
        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
        public string? Description { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        // Navigation property
        public List<Event>? Events { get; set; }
    }
}