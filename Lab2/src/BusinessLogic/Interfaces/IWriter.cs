using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IWriter
    {
        Task Write<T>(IEnumerable<T> list, string path);
    }
}