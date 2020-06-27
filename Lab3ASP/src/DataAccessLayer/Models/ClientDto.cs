using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using Eventus.DAL.Interfaces;

namespace Eventus.DAL.Models
{
    public class ClientDto : IEntity
    {
        public ClientDto()
        {
        }

        public ClientDto(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        private readonly ILazyLoader _lazyLoader;

        private ICollection<OrderDto> orders;

        public int Id { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public ICollection<OrderDto> Orders
        {
            get => _lazyLoader.Load(this, ref orders);
            set => orders = value;
        }
    }
}