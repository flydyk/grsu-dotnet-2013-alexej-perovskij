using GoodsCollectorService.TODO;
using GoodsCollectorService.DataBase;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace GoodsCollectorService.Controllers
{
    public class StocksController : IDisposable
    {
        DBFunctionality dbfunc = new DBFunctionality();
        private Action<string> tracer;

        public Action<string> Tracer
        {
            get { return tracer; }
            set { tracer = value; }
        }
        public StocksController()
        {
            Tracer = NullTracer.NullTracerMember;
        }
        public void AddStocksFromFile(string path)
        {
            try
            {
                List<Stock> stocks = ExtractStocks(path);
                Tracer("saving");
                dbfunc.AddSaveItems<Stock>(stocks);
                Tracer("saved");
            }
            catch (Exception e)
            {
                Tracer(e.Message);
            }
        }

        private  List<Stock> ExtractStocks(string path)
        {
            try
            {
                int managerId = CSVFileProcessor.ParseFileName(Path.GetFileNameWithoutExtension(path));
                Manager manager = dbfunc.Find<Manager>(managerId);
                if (manager == null)
                    throw new NullReferenceException("No managers with this id in DataBase");

                List<Stock> stocks = CSVFileProcessor.ParseFile(path, manager);

                File.Move(path, CSVFileProcessor.AppendToFileName(path, "_done_" + DateTime.Now.Ticks));

                return stocks;
            }
            catch (Exception e)
            {
                Tracer(e.Message);
                return null;
            }
        }

        public void AddStocksFromDirectory(string folderPath)
        {
            var fileNames = Directory.GetFiles(folderPath, "*.csv", SearchOption.TopDirectoryOnly);
            List<DataBase.Stock> stocks = new List<DataBase.Stock>();
            try
            {
                //Parallel.ForEach(fileNames, (s) =>
                //{
                foreach (var s in fileNames)
                {
                    var _stocks = ExtractStocks(s);
                    if (_stocks != null)
                    {
                        stocks.AddRange(_stocks);
                        Tracer(string.Format("File {0} was successfuly processed", s));
                    }
                }
                //});
                if (stocks.Count != 0)
                {
                    Tracer("saving");
                    dbfunc.AddSaveItems<Stock>(stocks);
                    Tracer("saved");
                }
            }
            catch (Exception e)
            {
                Tracer(e.Message);
            }
        }

        public void Dispose()
        {
            dbfunc.Dispose();
        }
    }
}
