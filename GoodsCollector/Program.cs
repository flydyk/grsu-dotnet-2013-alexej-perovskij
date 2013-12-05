
using System;
using System.ServiceProcess;
namespace GoodsCollectorService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                ConsoleRunner.ConsoleMain(args);
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                { 
                new CSVWatcherService() 
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
