using BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eventus.BusinessLogic.Interfaces;
using Eventus.BusinessLogic.Validations;

namespace Eventus.WebAPI.Controllers
{
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
        [HttpPut]
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



        [HttpDelete]
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

    }
}