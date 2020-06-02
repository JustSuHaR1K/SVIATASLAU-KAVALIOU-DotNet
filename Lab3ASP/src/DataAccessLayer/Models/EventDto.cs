using Microsoft.EntityFrameworkCore.Infrastructure;
using Eventus.DAL.Interfaces;

namespace Eventus.DAL.Models
{
    public class EventDto : IEntity
    {
        private readonly ILazyLoader _lazyLoader;

        public EventDto()
        {
        }

        public EventDto(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        public int Id { get; set; }

        public string GovernmentNumber { get; set; }

        public string Model { get; set; }

        public string Color { get; set; }

        public int YearOfIssue { get; set; }

        public string RegistrationNumber { get; set; }

        public bool IsRepair { get; set; }

        private MasterDto master;

        public MasterDto Master
        {
            get => _lazyLoader.Load(this, ref master);
            set => master = value;
        }
    }
}