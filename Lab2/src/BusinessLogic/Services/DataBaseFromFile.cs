using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Eventus.DAL.Interfaces;
using Eventus.DAL.Models;

namespace BusinessLogic.Services
{
    public class DataBaseFromFile
    {
        private readonly IRepository<ClientDto> _clientsRepository;

        private readonly IRepository<EventDto> _eventsRepository;

        private readonly IRepository<MasterDto> _mastersRepository;

        private readonly IRepository<OrderDto> _ordersRepository;

        private readonly IMapper _mapper;

        private readonly IReader _reader;

        public DataBaseFromFile(IRepository<ClientDto> clientsRepository, IRepository<EventDto> eventsRepository, IRepository<MasterDto> mastersRepository, IRepository<OrderDto> ordersRepository, IMapper mapper, IReader reader)
        {
            _clientsRepository = clientsRepository;
            _eventsRepository = eventsRepository;
            _mastersRepository = mastersRepository;
            _ordersRepository = ordersRepository;
            _mapper = mapper;
            _reader = reader;
        }

        public void ImportAndExportData(string carsPath, string clientsPath, string driversPath, string ordersPath)
        {
            //Read data
            var carsList = _mapper.Map<IEnumerable<EventDto>>(_reader.Read<Event>(carsPath));
            var clientsList = _mapper.Map<IEnumerable<ClientDto>>(_reader.Read<Client>(clientsPath));
            var driversList = _mapper.Map<IEnumerable<MasterDto>>(_reader.Read<Master>(driversPath));
            var ordersList = _mapper.Map<IEnumerable<OrderDto>>(_reader.Read<Order>(ordersPath));

            try
            {
                //Write data
                _eventsRepository.AddRange(carsList);
                _clientsRepository.AddRange(clientsList);
                _mastersRepository.AddRange(driversList);
                _ordersRepository.AddRange(ordersList);
            }
            catch (DbUpdateException)
            {
            }
            catch (Exception)
            {
            }
        }
    }
}