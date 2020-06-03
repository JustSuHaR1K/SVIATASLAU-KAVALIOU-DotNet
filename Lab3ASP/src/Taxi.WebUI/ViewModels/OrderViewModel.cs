using System;
using System.ComponentModel.DataAnnotations;

namespace Taxi.WebUI.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Is done")]
        public bool IsDone { get; set; }

        [Required]
        [Range(1.0, double.MaxValue, ErrorMessage = "Minimum cost = 1.0")]
        public double Cost { get; set; }

        [Required]
        [Range(1.0, double.MaxValue, ErrorMessage = "Minimum distance = 1.0")]
        public double Distance { get; set; }

        [Required]
        [Range(0, double.MaxValue / 2, ErrorMessage = "Minimum discount = 0.0")]
        public double Discount { get; set; }

        public int? DriverId { get; set; }

        public int ClientId { get; set; }

        public DriverViewModel Driver { get; set; }

        public ClientViewModel Client { get; set; }
    }
}