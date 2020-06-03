using AutoMapper;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Eventus.BusinessLogic.Interfaces;
using Eventus.WebUI.Models;
using Eventus.WebUI.ViewModels;

namespace Eventus.WebUI.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventService _carService;

        private readonly ILogger<EventsController> _logger;

        private readonly IMapper _mapper;

        public EventsController(IEventService carService, ILogger<EventsController> logger, IMapper mapper)
        {
            _carService = carService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ActionResult> Cars()
        {
            var carList = _mapper.Map<IEnumerable<EventViewModel>>(await _carService.GetAll());
            return View(carList);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EventViewModel carViewModel)
        {
            try
            {
                var car = _mapper.Map<Event>(carViewModel);
                if (await _carService.UniquenessCheck(car))
                {
                    await _carService.Add(car);
                }
                return RedirectToAction(nameof(Cars));
            }
            catch (Exception exception)
            {
                _logger.LogError($"Car create error:{exception.Message}");
                return View(carViewModel);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var car = await _carService.FindById(id);
                var carViewModel = _mapper.Map<EventViewModel>(car);
                return View(carViewModel);
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception.Message);
                return RedirectToAction(nameof(Cars));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EventViewModel carViewModel)
        {
            try
            {
                var car = _mapper.Map<Event>(carViewModel);
                await _carService.Update(car);
                return RedirectToAction(nameof(Cars));
            }
            catch (Exception exception)
            {
                _logger.LogError($"Car update error:{exception.Message}");
                return View();
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var car = await _carService.FindById(id);
                var carViewModel = _mapper.Map<EventViewModel>(car);
                return View(carViewModel);
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception.Message);
                return RedirectToAction(nameof(Cars));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(EventViewModel carViewModel)
        {
            try
            {
                await _carService.Delete(carViewModel.Id);
                return RedirectToAction(nameof(Cars));
            }
            catch (Exception exception)
            {
                _logger.LogError($"Car delete error:{exception.Message}");
                return View();
            }
        }

        public async Task<ActionResult> GetEventOnRework()
        {
            var carsList = _mapper.Map<IEnumerable<EventViewModel>>(await _carService.GetEventOnRework());
            return View(carsList);
        }

        public async Task<ActionResult> NewCars(int age)
        {
            var carsList = _mapper.Map<IEnumerable<EventViewModel>>(await _carService.GetOldEvents(age));
            return View(carsList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}