using Eventus.BusinessLogic;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eventus.BusinessLogic.Interfaces;
using Eventus.WebUI.Controllers;
using AutoMapper;
using Eventus.WebUI.ViewModels;
using System;
using Eventus.WebUI.Models;
using System.Diagnostics;
using Eventus.BusinessLogic.Validations;

namespace Eventus.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        private readonly IEventService _eventService;

        private readonly ILogger<EventsController> _logger;


        public EventsController(IEventService eventService, ILogger<EventsController> logger)
        {
            _eventService = eventService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> Events()
        {
            var result = await _eventService.GetAll();
            _logger.LogInformation("Fetched events");

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FindByIdAsync(int id)
        {
            try
            {
                var @event = await _eventService.FindById(id);
                _logger.LogInformation("Searched for Event");

                return Ok(@event);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Event @event)
        {
            try
            {
                await _eventService.Add(@event);
                _logger.LogInformation("Added new event");

                return Ok();
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _eventService.Delete(id);
                _logger.LogInformation("Deleted event");

                return Ok();
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Event @event)
        {
            try
            {
                await _eventService.Update(@event);
                _logger.LogInformation("Added new class");

                return Ok();
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
       
      
      
    }
}
