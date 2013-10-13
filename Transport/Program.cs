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
            
            pt.AddLuggageWagon();
            pt.AddPassengerWagon(WagonComfortClass.Business, 30);
            pt.AddLuggageWagon();

            Random rnd = new Random();
            for (int i = 0; i < 100; i++)
            {
                pt.AddPassenger(100 + rnd.Next(pt.PassWagonCount), i % 2);

            }

            foreach (var item in pt.GetPassWagons())
            {
                Console.WriteLine(item.ToString());
            }
            foreach (var item in pt.GetLuggageWagons())
            {
                Console.WriteLine(item.ToString());
            }
            Console.ReadLine();
        }
    }
}
