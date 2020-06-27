using BusinessLogic.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventus.BusinessLogic.Interfaces
{
    public interface IIdentityService
    {
        Task<User> GetByIdAsync(string id);

        Task<List<User>> GetUsersAsync();

        Task UpdateAsync(User user);

        Task DeleteAsync(string id);

        Task RegisterAsync(User user, string password);

        Task SignOutAsync();

        Task SignInAsync(string email, string password);

        Task<User> FindByEmailAsync(string email);

        Task CreateRoleAsync(IdentityRole role);

        Task<bool> RoleExistsAsync(string roleName);

        Task AddToRoleAsync(User user, string roleName);
    }
}
