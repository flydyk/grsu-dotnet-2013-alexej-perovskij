using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATS
{
    class Program
    {
        static void Main(string[] args)
        {
            ATS ats = new ATS(10, "Owner");
            ats.AddStand();
            Random rand_time = new Random();
            List<Subscriber> subscribers = GenerateSubscribers(ats);

            /*
            // 1
            subscribers[0].Call(subscribers[1].Telephone.TelephoneNumber);
            Sleep(rand_time);
            //subscribers[1].Abort();
            // 2
            subscribers[0].Call(subscribers[1].Telephone.TelephoneNumber);
            Sleep(rand_time);
            subscribers[2].Abort();
            // 3
            subscribers[2].Call(subscribers[1].Telephone.TelephoneNumber);
            Sleep(rand_time);
            subscribers[1].Abort();
            subscribers[2].Abort();
            // 4
            subscribers[2].Call(subscribers[1].Telephone.TelephoneNumber);
            Sleep(rand_time);
            subscribers[1].Abort();
            subscribers[2].Abort();
            */
            
            RandomConversations(ats,subscribers, 100);
            
            ShowStatForEach(subscribers);

            Console.ReadLine();
        }

        static List<Subscriber> GenerateSubscribers(ATS ats)
        {
            Subscriber s1 = new Subscriber(0, "Alexey", "Grodno");
            Subscriber s2 = new Subscriber(2, "George", "New-York");
            Subscriber s3 = new Subscriber(3, "Paolo", "Brazilia");
            Subscriber s4 = new Subscriber(4, "Elizabet", "Britain");
            Subscriber s5 = new Subscriber(5, "Franz", "Paris");
            Subscriber s6 = new Subscriber(6, "Ivan", "Moscow");
            Subscriber s7 = new Subscriber(7, "Daniela", "Spain");
            Subscriber s8 = new Subscriber(8, "Ksenia", "Minsk");
            List<Subscriber> subs = new List<Subscriber>() { s1, s2, s3, s4, s5, s6, s7, s8 };

            ats.SignContract(s1, Tarrifs.Cheap);
            ats.SignContract(s2, Tarrifs.Middle);
            ats.SignContract(s3, Tarrifs.Expensive);
            ats.SignContract(s4, Tarrifs.Cheap);
            ats.SignContract(s5, Tarrifs.Cheap);
            ats.SignContract(s6, Tarrifs.Middle);
            ats.SignContract(s7, Tarrifs.Middle);
            ats.SignContract(s8, Tarrifs.Cheap);

            foreach (var sub in subs)
            {
                sub.Telephone.ConnectTo(ats[sub.Contract.StandID][sub.Contract.PortID]);
                sub.ListenBell += sub_ListenBell;
            }

            return subs;
        }

        static void sub_ListenBell(object sender, BellEventArgs e)
        {
            Subscriber s = sender as Subscriber;
            Console.Write("{0} is calling to you [ {1} ]\nRecieve call? (y/n)",
                    e.Caller, s.Name);
            //string todo = Console.ReadLine();
            //if (todo == "y")
                s.AcceptCall();
            //else
            //    s.Abort();
        }

        static void ShowStatForEach(List<Subscriber> subscribers)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=====================================\n============= STATISTIC =============");

            foreach (var subscriber in subscribers)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("###### Statistic of: {0}. Tariff: {1}",
                    subscriber.Name, subscriber.Contract.Tarrif.TarrifType);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Should pay: {0} $", subscriber.Contract.ToPay);

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                foreach (var session in subscriber.GetSessions())
                {
                    Console.WriteLine(session.ToString());
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("###### Statistic filtered by Cost [0,50]");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                foreach (var session in subscriber.SessionsFilteredByCost(0, 50))
                {
                    Console.WriteLine(session.ToString());
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("###### Total Cost: {0}", subscriber.GetTotalCost());
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("=====================================");
            }
        }

        static void Sleep(Random rand_time)
        {
            Thread.Sleep(rand_time.Next(10, 80));
        }

        static void RandomConversations(ATS ats, List<Subscriber> subscribers, int count)
        {
            Random r = new Random();
            for (int i = 0; i < count; i++)
            {
                int sub1 = r.Next(subscribers.Count);
                int sub2 = r.Next(subscribers.Count);

                // subscribers try to pay services to be able call
                if (r.Next(10) == 0)
                {
                    ats.Pay(subscribers[sub1].Telephone.TelephoneNumber, subscribers[sub1].Contract.ToPay);
                    ats.Pay(subscribers[sub2].Telephone.TelephoneNumber, subscribers[sub2].Contract.ToPay);
                }

                subscribers[sub1].Call(subscribers[sub2].Telephone.TelephoneNumber);
                Sleep(r);
                if (sub1 == sub2) subscribers[sub1].Abort();
                else
                {
                    if (r.Next(2) == 0)
                    subscribers[sub2].Abort();
                    else 
                    subscribers[sub1].Abort();
                }
            }
        }

        
    }
}
