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

            
            //1
            subscribers[0].Call(subscribers[1].Telephone.TelephoneNumber);
            Sleep(rand_time);
            subscribers[0].Abort();
            //2
            subscribers[2].Call(subscribers[0].Telephone.TelephoneNumber);
            Sleep(rand_time);
            subscribers[2].Abort();
            //3
            subscribers[2].Call(subscribers[1].Telephone.TelephoneNumber);
            Sleep(rand_time);
            subscribers[1].Abort();
            subscribers[2].Abort();
            //TestConversation(subscribers);
            //RandomConversations(subscribers, 20);
            
            ShowStatForEach(subscribers);

            Console.ReadLine();
        }

        private static void TestConversation(List<Subscriber> subscribers)
        {
            throw new NotImplementedException();
        }

        static List<Subscriber> GenerateSubscribers(ATS ats)
        {
            Subscriber s1 = new Subscriber(1, "Alexey", "Grodno");
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

            s1.Telephone.ConnectTo(ats[s1.Contract.StandID][s1.Contract.PortID]);
            s2.Telephone.ConnectTo(ats[s2.Contract.StandID][s2.Contract.PortID]);
            s3.Telephone.ConnectTo(ats[s3.Contract.StandID][s3.Contract.PortID]);
            s4.Telephone.ConnectTo(ats[s4.Contract.StandID][s4.Contract.PortID]);
            s5.Telephone.ConnectTo(ats[s5.Contract.StandID][s5.Contract.PortID]);
            s6.Telephone.ConnectTo(ats[s6.Contract.StandID][s6.Contract.PortID]);
            s7.Telephone.ConnectTo(ats[s7.Contract.StandID][s7.Contract.PortID]);
            s8.Telephone.ConnectTo(ats[s8.Contract.StandID][s8.Contract.PortID]);

            return subs;
        }

        static void ShowStatForEach(List<Subscriber> subscribers)
        {
            Console.WriteLine("==================================\n============ STATISTIC ============");

            foreach (var subscriber in subscribers)
            {
                Console.WriteLine("###### Statistic of: {0}", subscriber.Name);
                foreach (var session in subscriber.GetSessions())
                {
                    Console.WriteLine(session.ToString());
                }
            }
        }

        static void Sleep(Random rand_time)
        {
            Thread.Sleep(rand_time.Next(40, 100));
        }

        static void RandomConversations(List<Subscriber> subscribers,int count)
        {
            Random r = new Random();
            for (int i = 0; i < count; i++)
            {
                int sub1 = r.Next(subscribers.Count);
                int sub2 = r.Next(subscribers.Count);
                subscribers[sub1].Call(subscribers[sub2].Telephone.TelephoneNumber);
                Sleep(r);
                if (sub1 == sub2) subscribers[sub1].Abort();
                else
                {
                    if (r.Next(2) == 0)
                        subscribers[sub1].Abort();
                    else 
                    subscribers[sub2].Abort();
                }
            }
        }
    }
}
