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
        private readonly IEventService _eventService;

        private readonly ILogger<EventsController> _logger;

        private readonly IMapper _mapper;

        public EventsController(IEventService eventService, ILogger<EventsController> logger, IMapper mapper)
        {
            _eventService = eventService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ActionResult> Events()
        {
            var eventsList = _mapper.Map<IEnumerable<EventViewModel>>(await _eventService.GetAll());
            return View(eventsList);
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
                var eventus = _mapper.Map<Event>(carViewModel);
                if (await _eventService.UniquenessCheck(eventus))
                {
                    await _eventService.Add(eventus);
                }
                return RedirectToAction(nameof(Events));
            }
            catch (Exception exception)
            {
                _logger.LogError($"Event create error:{exception.Message}");
                return View(carViewModel);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var eventus = await _eventService.FindById(id);
                var eventViewModel = _mapper.Map<EventViewModel>(eventus);
                return View(eventViewModel);
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception.Message);
                return RedirectToAction(nameof(Events));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EventViewModel eventViewModel)
        {
            try
            {
                var eventus = _mapper.Map<Event>(eventViewModel);
                await _eventService.Update(eventus);
                return RedirectToAction(nameof(Events));
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
                var eventus = await _eventService.FindById(id);
                var eventViewModel = _mapper.Map<EventViewModel>(eventus);
                return View(eventViewModel);
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception.Message);
                return RedirectToAction(nameof(Events));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(EventViewModel carViewModel)
        {
            try
            {
                await _eventService.Delete(carViewModel.Id);
                return RedirectToAction(nameof(Events));
            }
            catch (Exception exception)
            {
                _logger.LogError($"Car delete error:{exception.Message}");
                return View();
            }
        }

        public async Task<ActionResult> GetEventOnRework()
        {
            var eventsList = _mapper.Map<IEnumerable<EventViewModel>>(await _eventService.GetEventOnRework());
            return View(eventsList);
        }

        public async Task<ActionResult> GetLongEvents(int duration)
        {
            var eventsList = _mapper.Map<IEnumerable<EventViewModel>>(await _eventService.GetLongEvents(duration));
            return View(eventsList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}