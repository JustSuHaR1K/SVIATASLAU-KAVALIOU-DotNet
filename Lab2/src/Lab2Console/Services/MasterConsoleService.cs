﻿using BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;
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
    public class MasterConsoleService : IConsoleService<MasterConsoleService>
    {
        private readonly IMasterService _masterProcessing;

        private readonly IEventService _eventProcessing;

        private readonly ILogger _logger;

        public MasterConsoleService(IMasterService masterProcessing, IEventService eventProcessing)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _masterProcessing = masterProcessing;
            _eventProcessing = eventProcessing;
        }

        public void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("Master menu");
            Console.WriteLine("1.Show all master");
            Console.WriteLine("2.Add master");
            Console.WriteLine("3.Delete master");
            Console.WriteLine("4.Update master");
            Console.WriteLine("5.Find by license number");
            Console.WriteLine("6.Give event");
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
                    case (int)AdminsMasterMenu.AllMaster:
                        {
                            ShowMasters(await _masterProcessing.GetAll());
                        }
                        break;

                    case (int)AdminsMasterMenu.AddMaster:
                        {
                            var master = CreateMaster();
                            var validResults = master.IsValid();
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
                                if (await _masterProcessing.UniquenessCheck(master))
                                {
                                    await _masterProcessing.Add(master);
                                    Console.WriteLine("Success");
                                }
                                else
                                {
                                    Console.WriteLine("This master is already on the list");
                                }
                            }
                            Console.ReadKey();
                        }
                        break;

                    case (int)AdminsMasterMenu.DeleteMaster:
                        {
                            Console.WriteLine("Enter master id");

                            try
                            {
                                await _masterProcessing.Delete(ConsoleHelper.EnterNumber());
                                Console.WriteLine("Delete is sucessfull");
                            }
                            catch (NullReferenceException exception)
                            {
                                _logger.Error($"Not found:{exception.Message}");
                                Console.WriteLine("Master is not find");
                            }
                            catch (DbException exception)
                            {
                                _logger.Error($"Failed to remove:{exception.Message}");
                                Console.WriteLine("Find error");
                            }

                            Console.ReadKey();
                        }
                        break;

                    case (int)AdminsMasterMenu.UpdateMaster:
                        {
                        }
                        break;

                    case (int)AdminsMasterMenu.Find:
                        {
                            Console.Clear();
                            Console.WriteLine("Enter licence number:");
                            var master = await _masterProcessing.FindByMasterLicenseNumber(Console.ReadLine());
                            if (master == null)
                            {
                                Console.WriteLine("Licence number not find");
                            }
                            else
                            {
                                Console.WriteLine($"Surname | Name | Patronymic | Profession | MasterLicenseNumber | Date of issue an entrepreneurial license | Is on holiday | Is sick leave");
                                Console.WriteLine($"{master.Surname} | {master.Name} | {master.Patronymic} | {master.Profession} | {master.MasterLicenseNumber} | {master.DateOfIssueOfAnEntrepreneurialLicense} | {master.IsOnHoliday} | {master.IsSickLeave}");
                            }
                            Console.ReadKey();
                        }
                        break;

                    case (int)AdminsMasterMenu.GiveEvent:
                        {
                            Console.Clear();
                            Console.WriteLine("Enter event id");
                            var eventId = ConsoleHelper.EnterNumber();
                            if (await _eventProcessing.FindById(eventId) != null)
                            {
                                Console.WriteLine("Enter master id");
                                var masterId = ConsoleHelper.EnterNumber();
                                if (_masterProcessing.FindById(masterId) != null)
                                {
                                    try
                                    {
                                        await _masterProcessing.GiveEvent(masterId, eventId);
                                        Console.WriteLine("Sucessfull");
                                    }
                                    catch (DbUpdateException exception)
                                    {
                                        _logger.Error($"Update error:{exception.Message}");
                                        Console.WriteLine("This event is use");
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("Update error");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Master not find");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Event not find");
                            }
                            Console.ReadKey();
                        }
                        break;

                    case (int)AdminsMasterMenu.AdminMenu:
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

        public static void ShowMasters(IEnumerable<Master> masters)
        {
            Console.Clear();
            Console.WriteLine($"Surname | Name | Patronymic | Profession | MasterLicenseNumber | Date of issue an entrepreneurial license | Is on holiday | Is sick leave");
            foreach (var master in masters)
            {
                Console.WriteLine($"{master.Surname} | {master.Name} | {master.Patronymic} | {master.Profession} | {master.MasterLicenseNumber} | {master.DateOfIssueOfAnEntrepreneurialLicense} | {master.IsOnHoliday} | {master.IsSickLeave}");
            }
            Console.ReadKey();
        }

        public static Master CreateMaster()
        {
            Console.Clear();
            Console.WriteLine("Enter name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter surname:");
            string surname = Console.ReadLine();
            Console.WriteLine("Enter patronymic:");
            string patronymic = Console.ReadLine();
            Console.WriteLine("Enter master license number:");
            string masterLicenseNumber = Console.ReadLine();
            Console.WriteLine("Enter date of issue of an entrepreneurial license:");
            DateTime.TryParse(Console.ReadLine(), out DateTime dateOfIssueOfAnEntrepreneurialLicense);
            Console.WriteLine("Enter profession:");
            string profession = Console.ReadLine();
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

            var master = new Master()
            {
                Profession = profession,
                Surname = surname,
                Name = name,
                Patronymic = patronymic,
                MasterLicenseNumber = masterLicenseNumber,
                DateOfIssueOfAnEntrepreneurialLicense = dateOfIssueOfAnEntrepreneurialLicense,
                IsSickLeave = isSickLeave,
                IsOnHoliday = isOnHoliday
            };

            return master;
        }
    }
}