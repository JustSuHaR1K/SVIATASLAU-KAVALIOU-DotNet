using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IReader
    {
        Task<IEnumerable<T>> Read<T>(string path);
    }
}