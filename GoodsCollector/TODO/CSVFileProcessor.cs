using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodsCollectorService.TODO
{
    public static class CSVFileProcessor
    {
        
        public static List<DataBase.Stock> ParseFile(string path, DataBase.Manager manager)
        {
            if (File.Exists(path))
            {
                try
                {
                    var rows = File.ReadAllLines(path);
                    List<DataBase.Stock> stocks = new List<DataBase.Stock>();

                    foreach (var row in rows)
                    {
                        var stock = new DataBase.Stock()
                        {
                            ManagerID = manager.Id
                        };

                        var columns = row.Split('\t');
                        stock.Client = columns[0];
                        stock.GoodsID = int.Parse(columns[1]);
                        stock.Cost = decimal.Parse(columns[2]);
                        stock.Date = DateTime.ParseExact(columns[3], "ddMMyyyy", CultureInfo.InvariantCulture);

                        stocks.Add(stock);
                    }

                    return stocks;
                }
                catch (InvalidOperationException ioe)
                {
                    throw ioe;
                }
            }
            else
                throw new FileNotFoundException("File with name {0} does not exist.", path);
        }

        public static string AppendToFileName(string path, string append)
        {
            var dir = Path.GetDirectoryName(path);
            var oldName = new StringBuilder(Path.GetFileNameWithoutExtension(path));
            var ext = new StringBuilder(Path.GetExtension(path));

            return Path.Combine(dir, oldName.Append(append).Append(ext).ToString());
        }

        public static int ParseFileName(string fileName)
        {

            var stockInfo = fileName.Split('_');
            if (stockInfo.Length > 2 && stockInfo[2] == "done")
                throw new ApplicationException("File had been alreade processed\n" + fileName);
            try
            {
                int id = int.Parse(stockInfo[0]);
                //DataBase.Manager manager = DataBase.DBFunctionality.Find<DataBase.Manager>(id);
                return id;
            }
            catch { throw new FormatException(string.Format("FileName [{0}] have invalid format", fileName)); }
        }
    }
}
