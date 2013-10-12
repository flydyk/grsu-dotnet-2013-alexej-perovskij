using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport
{
    public abstract class RollingStock
    {
        public int WheelsCount { get; set; }
        public int Weight { get; set; }
        public int ID { get; set; }

        /// <summary>
        /// Инициализирует новый объект класса RollingStock c пармаетрами по умолчанию
        /// </summary>
        public RollingStock() :
            this(4, 20000, 1000001)
        {

        }
        /// <summary>
        /// Инициализирует новый объект класса RollingStock
        /// </summary>
        /// <param name="wheelsCount">Колличество колес у единицы подвижного состава</param>
        /// <param name="weight">Масса в килограммах</param>
        /// <param name="id">Идентификационный номер</param>
        public RollingStock(int wheelsCount, int weight, int id)
        {
            WheelsCount = wheelsCount;
            Weight = weight;
            ID = id;
        }
        public override string ToString()
        {
            return string.Format(
                "[ID: {0}, WheelCount: {1}, Weight: {2}",
                ID, WheelsCount, Weight);
        }
    }
}
