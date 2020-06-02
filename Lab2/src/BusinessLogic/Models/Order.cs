using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        [Range(typeof(DateTime), "1/1/1950", "1/1/2100", ErrorMessage = "Date is out of Range")]
        public DateTime Date { get; set; }

        [Required]
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

        public int? MasterId { get; set; }

        [Required]
        public int ClientId { get; set; }
    }
}