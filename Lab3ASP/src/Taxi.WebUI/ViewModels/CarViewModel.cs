using System.ComponentModel.DataAnnotations;

namespace Taxi.WebUI.ViewModels
{
    public class CarViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Number length must be between 8 and 15")]
        [Display(Name = "Government number")]
        public string GovernmentNumber { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Model length must be between 2 and 30")]
        public string Model { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Color length must be between 3 and 20")]
        public string Color { get; set; }

        [Required]
        [Range(1900, 2100, ErrorMessage = "Year of issue must be between 1900 and 2100")]
        [Display(Name = "Year of issue")]
        public int YearOfIssue { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Registration number length must be between 6 and 20")]
        [Display(Name = "Registration number")]
        public string RegistrationNumber { get; set; }

        [Required]
        [Display(Name = "Is repair")]
        public bool IsRepair { get; set; }
    }
}