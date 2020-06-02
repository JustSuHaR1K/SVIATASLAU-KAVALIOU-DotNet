using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventus.BusinessLogic.Interfaces
{
    public interface IManager<T>
    {
        Task Add(T master);

        Task Delete(int id);

        Task Update(T master);

        Task<IEnumerable<T>> GetAll();

        Task<T> FindById(int id);
    }
}