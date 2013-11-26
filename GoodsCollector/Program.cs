using System.ServiceProcess;
using System.Configuration.Install;
using System.Reflection;
using System;

namespace GoodsCollectorServ
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
                switch (args.Length)
                {
                    case 0:

                        break;
                    case 1:
                        if (args[0].Contains("/install"))
                        {
                            ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                            return;
                        }
                        else if (args[0].Contains("/uninstall"))
                        {
                            ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                            return;
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                { 
                new CSVWatcherServ() 
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
