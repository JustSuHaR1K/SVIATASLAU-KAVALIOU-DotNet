using BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;
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
    public class DriverConsoleService : IConsoleService<DriverConsoleService>
    {
        private readonly IDriverService _driverProcessing;

        private readonly ICarService _carProcessing;

        private readonly ILogger _logger;

        public DriverConsoleService(IDriverService driverProcessing, ICarService carProcessing)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _driverProcessing = driverProcessing;
            _carProcessing = carProcessing;
        }

        public void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("Driver menu");
            Console.WriteLine("1.Show all driver");
            Console.WriteLine("2.Add driver");
            Console.WriteLine("3.Delete driver");
            Console.WriteLine("4.Update driver");
            Console.WriteLine("5.Find by license number");
            Console.WriteLine("6.Give car");
            Console.WriteLine("7.Back to the administrator menu");
        }

        public async Task StartMenu()
        {
            while (true)
            {
                PrintMenu();
                int.TryParse(Console.ReadLine(), out int menuNumber);
                switch (menuNumber)
                {
                    case (int)AdminsDriverMenu.AllDriver:
                        {
                            ShowDrivers(await _driverProcessing.GetAll());
                        }
                        break;

                    case (int)AdminsDriverMenu.AddDriver:
                        {
                            var driver = CreateDriver();
                            var validResults = driver.IsValid();
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
                                if (await _driverProcessing.UniquenessCheck(driver))
                                {
                                    await _driverProcessing.Add(driver);
                                    Console.WriteLine("Success");
                                }
                                else
                                {
                                    Console.WriteLine("This driver is already on the list");
                                }
                            }
                            Console.ReadKey();
                        }
                        break;

                    case (int)AdminsDriverMenu.DeleteDriver:
                        {
                            Console.WriteLine("Enter driver id");

                            try
                            {
                                await _driverProcessing.Delete(ConsoleHelper.EnterNumber());
                                Console.WriteLine("Delete is sucessfull");
                            }
                            catch (NullReferenceException exception)
                            {
                                _logger.Error($"Not found:{exception.Message}");
                                Console.WriteLine("Driver is not find");
                            }
                            catch (DbException exception)
                            {
                                _logger.Error($"Failed to remove:{exception.Message}");
                                Console.WriteLine("Find error");
                            }

                            Console.ReadKey();
                        }
                        break;

                    case (int)AdminsDriverMenu.UpdateDriver:
                        {
                        }
                        break;

                    case (int)AdminsDriverMenu.Find:
                        {
                            Console.Clear();
                            Console.WriteLine("Enter licence number:");
                            var driver = await _driverProcessing.FindByDriverLicenseNumber(Console.ReadLine());
                            if (driver == null)
                            {
                                Console.WriteLine("Licence number not find");
                            }
                            else
                            {
                                Console.WriteLine($"Surname | Name | Patronymic | Call sign | DriverLicenseNumber | Date of issue of drivers license | Is on holiday | Is sick leave");
                                Console.WriteLine($"{driver.Surname} | {driver.Name} | {driver.Patronymic} | {driver.CallSign} | {driver.DriverLicenseNumber} | {driver.DateOfIssueOfDriversLicense} | {driver.IsOnHoliday} | {driver.IsSickLeave}");
                            }
                            Console.ReadKey();
                        }
                        break;

                    case (int)AdminsDriverMenu.GiveCar:
                        {
                            Console.Clear();
                            Console.WriteLine("Enter car id");
                            var carId = ConsoleHelper.EnterNumber();
                            if (await _carProcessing.FindById(carId) != null)
                            {
                                Console.WriteLine("Enter driver id");
                                var driverId = ConsoleHelper.EnterNumber();
                                if (_driverProcessing.FindById(driverId) != null)
                                {
                                    try
                                    {
                                        await _driverProcessing.GiveCar(driverId, carId);
                                        Console.WriteLine("Sucessfull");
                                    }
                                    catch (DbUpdateException exception)
                                    {
                                        _logger.Error($"Update error:{exception.Message}");
                                        Console.WriteLine("This car is use");
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("Update error");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Driver not find");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Car not find");
                            }
                            Console.ReadKey();
                        }
                        break;

                    case (int)AdminsDriverMenu.AdminMenu:
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

        public static void ShowDrivers(IEnumerable<Driver> drivers)
        {
            Console.Clear();
            Console.WriteLine($"Surname | Name | Patronymic | Call sign | DriverLicenseNumber | Date of issue of drivers license | Is on holiday | Is sick leave");
            foreach (var driver in drivers)
            {
                Console.WriteLine($"{driver.Surname} | {driver.Name} | {driver.Patronymic} | {driver.CallSign} | {driver.DriverLicenseNumber} | {driver.DateOfIssueOfDriversLicense} | {driver.IsOnHoliday} | {driver.IsSickLeave}");
            }
            Console.ReadKey();
        }

        public static Driver CreateDriver()
        {
            Console.Clear();
            Console.WriteLine("Enter name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter surname:");
            string surname = Console.ReadLine();
            Console.WriteLine("Enter patronymic:");
            string patronymic = Console.ReadLine();
            Console.WriteLine("Enter driver license number:");
            string driverLicenseNumber = Console.ReadLine();
            Console.WriteLine("Enter date of issue of drivers license:");
            DateTime.TryParse(Console.ReadLine(), out DateTime dateOfIssueOfDriversLicense);
            Console.WriteLine("Enter call sign:");
            int callSign = ConsoleHelper.EnterNumber();
            Console.WriteLine("Enter the state of holiday (on holiday 1, otherwise 0)");
            string state = Console.ReadLine();
            bool isOnHoliday;
            if (state.Equals("1"))
            {
                isOnHoliday = true;
            }
            else
            {
                isOnHoliday = false;
            }
            Console.WriteLine("Enter the state of sick leave (on sick leave 1, otherwise 0)");
            state = Console.ReadLine();
            bool isSickLeave;

            if (state.Equals("1"))
            {
                isSickLeave = true;
            }
            else
            {
                isSickLeave = false;
            }

            var driver = new Driver()
            {
                CallSign = callSign,
                Surname = surname,
                Name = name,
                Patronymic = patronymic,
                DriverLicenseNumber = driverLicenseNumber,
                DateOfIssueOfDriversLicense = dateOfIssueOfDriversLicense,
                IsSickLeave = isSickLeave,
                IsOnHoliday = isOnHoliday
            };

            return driver;
        }
    }
}