using BusinessLogic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventus.BusinessLogic.Interfaces
{
    public interface IEventService : IManager<Event>
    {
        Task<IEnumerable<Event>> EventsOnRework();

        Task<IEnumerable<Event>> LongEvents(int duration);

        Task<Event> FindByGovernmentNumberOfService(string governmentNumberOfService);

        Task<bool> UniquenessCheck(Event eventus);
    }
}