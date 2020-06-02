using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.IO;
using Eventus.DAL.Interfaces;
using Eventus.DAL.Models;

namespace BusinessLogic.Services
{
    public class FileFromDataBase
    {
        private readonly IRepository<ClientDto> _clientsRepository;

        private readonly IRepository<EventDto> _carsRepository;

        private readonly IRepository<MasterDto> _driversRepository;

        private readonly IRepository<OrderDto> _ordersRepository;

        private readonly IMapper _mapper;

        private readonly IWriter _writer;

        public FileFromDataBase(IRepository<ClientDto> clientsRepository, IRepository<EventDto> carsRepository, IRepository<MasterDto> driversRepository, IRepository<OrderDto> ordersRepository, IMapper mapper, IWriter writer)
        {
            _clientsRepository = clientsRepository;
            _carsRepository = carsRepository;
            _driversRepository = driversRepository;
            _ordersRepository = ordersRepository;
            _mapper = mapper;
            _writer = writer;
        }

        public void ImportAndExportData(string carsPath, string clientsPath, string driversPath, string ordersPath)
        {
            //Read data
            var cars = _mapper.Map<IEnumerable<Models.Event>>(_carsRepository.Get());
            var clients = _mapper.Map<IEnumerable<Client>>(_clientsRepository.Get());
            var drivers = _mapper.Map<IEnumerable<Master>>(_driversRepository.Get());
            var orders = _mapper.Map<IEnumerable<Order>>(_ordersRepository.Get());

            //Write data
            try
            {
                _writer.Write(cars, carsPath);
                _writer.Write(clients, clientsPath);
                _writer.Write(drivers, driversPath);
                _writer.Write(orders, ordersPath);
            }
            catch (IOException)
            {
            }
            catch (Exception)
            {
            }
        }
    }
}