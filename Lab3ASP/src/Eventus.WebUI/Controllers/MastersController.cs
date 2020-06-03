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

        private readonly ILogger<MastersController> _logger;

        private readonly IMapper _mapper;

        public MastersController(IEventService eventService, IMasterService masterService, ILogger<MastersController> logger, IMapper mapper)
        {
            _eventService = eventService;
            _masterService = masterService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ActionResult> Masters()
        {
            var masterList = _mapper.Map<IEnumerable<MasterViewModel>>(await _masterService.GetAll());

            foreach (var item in masterList)
            {
                try
                {
                    item.Event = _mapper.Map<EventViewModel>(await _eventService.FindById((int)item.EventusId));
                }
                catch (InvalidOperationException exception)
                {
                    _logger.LogError($"Find event error:{exception.Message}");
                }
            }

            return View(masterList);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MasterViewModel masterViewModel)
        {
            try
            {
                var master = _mapper.Map<Master>(masterViewModel);
                if (await _masterService.UniquenessCheck(master))
                {
                    await _masterService.Add(master);
                }
                return RedirectToAction(nameof(Masters));
            }
            catch (Exception exception)
            {
                _logger.LogError($"Master create error:{exception.Message}");
                return View(masterViewModel);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var master = await _masterService.FindById(id);
                var masterViewModel = _mapper.Map<MasterViewModel>(master);
                return View(masterViewModel);
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception.Message);
                return RedirectToAction(nameof(Masters));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MasterViewModel masterViewModel)
        {
            try
            {
                var master = _mapper.Map<Master>(masterViewModel);
                await _masterService.Update(master);
                return RedirectToAction(nameof(Masters));
            }
            catch (Exception exception)
            {
                _logger.LogError($"Master update error:{exception.Message}");
                return View();
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var master = await _masterService.FindById(id);
                var masterViewModel = _mapper.Map<MasterViewModel>(master);
                return View(masterViewModel);
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception.Message);
                return RedirectToAction(nameof(Masters));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(MasterViewModel masterViewModel)
        {
            try
            {
                await _masterService.Delete(masterViewModel.Id);
                return RedirectToAction(nameof(Masters));
            }
            catch (Exception exception)
            {
                _logger.LogError($"Event master error:{exception.Message}");
                return View();
            }
        }

        public ActionResult GiveEvent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GiveEvent(GiveEventViewModel giveEventViewModel)
        {
            try
            {
                var eventus = await _eventService.FindByGovernmentNumberOfService(giveEventViewModel.EventGovernmentNumber);
                var master = await _masterService.FindByMasterLicenseNumber(giveEventViewModel.MasterLicenseNumber);
                await _masterService.GiveEvent(master.Id, eventus.Id);
                return RedirectToAction(nameof(Masters));
            }
            catch (Exception exception)
            {
                _logger.LogError($"Give event error:{exception.Message}");
                return View();
            }
        }
    }
}