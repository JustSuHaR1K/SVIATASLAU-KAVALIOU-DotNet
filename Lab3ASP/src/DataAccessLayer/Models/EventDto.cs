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
        public string NameOfEvent { get; set; }

        public string CodeNumberOfService { get; set; }

        public string Description { get; set; }

        public int EventDuration { get; set; }

        public int PriceOfTheEvent { get; set; }

        public bool IsRework { get; set; }

        private MasterDto master;

        public MasterDto Master
        {
            get => _lazyLoader.Load(this, ref master);
            set => master = value;
        }
    }
}