using BusinessLogic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventus.BusinessLogic.Interfaces
{
    public interface IEventService : IManager<Event>
    {
        Task<IEnumerable<Event>> GetEventOnRework();

        Task<IEnumerable<Event>> GetLongEvents(int duration);

        Task<Event> FindByGovernmentNumberOfService(string governmentNumberOfService);

        Task<bool> UniquenessCheck(Event eventus);
    }
}