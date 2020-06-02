using System;
using System.Threading.Tasks;
using Eventus.ConsoleUI.Enums;
using Eventus.ConsoleUI.Interfaces;

namespace Eventus.ConsoleUI.Services
{
    public class AdminConsoleService : IConsoleService<AdminConsoleService>
    {
        private readonly IConsoleService<EventConsoleService> _eventService;

        private readonly IConsoleService<MasterConsoleService> _masterService;

        private readonly IConsoleService<OrderConsoleService> _orderService;

        public AdminConsoleService(IConsoleService<EventConsoleService> eventService, IConsoleService<MasterConsoleService> masterService, IConsoleService<OrderConsoleService> orderService)
        {
            _eventService = eventService;
            _masterService = masterService;
            _orderService = orderService;
        }

        public void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("Administrator menu");
            Console.WriteLine("1.Event menu");
            Console.WriteLine("2.Master menu");
            Console.WriteLine("3.Order menu");
            Console.WriteLine("4.Return to the main menu");
        }

        public async Task StartMenu()
        {
            while (true)
            {
                PrintMenu();
                int.TryParse(Console.ReadLine(), out int menuNumber);
                switch (menuNumber)
                {
                    case (int)AdminsMenu.Event:
                        {
                            await _eventService.StartMenu();
                        }
                        break;

                    case (int)AdminsMenu.Master:
                        {
                            await _masterService.StartMenu();
                        }
                        break;

                    case (int)AdminsMenu.Order:
                        {
                            await _orderService.StartMenu();
                        }
                        break;

                    case (int)AdminsMenu.MainMenu:
                        {
                            return;
                        }
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