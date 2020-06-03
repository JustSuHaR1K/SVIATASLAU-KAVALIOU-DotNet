using AutoMapper;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Taxi.BusinessLogic.Interfaces;
using Taxi.WebUI.ViewModels;

namespace Taxi.WebUI.Controllers
{
    public class DriversController : Controller
    {
        private readonly ICarService _carService;

        private readonly IDriverService _driverService;

        private readonly ILogger<CarsController> _logger;

        private readonly IMapper _mapper;

        public DriversController(ICarService carService, IDriverService driverService, ILogger<CarsController> logger, IMapper mapper)
        {
            _carService = carService;
            _driverService = driverService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ActionResult> Drivers()
        {
            var driverList = _mapper.Map<IEnumerable<DriverViewModel>>(await _driverService.GetAll());

            foreach (var item in driverList)
            {
                try
                {
                    item.Car = _mapper.Map<CarViewModel>(await _carService.FindById((int)item.CarId));
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
        public async Task<ActionResult> Create(DriverViewModel driverViewModel)
        {
            try
            {
                var driver = _mapper.Map<Driver>(driverViewModel);
                if (await _driverService.UniquenessCheck(driver))
                {
                    await _driverService.Add(driver);
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
                var driver = await _driverService.FindById(id);
                var driverViewModel = _mapper.Map<DriverViewModel>(driver);
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
        public async Task<ActionResult> Edit(DriverViewModel driverViewModel)
        {
            try
            {
                var driver = _mapper.Map<Driver>(driverViewModel);
                await _driverService.Update(driver);
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
                var driver = await _driverService.FindById(id);
                var driverViewModel = _mapper.Map<DriverViewModel>(driver);
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
        public async Task<ActionResult> Delete(DriverViewModel driverViewModel)
        {
            try
            {
                await _driverService.Delete(driverViewModel.Id);
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
        public async Task<ActionResult> GiveCar(GiveCarViewModel giveCarViewModel)
        {
            try
            {
                var car = await _carService.FindByGovernmentNumber(giveCarViewModel.CarGovernmentNumber);
                var driver = await _driverService.FindByDriverLicenseNumber(giveCarViewModel.DriverLicenseNumber);
                await _driverService.GiveCar(driver.Id, car.Id);
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