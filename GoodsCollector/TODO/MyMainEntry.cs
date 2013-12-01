using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace GoodsCollectorService.TODO
{
    internal class MyMainEntry
    {
        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        public static void MyMain(string[] args)
        {
            if (Environment.UserInteractive)
            {
                switch (args.Length)
                {
                    case 0:
                        AttachConsole(ATTACH_PARENT_PROCESS);
                        ShowHelp();
                        break;
                    case 1:
                        try
                        {
                            switch (args[0])
                            {
                                case "/install":
                                    ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                                    return;
                                case "/start":
                                    new ServiceController("CSVWatcherService").Start();
                                    break;
                                case "/stop":
                                    new ServiceController("CSVWatcherService").Stop();
                                    break;
                                case "/uninstall":
                                    using (var serv = new ServiceController("CSVWatcherService"))
                                    {
                                        if (serv.Status != ServiceControllerStatus.Stopped)
                                            serv.Stop();
                                    }
                                    ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                                    return;
                                default:
                                    AttachConsole(ATTACH_PARENT_PROCESS);
                                    CSVFileProcessor.Tracer = ShowProcessing;
                                    CSVFileProcessor.ProcessFolder(args[0]);
                                    break;
                            }
                        }
                        catch { }
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
                new CSVWatcherService() 
                };
                ServiceBase.Run(ServicesToRun);
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine(
                "\nThis application was made to watch special folder \"E:\\rep_csv\" and to write data from them into DataBase %\\Prodution.mdf");
            Console.WriteLine("Commands:\n");
            Console.WriteLine("/install\tInstall this app as service");
            Console.WriteLine("/uninstall\tUninstall service installed from this app");
            Console.WriteLine("/start\t\tStart service which was installed from this app");
            Console.WriteLine("/stop\t\tStop service which was installed from this app");
            Console.WriteLine("[Folder Name]\tProcess \"folder name\"");
        }

        private static void ShowProcessing(string obj)
        {
            Console.WriteLine(obj);
        }
    }
}
