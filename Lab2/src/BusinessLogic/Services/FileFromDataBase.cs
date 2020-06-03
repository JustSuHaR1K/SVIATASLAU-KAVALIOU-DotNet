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

        private readonly IRepository<EventDto> _eventsRepository;

        private readonly IRepository<MasterDto> _mastersRepository;

        private readonly IRepository<OrderDto> _ordersRepository;

        private readonly IMapper _mapper;

        private readonly IWriter _writer;

        public FileFromDataBase(IRepository<ClientDto> clientsRepository, IRepository<EventDto> eventsRepository, IRepository<MasterDto> mastersRepository, IRepository<OrderDto> ordersRepository, IMapper mapper, IWriter writer)
        {
            _clientsRepository = clientsRepository;
            _eventsRepository = eventsRepository;
            _mastersRepository = mastersRepository;
            _ordersRepository = ordersRepository;
            _mapper = mapper;
            _writer = writer;
        }

        public void ImportAndExportData(string eventsPath, string clientsPath, string mastersPath, string ordersPath)
        {
            //Read data
            var events = _mapper.Map<IEnumerable<Event>>(_eventsRepository.Get());
            var clients = _mapper.Map<IEnumerable<Client>>(_clientsRepository.Get());
            var masters = _mapper.Map<IEnumerable<Master>>(_mastersRepository.Get());
            var orders = _mapper.Map<IEnumerable<Order>>(_ordersRepository.Get());

            //Write data
            try
            {
                _writer.Write(events, eventsPath);
                _writer.Write(clients, clientsPath);
                _writer.Write(masters, mastersPath);
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