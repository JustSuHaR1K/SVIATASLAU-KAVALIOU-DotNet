using Microsoft.AspNetCore.Identity;

namespace Eventus.DAL.Models
{
    public class UserDto : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
