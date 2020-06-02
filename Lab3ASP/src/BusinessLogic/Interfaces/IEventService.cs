using BusinessLogic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventus.BusinessLogic.Interfaces
{
    public interface IEventService : IManager<global::BusinessLogic.Models.Event>
    {
        Task<IEnumerable<global::BusinessLogic.Models.Event>> GetEventOnRework();

        Task<IEnumerable<global::BusinessLogic.Models.Event>> GetOldEvents(int age);

        Task<global::BusinessLogic.Models.Event> FindByGovernmentNumber(string governmentNumber);

        Task<bool> UniquenessCheck(global::BusinessLogic.Models.Event car);
    }
}