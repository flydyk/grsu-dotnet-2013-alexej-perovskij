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
            pt.AddPassengerWagon(WagonComfortClass.Cheap, 30);
            pt.AddLuggageWagon();

            Random rnd = new Random();
            for (int i = 0; i < 100; i++)
            {
                pt.AddPassenger(100 + rnd.Next(pt.PassWagonCount), i % 2);

            }
            pt.SortWagonsByComfort();
            Console.WriteLine("=================Sorted by COMFORT====================");
            PrintInfoAboutTrain(pt);
            Console.WriteLine();
            Console.WriteLine("=================Sorted by Passenger count============");
            pt.SortWagonsByPassCount();
            PrintInfoAboutTrain(pt);
            int lowBound=10,highBound=20;
            Console.WriteLine();
            Console.WriteLine("============Selected with bounds [{0}, {1}]===========", lowBound, highBound);
            PrintInfoAboutTrain(new PassengerTrain(GetWagonsByCount(pt, lowBound, highBound), new Locomotive()));
            Console.ReadLine();
        }

        static void PrintInfoAboutTrain(PassengerTrain train)
        {
            foreach (var PWagon in train.GetPassWagons())
            {
                Console.WriteLine(PWagon.ToString());
                for (int i = 0; i < PWagon.Capacity/2; i++)
                {
                    Console.Write("place {0}: {1}\t", i, PWagon[i] ? "+" : "-");
                }
                Console.WriteLine();
                for (int i = PWagon.Capacity/2; i < PWagon.Capacity; i++)
                {
                    Console.Write("place {0}: {1}\t", i, PWagon[i] ? "+" : "-");
                }
                Console.WriteLine();
            }

            foreach (var LWagon in train.GetLuggageWagons())
            {
                Console.WriteLine(LWagon.ToString());
            }            
        }

        static List<PassengerWagon> GetWagonsByCount(PassengerTrain train, int lowBound, int highBound)
        {
            return (from wagon in train.GetPassWagons()
                    where wagon.PassengerCount >= lowBound && wagon.PassengerCount <= highBound
                    select wagon).ToList<PassengerWagon>();
        }
    }
}
