using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using Eventus.DAL.Interfaces;

namespace Eventus.DAL.Models
{
    public class MasterDto : IEntity
    {
        public MasterDto()
        {
        }

        public MasterDto(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        private readonly ILazyLoader _lazyLoader;

        private ICollection<OrderDto> orders;

        private EventDto eventus;

        public int Id { get; set; }

        public int? EventusId { get; set; }

        public string Profession { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public string MasterLicenseNumber { get; set; }

        public DateTime DateOfIssueOfAnEntrepreneurialLicense { get; set; }

        public bool IsSickLeave { get; set; }

        public bool IsOnHoliday { get; set; }

        public ICollection<OrderDto> Orders
        {
            get => _lazyLoader.Load(this, ref orders);
            set => orders = value;
        }

        public EventDto Eventus
        {
            get => _lazyLoader.Load(this, ref eventus);
            set => eventus = value;
        }
    }
}