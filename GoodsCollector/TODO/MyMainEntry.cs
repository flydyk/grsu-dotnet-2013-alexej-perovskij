using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace GoodsCollectorServ.TODO
{
    internal class MyMainEntry
    {
        public static void MyMain(string[] args)
        {
            if (Environment.UserInteractive)
            {
                switch (args.Length)
                {
                    case 0:
                        ShowHelp();
                        break;
                    case 1:
                        switch (args[0])
                        {
                            case "/install":
                                ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                                return;
                            case "/uninstall":
                                ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                                return;
                            default:
                                CSVFileProcessor.Tracer = ShowProcessing;
                                CSVFileProcessor.ProcessFolder(args[0]);
                                break;
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

        private static void ShowHelp()
        {
            Console.WriteLine(
                "This application was made to watch special folder \"E:\\rep_csv\" and to write data from them into DataBase %\\Prodution.mdf");
            Console.WriteLine("Commands:\n");
            Console.WriteLine("/install\nInstall this app as service");
            Console.WriteLine("/uninstall\nUninstall service installed from this app");
            Console.WriteLine("[Folder Name]\nProcess \"folder name\"");
            Console.ReadLine();
            
        }

        private static void ShowProcessing(string obj)
        {
            Console.WriteLine(obj);
        }
    }
}
