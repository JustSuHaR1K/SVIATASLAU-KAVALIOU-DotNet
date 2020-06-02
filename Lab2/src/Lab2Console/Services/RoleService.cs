using System;
using System.Threading.Tasks;
using Eventus.ConsoleUI.Enums;
using Eventus.ConsoleUI.Interfaces;

namespace Eventus.ConsoleUI.Services
{
    public class RoleService : IConsoleService<RoleService>
    {
        private readonly IConsoleService<AdminConsoleService> _adminMenu;

        public RoleService(IConsoleService<AdminConsoleService> adminMenu)
        {
            _adminMenu = adminMenu;
        }

        public void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("Choose role");
            Console.WriteLine("1.Administrator");
            Console.WriteLine("2.Master");
            Console.WriteLine("3.Client");
            Console.WriteLine("4.Exit");
        }

        public async Task StartMenu()
        {
            while (true)
            {
                PrintMenu();
                int.TryParse(Console.ReadLine(), out int menuNumber);
                switch (menuNumber)
                {
                    case (int)MainMenu.Administrator:
                        {
                            await _adminMenu.StartMenu();
                        }
                        break;

                    case (int)MainMenu.Master:
                        {
                        }
                        break;

                    case (int)MainMenu.Client:
                        {
                        }
                        break;

                    case (int)MainMenu.Exit:
                        {
                            Environment.Exit(0);
                        }
                        break;

                    default:
                        {
                            Console.WriteLine("Incorrect number");
                        }
                        break;
                }
            }
        }
    }
}