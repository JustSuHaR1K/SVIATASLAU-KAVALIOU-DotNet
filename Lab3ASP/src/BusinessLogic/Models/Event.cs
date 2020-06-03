using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Event Name length must be between 2 and 30")]
        public string NameOfEvent { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Number length must be between 8 and 15")]
        public string GovernmentNumberOfService { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Description length must be between 3 and 25")]
        public string Description { get; set; }

        [Required]
        [Range(1, 24, ErrorMessage = "Event duration must be between 1 and 24")]
        public int EventDuration { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "Price of the event must be between 1 and 24")]
        public int PriceOfTheEvent { get; set; }

        [Required]
        public bool IsRework { get; set; }
    }
}