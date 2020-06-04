using AutoMapper;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Http;
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

        //public ActionResult Create()
        //{
        //    return View();
        //}

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

        //[HttpPut]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit(OrderViewModel orderViewModel)
        //{
        //    try
        //    {
        //        var order = _mapper.Map<Order>(orderViewModel);
        //        await _orderService.Update(order);
        //        _logger.LogInformation("order was updated");
        //        return RedirectToAction(nameof(Orders));
        //    }
        //    catch (Exception exception)
        //    {
        //        _logger.LogError($"Order update error:{exception.Message}");
        //        return View();
        //    }
        //}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Delete(OrderViewModel orderViewModel)
        //{
        //    try
        //    {
        //        await _orderService.Delete(orderViewModel.Id);
        //        _logger.LogInformation("order was deleted");
        //        return RedirectToAction(nameof(Orders));
        //    }
        //    catch (Exception exception)
        //    {
        //        _logger.LogError($"Event master error:{exception.Message}");
        //        return View();
        //    }
        //}

        //public async Task<ActionResult> ActiveOrders()
        //{
        //    var ordersList = _mapper.Map<IEnumerable<OrderViewModel>>(await _orderService.GetActiveOrders());
        //    return View(ordersList);
        //}

        //public async Task<ActionResult> InActiveOrders()
        //{
        //    var ordersList = _mapper.Map<IEnumerable<OrderViewModel>>(await _orderService.GetInActiveOrders());
        //    return View(ordersList);
        //}
    }
}