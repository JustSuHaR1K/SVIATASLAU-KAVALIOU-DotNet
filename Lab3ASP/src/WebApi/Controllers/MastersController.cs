using AutoMapper;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eventus.BusinessLogic.Interfaces;
using Eventus.WebUI.ViewModels;
using Eventus.BusinessLogic.Validations;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MastersController : Controller
    {

        private readonly IMasterService _masterService;

        private readonly ILogger<MastersController> _logger;


        public MastersController(IMasterService masterService, ILogger<MastersController> logger)
        {
            _masterService = masterService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Master>>> Masters()
        {
            var result = await _masterService.GetAll();
            _logger.LogInformation("Fetched masters");

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FindByIdAsync(int id)
        {
            try
            {
                var user = await _masterService.FindById(id);
                _logger.LogInformation("Searched for Master");

                return Ok(user);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Master master)
        {
            try
            {
                await _masterService.Add(master);
                _logger.LogInformation("Added new master");

                return Ok();
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Update([FromBody] Master master)
        {
            try
            {
                await _masterService.Update(master);
                _logger.LogInformation("Updated master");

                return Ok();
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit(MasterViewModel masterViewModel)
        //{
        //    try
        //    {
        //        var master = _mapper.Map<Master>(masterViewModel);
        //        await _masterService.Update(master);
        //        _logger.LogInformation("master was updated");
        //        return RedirectToAction(nameof(Masters));
        //    }
        //    catch (Exception exception)
        //    {
        //        _logger.LogError($"Master update error:{exception.Message}");
        //        return View();
        //    }
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _masterService.Delete(id);
                _logger.LogInformation("Deleted master");

                return Ok();
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Delete(MasterViewModel masterViewModel)
        //{
        //    try
        //    {
        //        await _masterService.Delete(masterViewModel.Id);
        //        _logger.LogInformation("master was deleted");
        //        return RedirectToAction(nameof(Masters));
        //    }
        //    catch (Exception exception)
        //    {
        //        _logger.LogError($"Event master error:{exception.Message}");
        //        return View();
        //    }
        //}

        //public ActionResult GiveEvent()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> GiveEvent(GiveEventViewModel giveEventViewModel)
        //{
        //    try
        //    {
        //        var eventus = await _eventService.FindByGovernmentNumberOfService(giveEventViewModel.EventCodeNumber);
        //        var master = await _masterService.FindByMasterLicenseNumber(giveEventViewModel.MasterLicenseNumber);
        //        _logger.LogInformation("Master and their event was shown");
        //        await _masterService.GiveEvent(master.Id, eventus.Id);
        //        return RedirectToAction(nameof(Masters));
        //    }
        //    catch (Exception exception)
        //    {
        //        _logger.LogError($"Give event error:{exception.Message}");
        //        return View();
        //    }
        //}
    }
}