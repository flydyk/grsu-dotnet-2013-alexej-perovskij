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
            Subscriber s1 = new Subscriber(1, "Alexey", "Grodno");
            Subscriber s2 = new Subscriber(2, "George", "New-York");
            Subscriber s3 = new Subscriber(2, "Paolo", "Brazilia");
            List<Subscriber> subs = new List<Subscriber>() { s1, s2, s3 };
            ats.SignContract(s1, Tarrifs.Cheap);
            ats.SignContract(s2, Tarrifs.Middle);
            ats.SignContract(s3, Tarrifs.Expensive);

            s1.Telephone.ConnectTo(ats[s1.Contract.StandID][s1.Contract.PortID]);
            s2.Telephone.ConnectTo(ats[s2.Contract.StandID][s2.Contract.PortID]);
            s3.Telephone.ConnectTo(ats[s3.Contract.StandID][s3.Contract.PortID]);
            //1
            s1.Call(s2.Telephone.TelephoneNumber);
            Thread.Sleep(2000);
            s2.Abort();
            //2
            s3.Call(s1.Telephone.TelephoneNumber);
            Thread.Sleep(3000);
            s3.Abort();
            //3
            s3.Call(s2.Telephone.TelephoneNumber);
            Thread.Sleep(2000);
            s2.Abort();

            //ats.SignContract
            foreach (var sub in subs)
            {
                Console.WriteLine("Statistic of: {0}", sub.Name);
                foreach (var session in sub.GetSessions())
                {
                    Console.WriteLine(session.ToString());
                }
            }
            Console.ReadLine();
        }
    }
}
