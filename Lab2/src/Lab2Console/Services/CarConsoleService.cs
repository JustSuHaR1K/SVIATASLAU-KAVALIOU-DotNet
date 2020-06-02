using BusinessLogic.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Taxi.BusinessLogic.Interfaces;
using Taxi.BusinessLogic.Validations;
using Taxi.ConsoleUI.Enums;
using Taxi.ConsoleUI.Interfaces;
using ILogger = NLog.ILogger;

namespace Taxi.ConsoleUI.Services
{
    public class CarConsoleService : IConsoleService<CarConsoleService>
    {
        private readonly ILogger _logger;
        private readonly ICarService _carProcessing;

        public CarConsoleService(ICarService carProcessing)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _carProcessing = carProcessing;
        }

        public void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("Car menu");
            Console.WriteLine("1.Show all cars");
            Console.WriteLine("2.Add car");
            Console.WriteLine("3.Delete car");
            Console.WriteLine("4.Update car");
            Console.WriteLine("5.Car on repair");
            Console.WriteLine("6.Find car by government number");
            Console.WriteLine("7.Get old cars");
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
                    case (int)AdminsCarMenu.AllCar:
                        {
                            ShowCars(await _carProcessing.GetAll());
                        }
                        break;

                    case (int)AdminsCarMenu.AddCar:
                        {
                            var car = CreateCar();
                            var validResults = car.IsValid();
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
                                if (await _carProcessing.UniquenessCheck(car))
                                {
                                    await _carProcessing.Add(car);
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

                    case (int)AdminsCarMenu.DeleteCar:
                        {
                            Console.WriteLine("Enter car id");
                            try
                            {
                                await _carProcessing.Delete(ConsoleHelper.EnterNumber());
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

                    case (int)AdminsCarMenu.UpdateCar:
                        {
                        }
                        break;

                    case (int)AdminsCarMenu.OnRepair:
                        {
                            ShowCars(await _carProcessing.GetCarOnRepair());
                        }
                        break;

                    case (int)AdminsCarMenu.Find:
                        {
                            try
                            {
                                var car = await _carProcessing.FindByGovernmentNumber(Console.ReadLine());
                                Console.WriteLine("Goverment number | Model | Color | Registration number | Year of issue | Is repair");
                                Console.WriteLine($"{car.GovernmentNumber} | {car.Model} | {car.Color} | {car.RegistrationNumber} | {car.YearOfIssue} | {car.IsRepair}");
                            }
                            catch (Exception exception)
                            {
                                _logger.Error($"Not found:{exception.Message}");
                                Console.WriteLine("Car is not find");
                            }

                            Console.ReadKey();
                        }
                        break;

                    case (int)AdminsCarMenu.OldCars:
                        {
                            Console.Clear();
                            Console.WriteLine("Max age:");
                            ShowCars(await _carProcessing.GetOldCars(ConsoleHelper.EnterNumber()));
                        }
                        break;

                    case (int)AdminsCarMenu.AdminMenu:
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

        private static void ShowCars(IEnumerable<Car> cars)
        {
            Console.Clear();
            Console.WriteLine("Goverment number | Model | Color | Registration number | Year of issue | Is repair");
            foreach (var car in cars)
            {
                Console.WriteLine($"{car.GovernmentNumber} | {car.Model} | {car.Color} | {car.RegistrationNumber} | {car.YearOfIssue} | {car.IsRepair}");
            }
            Console.ReadKey();
        }

        private static Car CreateCar()
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
            Console.WriteLine("Enter the state of the machine (repair 1, otherwise 0)");
            string chooseState = Console.ReadLine();
            bool isRepair;

            if (chooseState.Equals("1"))
            {
                isRepair = true;
            }
            else
            {
                isRepair = false;
            }

            var car = new Car()
            {
                GovernmentNumber = governmentNumber,
                Model = model,
                Color = color,
                YearOfIssue = yearOfIssue,
                RegistrationNumber = registrationNumber,
                IsRepair = isRepair
            };

            return car;
        }
    }
}