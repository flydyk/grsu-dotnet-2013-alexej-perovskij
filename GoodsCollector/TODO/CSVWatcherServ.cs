using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.ServiceProcess;
using System.Threading;

namespace GoodsCollectorServ
{
    public partial class CSVWatcherServ : ServiceBase
    {
        FileSystemWatcher watcher;

        public CSVWatcherServ()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            TODO.CSVFileProcessor.Tracer = AddLog;
            AddLog("start");
            string dir = ConfigurationManager.AppSettings["dirToWatch"];
            watcher = new FileSystemWatcher(dir, "*.csv");
            watcher.Created += watcher_Created;
            watcher.EnableRaisingEvents = true;
        }

        protected override void OnStop()
        {
             if (watcher != null)
                watcher.Dispose();

             AddLog("stop");
        }

        private void watcher_Created(object sender, FileSystemEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(TODO.CSVFileProcessor.ProcessFile, e.FullPath);
        }
        
        private void AddLog(string log)
        {
            try
            {
                if (!EventLog.SourceExists("CSVWatcherServ"))
                {
                    EventLog.CreateEventSource("CSVWatcherServ", "CSVWatcherServ");
                }
                eventLog1.Source = "CSVWatcherServ";
                eventLog1.WriteEntry(log);
            }
            catch { }
        }
    }
}
