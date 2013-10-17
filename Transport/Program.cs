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
            InitializeTrain(pt);

            //SORTING WAGONS
            pt.SortWagonsByComfort();
            Console.WriteLine("=================Sorted by COMFORT====================");
            PrintInfoAboutTrain(pt);
            Console.WriteLine();
            Console.WriteLine("=================Sorted by Passenger count============");
            pt.SortWagonsByPassCount();
            PrintInfoAboutTrain(pt);


            //SELECTING WAGONS
            int lowBound = 10, highBound = 20;
            Console.WriteLine();
            Console.WriteLine("============Selected with bounds [{0}, {1}]===========", lowBound, highBound);
            PrintInfoAboutTrain(new PassengerTrain(GetWagonsByCount(pt, lowBound, highBound), new Locomotive()));
            


            //COUNTING PASSENGERS AND LUGGAGE IN THE TRAIN
            Console.WriteLine();
            Console.WriteLine("===========Count of passengers and luggage============");
            int countOfPassengers = pt.GetPassWagons().Sum(w => w.PassengerCount);
            int countOfLuggage = pt.GetLuggageWagons().Sum(w => w.Load);
            Console.WriteLine("Count of passengers: {0}\nCount of luggage: {1}",
                                  countOfPassengers, countOfLuggage);

            Console.WriteLine("\n\n");
            
            //TESTING WORK OF CLASS METHODS
            bool error = true;
            TestClassMethods(fw, pw, loco, pt, error);

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



        private static void InitializeTrain(PassengerTrain pt)
        {

            pt.AddLuggageWagon();
            pt.AddPassengerWagon(WagonComfortClass.Business, 30);
            pt.AddPassengerWagon(WagonComfortClass.Cheap, 30);
            pt.AddLuggageWagon();

            Random rnd = new Random();
            for (int i = 0; i < 100; i++)
            {
                pt.AddPassenger(100 + rnd.Next(pt.PassWagonCount), i % 2);

            }
        }
        
        private static void TestClassMethods(FreightWagon fw, PassengerWagon pw, Locomotive loco, PassengerTrain pt, bool error)
        {
            fw.Couple(pw, Coupler.Front);
            if (error)
                pw.Couple(loco, Coupler.Back);
            else
                pw.Couple(loco, Coupler.Front);

            fw.Open();
            pw.Close();
            loco.Run();
            pt.Stop();
        }
    }
}
