using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport
{
    public class PassengerTrain:IRunable
    {
        List<PassengerWagon> pWagons = null;
        List<FreightWagon> fWagons = new List<FreightWagon>();
        Locomotive loco = null;

        public PassengerTrain(List<PassengerWagon> pw, Locomotive loco)
        {
            pWagons = pw;
            this.loco = loco;
        }
        public PassengerTrain(int pwCount, LocomotiveType locoType)
        {
            pWagons = new List<PassengerWagon>();
            for (int i = 0; i < pwCount; i++)
            {
                pWagons.Add(
                    new PassengerWagon(WagonComfortClass.Middle) { ID = i+100 }
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
            fWagons.Add(new FreightWagon(CargoType.Luggage)
                {
                    ID = fWagons.Count + 10
                });
        }

        public void RemoveLuggageWagon()
        {
            if (fWagons == null || fWagons.Count == 0) return;
            fWagons.RemoveAt(fWagons.Count - 1);
        }

        public void AddPassengerWagon(WagonComfortClass comfort = WagonComfortClass.Middle, int capacity = 50)
        {
            pWagons.Add(new PassengerWagon(comfort, capacity) { ID = pWagons.Count + 100 });
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
        public FreightWagon[] GetLuggageWagons()
        {
            return fWagons.ToArray();
        }
        /// <summary>
        /// Добавить пассажира на поезд в вагон с ID = wagonID. По умолчанию пассажир без багажа 
        /// </summary>
        /// <param name="wagonID">ID вагона в поезде</param>
        /// <param name="luggage">Объем багажа в килограммах</param>
        public void AddPassenger(int wagonID, int luggage = 0)
        {
            this[wagonID].AddPassenger();
            if (luggage == 0) return;
            if (fWagons[0].Capacity >= fWagons[0].Load + luggage)
                fWagons[0].LoadWagon(luggage);
            else fWagons[1].LoadWagon(luggage);
        }


        public void SortWagonsByComfort() 
        {
            pWagons.Sort(PassengerWagon.ComfortComparer);
        }
        public void SortWagonsByPassCount()
        {
            pWagons.Sort();
        }

        public void Run()
        {
            loco.Run();
        }

        public void Stop()
        {
            loco.Stop();
        }


        private PassengerWagon this[int ID]
        {
            get
            {
                PassengerWagon pw = null;
                pw = pWagons.Find(p => { return p.ID == ID; });
                if (pw == null) throw new Exception("Invalid passenger's wagon ID number");
                else return pw;
            }
        }

        public int PassWagonCount
        {
            get { return pWagons.Count; }
        }
    }
}
