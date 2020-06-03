using System.ComponentModel.DataAnnotations;

namespace Eventus.WebUI.ViewModels
{
    public class GiveEventViewModel
    {
        [Required]
        [Display(Name = "Master license number")]
        public string MasterLicenseNumber { get; set; }

        [Required]
        [Display(Name = "Event government number")]
        public string EventGovernmentNumber { get; set; }
    }
}