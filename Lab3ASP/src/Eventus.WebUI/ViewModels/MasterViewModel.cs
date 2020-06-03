using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eventus.WebUI.ViewModels
{
    public class MasterViewModel
    {
        public int Id { get; set; }

        public int? EventId { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Profession name length must be between 2 and 30")]
        [Display(Name = "Profession")]
        public string Profession { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Surname length must be between 2 and 30")]
        public string Surname { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Name length must be between 2 and 30")]
        public string Name { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Patronymic length must be between 2 and 30")]
        public string Patronymic { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Master license number must be between 5 and 30")]
        [Display(Name = "Master license number")]
        public string MasterLicenseNumber { get; set; }

        [Required]
        [Display(Name = "Date of issue of an entrepreneurial license")]
        public DateTime DateOfIssueOfAnEntrepreneurialLicense { get; set; }

        [Required]
        [Display(Name = "Is sick leave")]
        public bool IsSickLeave { get; set; }

        [Required]
        [Display(Name = "Is on holiday")]
        public bool IsOnHoliday { get; set; }

        public EventViewModel Event { get; set; }
    }
}
