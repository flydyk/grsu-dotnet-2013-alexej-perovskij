using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport
{
    public class PassengerTrain
    {
        List<PassengerWagon> pWagons = null;
        List<FreightWagon> fWagons = null;
        Locomotive loco = null;

        public PassengerTrain(List<PassengerWagon> pw,Locomotive loco)
        {
            pWagons = pw;
            this.loco = loco;
        }
        public PassengerTrain(int pwCount,LocomotiveType locoType)
        {
            pWagons = new List<PassengerWagon>();
            for (int i = 1; i <= pwCount; i++)
            {
                pWagons.Add(
                    new PassengerWagon(WagonComfortClass.Middle) { ID = i }
                    );
            }
            loco = new Locomotive() { ID = 0 };
        }
        public PassengerTrain()
            : this(4, LocomotiveType.ElectricLoco)
        {

        }

        public void AddLuggageWagon()
        {
            if (fWagons == null) fWagons = new List<FreightWagon>();
            fWagons.Add(new FreightWagon(CargoType.Luggage));
        }

        public void RemoveLuggageWagon()
        {
            if (fWagons == null || fWagons.Count == 0) return;
            fWagons.RemoveAt(fWagons.Count - 1);
        }

        public void AddPassengerWagon(int id, WagonComfortClass comfort = WagonComfortClass.Middle, int capacity = 50)
        {
            pWagons.Add(new PassengerWagon(comfort, capacity) { ID = id });
        }

        public void RemovePassengerWagon(int id)
        {
            if (pWagons.Count == 0 || pWagons == null) return;

            pWagons.RemoveAt(pWagons.FindIndex(
                (PassengerWagon p) => { return p.ID == id; }
                ));
        }
        public PassengerWagon[] GetPassWagons()
        {
            return pWagons.ToArray();
        }


        public void Run() { loco.Run(); }
        public void Stop() { loco.Stop(); }
    }
}
