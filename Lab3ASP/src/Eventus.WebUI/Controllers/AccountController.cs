using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using Eventus.BusinessLogic.Interfaces;
using Eventus.WebUI.Identitefication;
using Eventus.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventus.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IIdentityService identityService, ILogger<AccountController> logger)
        {
            _identityService = identityService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginInputModel();

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _identityService.SignInAsync(model.Email, model.Password);
                    _logger.LogInformation("User signed in");

                    return RedirectToAction("Events", "Events");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occured during login. Exception: {ex.Message}");
                    return View();
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterInputModel();

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        UserName = $"{model.FirstName}{model.LastName}",
                    };

                    await _identityService.RegisterAsync(user, model.Password);
                    await _identityService.AddToRoleAsync(user, Roles.User);
                    _logger.LogInformation("Created new user");

                    return RedirectToAction("Events", "Events");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occured during register. Exception: {ex.Message}");
                    return View();
                }
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await _identityService.SignOutAsync();

            return RedirectToAction("Events", "Events");
        }
    }
}
