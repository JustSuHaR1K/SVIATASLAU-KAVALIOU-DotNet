using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using Eventus.BusinessLogic.Interfaces;
using Eventus.WebUI.Identitefication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventus.WebUI.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class UsersController : Controller
    {
        private readonly IIdentityService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IIdentityService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var users = await _userService.GetUsersAsync();
            _logger.LogInformation("Fetched user list");

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var user = await _userService.GetByIdAsync(id);

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Update(User user)
        {
            try
            {
                await _userService.UpdateAsync(user);
                _logger.LogInformation("Updated user");

                return RedirectToAction(nameof(List));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Error occured during updating user. Exception: {ex.Message}");
                return View(user);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _userService.DeleteAsync(id);
                _logger.LogInformation("Deleted user");

                return RedirectToAction(nameof(List));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Error occured during updating user. Exception: {ex.Message}");
                return View();
            }
        }
    }
}
