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
            AddLog("start");
            string dir = ConfigurationManager.AppSettings["dirToWatch"];
            watcher = new FileSystemWatcher(dir, "*.csv");
            watcher.Created += watcher_Created;
            watcher.EnableRaisingEvents = true;Program
        }

        protected override void OnStop()
        {
             if (watcher != null)
                watcher.Dispose();

             AddLog("stop");
        }

        private void watcher_Created(object sender, FileSystemEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(ProcessCSVFile, e.FullPath);
        }
        
        private void ProcessCSVFile(object filePath)
        {
            var path = (string)filePath;
            var fileName = Path.GetFileNameWithoutExtension(path);
            DataBase.Stock stockInfo = new DataBase.Stock();
            try
            {
                ParseFileName(fileName, stockInfo);
                ParseFile(path, stockInfo);
            }
            //catch (FormatException fe) { AddLog(fe.Message); }
            //catch (InvalidOperationException ioe) { AddLog(ioe.Message); }
            catch (Exception e) { AddLog(e.Message); }
        }

        private void ParseFile(string path, DataBase.Stock stockInfo)
        {
            if (File.Exists(path))
            {
                try
                {
                    var rows = File.ReadAllLines(path);
                    using (DataBase.ProductionEntities db = new DataBase.ProductionEntities())
                    {
                        foreach (var row in rows)
                        {
                            var stock = new DataBase.Stock()
                            {
                                ManagerID = stockInfo.ManagerID,
                                Date = stockInfo.Date
                            };

                            var columns = row.Split('\t');
                            stock.Client = columns[0];
                            stock.GoodsID = int.Parse(columns[1]);
                            stock.Cost = decimal.Parse(columns[2]);
                            
                            db.Stocks.Add(stock);
                        }
                        AddLog("saving " + path);
                        db.SaveChanges();
                        AddLog("saved " + path);
                    }
                }
                catch(InvalidOperationException ioe)
                {
                    throw ioe;
                }
            }
        }

        private void ParseFileName(string fileName, DataBase.Stock stock)
        {
            var stockInfo = fileName.Split('_');
            try
            {
                stock.ManagerID = int.Parse(stockInfo[1]);
                stock.Date = DateTime.ParseExact(stockInfo[2], "ddMMyyyy", CultureInfo.InvariantCulture);
            }
            catch { throw new FormatException(string.Format("FileName [{0}] have invalid format", fileName)); }
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
