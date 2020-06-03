using AutoMapper;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eventus.BusinessLogic.Interfaces;
using Eventus.WebUI.ViewModels;

namespace Eventus.WebUI.Controllers
{
    public class MastersController : Controller
    {
        private readonly IEventService _eventService;

        private readonly IMasterService _masterService;

        private readonly ILogger<EventsController> _logger;

        private readonly IMapper _mapper;

        public MastersController(IEventService eventService, IMasterService masterService, ILogger<EventsController> logger, IMapper mapper)
        {
            _eventService = eventService;
            _masterService = masterService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ActionResult> Drivers()
        {
            var driverList = _mapper.Map<IEnumerable<MasterViewModel>>(await _masterService.GetAll());

            foreach (var item in driverList)
            {
                try
                {
                    item.Car = _mapper.Map<EventViewModel>(await _eventService.FindById((int)item.CarId));
                }
                catch (InvalidOperationException exception)
                {
                    _logger.LogError($"Find car error:{exception.Message}");
                }
            }

            return View(driverList);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MasterViewModel driverViewModel)
        {
            try
            {
                var driver = _mapper.Map<Master>(driverViewModel);
                if (await _masterService.UniquenessCheck(driver))
                {
                    await _masterService.Add(driver);
                }
                return RedirectToAction(nameof(Drivers));
            }
            catch (Exception exception)
            {
                _logger.LogError($"Driver create error:{exception.Message}");
                return View(driverViewModel);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var driver = await _masterService.FindById(id);
                var driverViewModel = _mapper.Map<MasterViewModel>(driver);
                return View(driverViewModel);
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception.Message);
                return RedirectToAction(nameof(Drivers));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MasterViewModel driverViewModel)
        {
            try
            {
                var driver = _mapper.Map<Master>(driverViewModel);
                await _masterService.Update(driver);
                return RedirectToAction(nameof(Drivers));
            }
            catch (Exception exception)
            {
                _logger.LogError($"Driver update error:{exception.Message}");
                return View();
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var driver = await _masterService.FindById(id);
                var driverViewModel = _mapper.Map<MasterViewModel>(driver);
                return View(driverViewModel);
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception.Message);
                return RedirectToAction(nameof(Drivers));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(MasterViewModel driverViewModel)
        {
            try
            {
                await _masterService.Delete(driverViewModel.Id);
                return RedirectToAction(nameof(Drivers));
            }
            catch (Exception exception)
            {
                _logger.LogError($"Car driver error:{exception.Message}");
                return View();
            }
        }

        public ActionResult GiveCar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GiveCar(GiveEventViewModel giveCarViewModel)
        {
            try
            {
                var car = await _eventService.FindByGovernmentNumber(giveCarViewModel.CarGovernmentNumber);
                var driver = await _masterService.FindByDriverLicenseNumber(giveCarViewModel.DriverLicenseNumber);
                await _masterService.GiveCar(driver.Id, car.Id);
                return RedirectToAction(nameof(Drivers));
            }
            catch (Exception exception)
            {
                _logger.LogError($"Give car error:{exception.Message}");
                return View();
            }
        }
    }
}