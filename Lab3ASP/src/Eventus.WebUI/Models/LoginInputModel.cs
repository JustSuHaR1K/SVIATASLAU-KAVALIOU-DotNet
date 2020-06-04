using System.ComponentModel.DataAnnotations;

namespace Eventus.WebUI.Models
{
    public class LoginInputModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}