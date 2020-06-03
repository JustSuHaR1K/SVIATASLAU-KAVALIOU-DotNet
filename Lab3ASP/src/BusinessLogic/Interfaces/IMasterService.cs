using BusinessLogic.Models;
using System.Threading.Tasks;

namespace Eventus.BusinessLogic.Interfaces
{
    public interface IMasterService : IManager<Master>
    {
        Task<Master> FindByMasterLicenseNumber(string licenseNumber);

        Task GiveEvent(int masterId, int eventId);

        Task<bool> UniquenessCheck(Master master);
    }
}