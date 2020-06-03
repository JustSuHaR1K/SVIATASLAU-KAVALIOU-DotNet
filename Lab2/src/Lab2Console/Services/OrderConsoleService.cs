using BusinessLogic.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Eventus.BusinessLogic.Services;
using Eventus.BusinessLogic.Validations;
using Eventus.ConsoleUI.Enums;
using Eventus.ConsoleUI.Interfaces;
using ILogger = NLog.ILogger;

namespace Eventus.ConsoleUI.Services
{
    public class OrderConsoleService : IConsoleService<OrderConsoleService>
    {
        private readonly OrderService _orderProcessing;

        private readonly ILogger _logger;

        public OrderConsoleService(OrderService orderProcessing)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _orderProcessing = orderProcessing;
        }

        public void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("Order menu");
            Console.WriteLine("1.Show all orders");
            Console.WriteLine("2.Show active orders");
            Console.WriteLine("3.Show inactive orders");
            Console.WriteLine("4.Add order");
            Console.WriteLine("5.Back to administrator menu");
        }

        public async Task StartMenu()
        {
            while (true)
            {
                PrintMenu();
                int.TryParse(Console.ReadLine(), out int menuNumber);
                switch (menuNumber)
                {
                    case (int)AdminsOrderMenu.ShowAll:
                        {
                            ShowOrders(await _orderProcessing.GetAll());
                        }
                        break;

                    case (int)AdminsOrderMenu.ShowActive:
                        {
                            ShowOrders(await _orderProcessing.GetActiveOrders());
                        }
                        break;

                    case (int)AdminsOrderMenu.ShowInactive:
                        {
                            ShowOrders(await _orderProcessing.GetInActiveOrders());
                        }
                        break;

                    case (int)AdminsOrderMenu.Add:
                        {
                            const int NoError = 0;
                            var order = CreateOrder();
                            var validResults = order.IsValid();
                            if (validResults.Count() != NoError)
                            {
                                foreach (var result in validResults)
                                {
                                    Console.WriteLine(result.ErrorMessage);
                                }
                            }
                            else
                            {
                                try
                                {
                                    await _orderProcessing.Add(order);
                                    Console.WriteLine("Success");
                                }
                                catch (DbException exception)
                                {
                                    _logger.Error($"Add error:{exception.Message}");
                                    Console.WriteLine("Incorrect client id");
                                }
                                catch (Exception exception)
                                {
                                    _logger.Error($"Add error:{exception.Message}");
                                    Console.WriteLine("Adding error");
                                }
                                Console.ReadKey();
                            }
                        }
                        break;

                    case (int)AdminsOrderMenu.AdminMenu:
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

        public static void ShowOrders(IEnumerable<Order> orders)
        {
            Console.Clear();
            Console.WriteLine($"Cost | Date | Distance to party | Discount | Is done");
            foreach (var order in orders)
            {
                Console.WriteLine($"{order.Cost} | {order.Date} | {order.Distance} | {order.Discount} | {order.IsDone}");
            }
            Console.ReadKey();
        }

        private static Order CreateOrder()
        {
            Console.Clear();
            Console.WriteLine("Enter client id:");
            int clientId = ConsoleHelper.EnterNumber();
            Console.WriteLine("Enter cost:");
            double cost = ConsoleHelper.EnterDoubleNumber();
            Console.WriteLine("Enter distance to party:");
            double distance = ConsoleHelper.EnterDoubleNumber();
            Console.WriteLine("Enter discount");
            double discount = ConsoleHelper.EnterDoubleNumber();

            var order = new Order()
            {
                Date = DateTime.Now,
                IsDone = false,
                Cost = cost,
                Distance = distance,
                Discount = discount,
                ClientId = clientId
            };

            return order;
        }
    }
}