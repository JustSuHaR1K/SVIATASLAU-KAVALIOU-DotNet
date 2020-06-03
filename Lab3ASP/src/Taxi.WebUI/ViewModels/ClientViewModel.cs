using System.ComponentModel.DataAnnotations;

namespace Taxi.WebUI.ViewModels
{
    public class ClientViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Patronymic { get; set; }
    }
}