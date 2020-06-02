using BusinessLogic.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Eventus.BusinessLogic.Interfaces;
using Eventus.BusinessLogic.Validations;
using Eventus.ConsoleUI.Enums;
using Eventus.ConsoleUI.Interfaces;
using ILogger = NLog.ILogger;


namespace Eventus.ConsoleUI.Services
{
    public class EventConsoleService : IConsoleService<EventConsoleService>
    {
        private readonly ILogger _logger;
        private readonly IEventService _eventProcessing;

        public EventConsoleService(IEventService eventProcessing)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _eventProcessing = eventProcessing;
        }

        public void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("Event menu");
            Console.WriteLine("1.Show all events");
            Console.WriteLine("2.Add event");
            Console.WriteLine("3.Delete event");
            Console.WriteLine("4.Update event");
            Console.WriteLine("5.event on rework");
            Console.WriteLine("6.Find event by government number");
            Console.WriteLine("7.Get old events");
            Console.WriteLine("8.Return to the administrator menu");
        }

        public async Task StartMenu()
        {
            while (true)
            {
                PrintMenu();
                int.TryParse(Console.ReadLine(), out int menuNumber);
                switch (menuNumber)
                {
                    case (int)AdminsEventMenu.AllCar:
                        {
                            ShowEvents(await _eventProcessing.GetAll());
                        }
                        break;

                    case (int)AdminsEventMenu.AddCar:
                        {
                            var eventus = CreateEvent();
                            var validResults = eventus.IsValid();
                            const int NoError = 0;
                            if (validResults.Count() != NoError)
                            {
                                foreach (var result in validResults)
                                {
                                    Console.WriteLine(result.ErrorMessage);
                                }
                            }
                            else
                            {
                                if (await _eventProcessing.UniquenessCheck(eventus))
                                {
                                    await _eventProcessing.Add(eventus);
                                    Console.WriteLine("Success");
                                }
                                else
                                {
                                    Console.WriteLine("This car is already on the list");
                                }
                            }
                            Console.ReadKey();
                        }
                        break;

                    case (int)AdminsEventMenu.DeleteEvent:
                        {
                            Console.WriteLine("Enter car id");
                            try
                            {
                                await _eventProcessing.Delete(ConsoleHelper.EnterNumber());
                                Console.WriteLine("Delete is sucessfull");
                            }
                            catch (DbException exception)
                            {
                                _logger.Error($"Failed to remove:{exception.Message}");
                                Console.WriteLine("Failed to remove the car");
                            }
                            catch (Exception exception)
                            {
                                _logger.Error($"Delete error:{exception.Message}");
                                Console.WriteLine("Delete error");
                            }

                            Console.ReadKey();
                        }
                        break;

                    case (int)AdminsEventMenu.UpdateEvent:
                        {
                        }
                        break;

                    case (int)AdminsEventMenu.OnRepair:
                        {
                            ShowEvents(await _eventProcessing.GetEventOnRework());
                        }
                        break;

                    case (int)AdminsEventMenu.Find:
                        {
                            try
                            {
                                var eventus = await _eventProcessing.FindByGovernmentNumber(Console.ReadLine());
                                Console.WriteLine("Goverment number | Model | Color | Registration number | Year of issue | Is repair");
                                Console.WriteLine($"{eventus.GovernmentNumber} | {eventus.Model} | {eventus.Color} | {eventus.RegistrationNumber} | {eventus.YearOfIssue} | {eventus.IsRework}");
                            }
                            catch (Exception exception)
                            {
                                _logger.Error($"Not found:{exception.Message}");
                                Console.WriteLine("Event is not find");
                            }

                            Console.ReadKey();
                        }
                        break;

                    case (int)AdminsEventMenu.OldEvent:
                        {
                            Console.Clear();
                            Console.WriteLine("Max age:");
                            ShowEvents(await _eventProcessing.GetOldEvents(ConsoleHelper.EnterNumber()));
                        }
                        break;

                    case (int)AdminsEventMenu.AdminMenu:
                        {
                            return;
                        }
                    default:
                        {
                            Console.Clear();
                            Console.WriteLine("Incorrect number");
                        }
                        break;
                }
            }
        }

        private static void ShowEvents(IEnumerable<global::BusinessLogic.Models.Event> events)
        {
            Console.Clear();
            Console.WriteLine("Goverment number | Model | Color | Registration number | Year of issue | Is repair");
            foreach (var eventus in events)
            {
                Console.WriteLine($"{eventus.GovernmentNumber} | {eventus.Model} | {eventus.Color} | {eventus.RegistrationNumber} | {eventus.YearOfIssue} | {eventus.IsRework}");
            }
            Console.ReadKey();
        }

        private static global::BusinessLogic.Models.Event CreateEvent()
        {
            Console.Clear();
            Console.WriteLine("Enter government number:");
            string governmentNumber = Console.ReadLine();
            Console.WriteLine("Enter registration number:");
            string registrationNumber = Console.ReadLine();
            Console.WriteLine("Enter model:");
            string model = Console.ReadLine();
            Console.WriteLine("Enter color:");
            string color = Console.ReadLine();
            Console.WriteLine("Enter year of issue:");
            int yearOfIssue = ConsoleHelper.EnterNumber();
            Console.WriteLine("Enter the state of the eventus (repair 1, otherwise 0)");
            string chooseState = Console.ReadLine();
            bool isRework;

            if (chooseState.Equals("1"))
            {
                isRework = true;
            }
            else
            {
                isRework = false;
            }

            var eventus = new global::BusinessLogic.Models.Event()
            {
                GovernmentNumber = governmentNumber,
                Model = model,
                Color = color,
                YearOfIssue = yearOfIssue,
                RegistrationNumber = registrationNumber,
                IsRework = isRework
            };

            return eventus;
        }
    }
}