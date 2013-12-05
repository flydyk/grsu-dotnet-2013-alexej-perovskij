using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using GoodsCollectorService.Controllers;

namespace GoodsCollectorService
{
    public partial class CSVWatcherService : ServiceBase
    {
        FileSystemWatcher watcher;

        public CSVWatcherService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ThreadPool.SetMaxThreads(10, 100);
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
            ThreadPool.QueueUserWorkItem(ProcessFile, e.FullPath);
        }

        private void ProcessFile(object state)
        {
            try
            {
                var path = (string)state;
                using (StocksController c = new StocksController())
                {
                    c.Tracer = AddLog;
                    c.AddStocksFromFile(path);
                }
            }
            catch (Exception e) { AddLog(e.Message); }
        }

        private void AddLog(string log)
        {
            string serviceName = "CSVWatcherService";
            try
            {
                if (!EventLog.SourceExists(serviceName))
                {
                    EventLog.CreateEventSource(serviceName, serviceName);
                }
                eventLog1.Source = serviceName;
                eventLog1.WriteEntry(log);
            }
            catch { }
        }
    }
}
