using System.ComponentModel.DataAnnotations;

namespace Eventus.WebUI.ViewModels
{
    public class GiveEventViewModel
    {
        [Required]
        [Display(Name = "Driver license number")]
        public string DriverLicenseNumber { get; set; }

        [Required]
        [Display(Name = "Car government number")]
        public string CarGovernmentNumber { get; set; }
    }
}