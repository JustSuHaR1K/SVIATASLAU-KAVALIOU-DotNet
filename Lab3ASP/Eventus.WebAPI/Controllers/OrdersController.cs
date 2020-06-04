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
    public class OrdersController : Controller
    {

        private readonly IOrderService _orderService;

        private readonly ILogger<OrdersController> _logger;


        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> Orders()
        {
            var result = await _orderService.GetAll();
            _logger.LogInformation("Fetched orders");

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FindByIdAsync(int id)
        {
            try
            {
                var user = await _orderService.FindById(id);
                _logger.LogInformation("Searched for Order");

                return Ok(user);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([FromBody] Order order)
        {
            try
            {
                await _orderService.Add(order);
                _logger.LogInformation("Added new order");

                return Ok();
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Order order)
        {
            try
            {
                await _orderService.Update(order);
                _logger.LogInformation("Updated order");

                return Ok();
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _orderService.Delete(id);
                _logger.LogInformation("Deleted order");

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