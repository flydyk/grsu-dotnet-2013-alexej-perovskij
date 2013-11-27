using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodsCollectorService.TODO
{
    public class CSVFileProcessor
    {
        private static Action<string> tracer;

        public static Action<string> Tracer
        {
            get { return tracer; }
            set { tracer = value; }
        }

        public static void ProcessFolder(string folderPath)
        {
            var fileNames = Directory.GetFiles(folderPath, "*.csv", SearchOption.TopDirectoryOnly);

            foreach (var fileName in fileNames)
            {
                ProcessFile(fileName);
            }
        }
        public static void ProcessFile(object filePath)
        {
            var path = (string)filePath;
            var fileName = Path.GetFileNameWithoutExtension(path);
            DataBase.Stock stockInfo = new DataBase.Stock();
            try
            {
                ParseFileName(fileName, stockInfo);
                ParseFile(path, stockInfo);
            }
            //catch (FormatException fe) { logging(fe.Message); }
            //catch (InvalidOperationException ioe) { logging(ioe.Message); }
            catch (Exception e) { tracer(e.Message); }
        }

        private static void ParseFile(string path, DataBase.Stock stockInfo)
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
                        tracer("saving " + path);
                        db.SaveChanges();
                        tracer("saved " + path);
                    }
                }
                catch (InvalidOperationException ioe)
                {
                    throw ioe;
                }
            }
        }

        private static void ParseFileName(string fileName, DataBase.Stock stock)
        {
            var stockInfo = fileName.Split('_');
            try
            {
                stock.ManagerID = int.Parse(stockInfo[1]);
                stock.Date = DateTime.ParseExact(stockInfo[2], "ddMMyyyy", CultureInfo.InvariantCulture);
            }
            catch { throw new FormatException(string.Format("FileName [{0}] have invalid format", fileName)); }
        }
    }
}
