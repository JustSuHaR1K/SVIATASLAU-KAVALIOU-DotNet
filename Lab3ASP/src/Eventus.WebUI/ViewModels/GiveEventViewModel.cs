using System.ComponentModel.DataAnnotations;

namespace Eventus.WebUI.ViewModels
{
    public class GiveEventViewModel
    {
        [Required]
        [Display(Name = "Master license number")]
        public string MasterLicenseNumber { get; set; }

        [Required]
        [Display(Name = "Event code number")]
        public string EventCodeNumber { get; set; }
    }
}