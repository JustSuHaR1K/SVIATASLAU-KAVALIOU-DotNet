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
                    case (int)AdminsEventMenu.AllEvent:
                        {
                            ShowEvents(await _eventProcessing.GetAll());
                        }
                        break;

                    case (int)AdminsEventMenu.AddEvent:
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
                                    Console.WriteLine("This event is already on the list");
                                }
                            }
                            Console.ReadKey();
                        }
                        break;

                    case (int)AdminsEventMenu.DeleteEvent:
                        {
                            Console.WriteLine("Enter event id");
                            try
                            {
                                await _eventProcessing.Delete(ConsoleHelper.EnterNumber());
                                Console.WriteLine("Delete is sucessfull");
                            }
                            catch (DbException exception)
                            {
                                _logger.Error($"Failed to remove:{exception.Message}");
                                Console.WriteLine("Failed to remove the event");
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

                    case (int)AdminsEventMenu.OnRework:
                        {
                            ShowEvents(await _eventProcessing.GetEventOnRework());
                        }
                        break;

                    case (int)AdminsEventMenu.Find:
                        {
                            try
                            {
                                var eventus = await _eventProcessing.FindByGovernmentNumberOfService(Console.ReadLine());
                                Console.WriteLine("Name of Event | Government number of service | Description | Price of the event | Event duration | Is rework");
                                Console.WriteLine($"{eventus.NameOfEvent} | {eventus.GovernmentNumberOfService} | {eventus.Description} | {eventus.PriceOfTheEvent} | {eventus.EventDuration} | {eventus.IsRework}");
                            }
                            catch (Exception exception)
                            {
                                _logger.Error($"Not found:{exception.Message}");
                                Console.WriteLine("Event is not find");
                            }

                            Console.ReadKey();
                        }
                        break;

                    case (int)AdminsEventMenu.LongEvent:
                        {
                            Console.Clear();
                            Console.WriteLine("Max duration:");
                            ShowEvents(await _eventProcessing.GetLongEvents(ConsoleHelper.EnterNumber()));
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
            Console.WriteLine("Name of Event | Government number of service | Description | Price of the event | Event duration | Is rework");
            foreach (var eventus in events)
            {
                Console.WriteLine($"{eventus.NameOfEvent} | {eventus.GovernmentNumberOfService} | {eventus.Description} | {eventus.PriceOfTheEvent} | {eventus.EventDuration} | {eventus.IsRework}");
            }
            Console.ReadKey();
        }

        private static global::BusinessLogic.Models.Event CreateEvent()
        {
            Console.Clear();
            Console.WriteLine("Enter name:");
            string nameOfEvent = Console.ReadLine();
            Console.WriteLine("Enter government number of service:");
            string governmentNumber = Console.ReadLine();
            Console.WriteLine("Enter description:");
            string description = Console.ReadLine();
            Console.WriteLine("Enter price of the event number:");
            int priceOfTheEvent = ConsoleHelper.EnterNumber();
            Console.WriteLine("Enter event duration:");
            int eventDuration = ConsoleHelper.EnterNumber();
            Console.WriteLine("Enter the state of the eventus (rework 1, otherwise 0)");
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
                NameOfEvent = nameOfEvent,
                GovernmentNumberOfService = governmentNumber,
                Description = description,
                EventDuration = eventDuration,
                PriceOfTheEvent = priceOfTheEvent,
                IsRework = isRework
            };

            return eventus;
        }
    }
}