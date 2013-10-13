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
            Locomotive loco = new Locomotive();
            PassengerTrain pt = new PassengerTrain();
            foreach (var item in pt.GetPassWagons())
            {
                Console.WriteLine(item.ToString());
            }
            pt.RemovePassengerWagon(3);
            
            foreach (var item in pt.GetPassWagons())
            {
                Console.WriteLine(item.ToString());
            }
            Console.ReadLine();
        }
    }
}
