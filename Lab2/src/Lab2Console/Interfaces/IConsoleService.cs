using System.Threading.Tasks;

namespace Eventus.ConsoleUI.Interfaces
{
    public interface IConsoleService<T>
    {
        void PrintMenu();

        Task StartMenu();
    }
}