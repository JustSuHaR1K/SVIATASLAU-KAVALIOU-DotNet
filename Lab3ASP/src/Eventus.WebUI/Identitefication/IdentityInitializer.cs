using BusinessLogic.Models;
using Eventus.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Eventus.WebUI.Identitefication
{
    public class IdentityInitializer
    {
        
            private readonly IIdentityService _identityService;

            public IdentityInitializer(IIdentityService identityService)
            {
                _identityService = identityService;
            }

            public async Task Seed()
            {
                var password = "123456Aa+";

                var users = new User[]
                {
                new User
                {
                    UserName = "ahah2418",
                    FirstName = "Netu",
                    LastName = "TozheNet",
                    Email = "vasdas@gmail.ru",
                },

                new User
                {
                    UserName = "Kuchka15",
                    FirstName = "SIMON",
                    LastName = "Nekrasivii",
                    Email = "adminesas@gmail.by",
                },
                };

                var roles = new string[]
                {
                Roles.User,
                Roles.Admin,
                };

                foreach (var role in roles)
                {
                    if (!await _identityService.RoleExistsAsync(role))
                    {
                        await _identityService.CreateRoleAsync(new IdentityRole(role));
                    }
                }

                var i = 0;
                foreach (var user in users)
                {
                    if (await _identityService.FindByEmailAsync(user.Email) == null)
                    {
                        await _identityService.RegisterAsync(user, password);
                        await _identityService.AddToRoleAsync(user, roles[i++]);
                    }
                }
            }
        
    }
}
