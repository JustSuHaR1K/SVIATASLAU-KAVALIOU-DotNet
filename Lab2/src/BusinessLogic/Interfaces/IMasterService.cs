using BusinessLogic.Models;
using System.Threading.Tasks;

namespace Eventus.BusinessLogic.Interfaces
{
    public interface IMasterService : IManager<Master>
    {
        Task<Master> FindByDriverLicenseNumber(string licenseNumber);

        Task GiveCar(int masterId, int eventId);

        Task<bool> UniquenessCheck(Master master);
    }
}