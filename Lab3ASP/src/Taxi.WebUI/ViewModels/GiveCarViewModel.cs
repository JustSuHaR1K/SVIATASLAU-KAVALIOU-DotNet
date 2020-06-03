using System.ComponentModel.DataAnnotations;

namespace Taxi.WebUI.ViewModels
{
    public class GiveCarViewModel
    {
        [Required]
        [Display(Name = "Driver license number")]
        public string DriverLicenseNumber { get; set; }

        [Required]
        [Display(Name = "Car government number")]
        public string CarGovernmentNumber { get; set; }
    }
}