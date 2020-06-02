using Ninject;
using System;
using System.Threading.Tasks;
using Eventus.ConsoleUI.Configuration;
using Eventus.ConsoleUI.Interfaces;
using Eventus.ConsoleUI.Services;

namespace Event.ConsoleUI
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            IKernel kernel = new StandardKernel(new NinjectConfiguration());
            IConsoleService<RoleService> roleInterface = kernel.Get<RoleService>();
            await roleInterface.StartMenu();
            Console.ReadKey();
        }
    }
}