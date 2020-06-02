using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using Eventus.DAL.Interfaces;

namespace Eventus.DAL.Models
{
    public class OrderDto : IEntity
    {
        public OrderDto()
        {
        }

        public OrderDto(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        private readonly ILazyLoader _lazyLoader;

        private MasterDto master;

        private ClientDto client;

        public int Id { get; set; }

        public DateTime Date { get; set; }

        public bool IsDone { get; set; }

        public double Cost { get; set; }

        public double Distance { get; set; }

        public double Discount { get; set; }

        public int? MasterId { get; set; }

        public int ClientId { get; set; }

        public MasterDto Master
        {
            get => _lazyLoader.Load(this, ref master);
            set => master = value;
        }

        public ClientDto Client
        {
            get => _lazyLoader.Load(this, ref client);
            set => client = value;
        }
    }
}