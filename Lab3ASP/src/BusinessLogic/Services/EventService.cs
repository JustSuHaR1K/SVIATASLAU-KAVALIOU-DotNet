using AutoMapper;
using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eventus.BusinessLogic.Interfaces;
using Eventus.DAL.Interfaces;
using Eventus.DAL.Models;

namespace Eventus.BusinessLogic.Services
{
    public class EventService : IEventService
    {
        private readonly IRepository<EventDto> _eventRepository;

        private readonly IRepository<MasterDto> _masterRepository;

        private readonly IMapper _mapper;

        public EventService(IRepository<EventDto> eventRepository, IRepository<MasterDto> masterRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _masterRepository = masterRepository;
            _mapper = mapper;
        }

        public async Task Add(Event master)
        {
            var newEvent = _mapper.Map<EventDto>(master);
            await _eventRepository.Create(newEvent);
        }

        public async Task Delete(int id)
        {
            var eventus = await _eventRepository.FindById(id);
            var master = await _masterRepository.FindById(id);
            if (eventus != null && master != null)
            {
                master.EventusId = null;
                await _masterRepository.Update(master);
                await _eventRepository.Remove(eventus);
            }
            else if (eventus != null && master == null)
            {
                await _eventRepository.Remove(eventus);
            }
        }

        public async Task<Event> FindById(int id)
        {
            return _mapper.Map<Event>(await _eventRepository.FindById(id));
        }

        public async Task Update(Event master)
        {
            var newEvent = _mapper.Map<EventDto>(master);
            await _eventRepository.Update(newEvent);
        }

        public async Task<IEnumerable<Event>> GetAll()
        {
            return _mapper.Map<IEnumerable<Event>>(await _eventRepository.Get());
        }

        public async Task<IEnumerable<Event>> EventsOnRework()
        {
            var events = await _eventRepository.Get();
            return _mapper.Map<IEnumerable<Event>>(events.Where(e => e.IsRework));
        }

        public async Task<IEnumerable<Event>> LongEvents(int duration)
        {
            var events = await _eventRepository.Get();
            return _mapper.Map<IEnumerable<Event>>(events.Where(e => e.EventDuration <= duration));
        }

        public async Task<Event> FindByGovernmentNumberOfService(string governmentNumberOfService)
        {
            var events = await _eventRepository.Get();
            var eventus = events.FirstOrDefault(e => e.GovernmentNumberOfService.Equals(governmentNumberOfService));
            return _mapper.Map<Event>(eventus);
        }

        public async Task<bool> UniquenessCheck(Event eventus)
        {
            var events = await _eventRepository.Get();
            try
            {
                var resultOfFind = events.Single(e => e.GovernmentNumberOfService.Equals(eventus.GovernmentNumberOfService) || e.PriceOfTheEvent.Equals(eventus.PriceOfTheEvent));
                return false;
            }
            catch (ArgumentNullException)
            {
                return true;
            }
            catch (InvalidOperationException)
            {
                return true;
            }
        }
    }
}