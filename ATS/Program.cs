using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATS
{
    class Program
    {
        /*
         * Config file should have this
         * 
        <appSettings>
            <add key="Name" value="Alexey,George,Paolo,Elizabet,Franz,Ivan,Daniela,Ksenia" />
            <add key="City" value="Grodno,New-York,Brazilia,London,Paris,Moscow,Spain,Minsk"/>
            <add key="Tarrif" value="Cheap,Middle,Expensive,Cheap,Cheap,Middle,Middle,Cheap"/>
            <add key="conversationsCount" value="100"/>
          </appSettings>
        */
        static void Main(string[] args)
        {
            int conversationsCount = int.Parse(ConfigurationManager.AppSettings["conversationsCount"]);

            ATS ats = new ATS(10, "Owner");
            ats.AddStand();
            List<Subscriber> subscribers = GenerateSubscribers(ats);

            #region fast Little test
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
            #endregion

            RandomConversations(ats, subscribers, conversationsCount);
            
            ShowStatForEach(subscribers);

            Console.ReadLine();
        }

        static List<Subscriber> GenerateSubscribers(ATS ats)
        {
            var keys = ConfigurationManager.AppSettings.AllKeys;
            var names = ConfigurationManager.AppSettings[keys[0]].Split(',');
            var cities = ConfigurationManager.AppSettings[keys[1]].Split(',');
            var tarrifs = ConfigurationManager.AppSettings[keys[2]].Split(',');

            List<Subscriber> subs = new List<Subscriber>();
            for (int i = 0; i < names.Length; i++)
            {
                Subscriber sub = new Subscriber(i, names[i], cities[i]);
                ats.SignContract(sub, (Tarrifs)Enum.Parse(typeof(Tarrifs), tarrifs[i]));
                sub.Telephone.ConnectTo(ats[sub.Contract.StandID][sub.Contract.PortID]);
                sub.ListenBell += sub_ListenBell;

                subs.Add(sub);
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
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine("###### Statistic of: {0}. Tariff: {1}",
                    subscriber.Name, subscriber.Contract.Tarrif.TarrifType);
                Console.ResetColor();
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
                Console.WriteLine("###### Total Cost of conversations: {0}", subscriber.GetTotalCost());
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
