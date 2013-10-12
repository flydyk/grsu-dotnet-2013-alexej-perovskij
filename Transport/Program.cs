using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport
{
    class Program
    {
        static void Main(string[] args)
        {
            FreightWagon fw = new FreightWagon(CargoType.Animals);
            PassengerWagon pw = new PassengerWagon(WagonComfortClass.Business);
            Console.WriteLine(fw.ToString());
            Console.WriteLine(pw.ToString());
            Console.ReadLine();
        }
    }
}
