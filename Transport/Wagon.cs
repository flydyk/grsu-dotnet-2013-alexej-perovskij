using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport
{
    public abstract class Wagon : RollingStock
    {
        readonly int capacity;
        
        ////Сцепки вагонов

        /// <summary>
        /// Инициализирует новый объект класса Wagon c пармаетрами по умолчанию
        /// </summary>
        public Wagon()
            : this(50)
        {
        }
        /// <summary>
        /// Инициализирует новый объект класса Wagon
        /// </summary>
        /// <param name="capacity">Вместимость вагона</param>
        public Wagon(int capacity)
        {
            this.capacity = capacity;
        }


        /// <summary>
        /// Вместимость вагона
        /// </summary>
        public int Capacity
        {
            get { return capacity; }
        }

        public abstract void Open();
        public abstract void Close();

        public override string ToString()
        {
            return string.Format("{0}, Capacity: {1}]", base.ToString().Substring(0, base.ToString().Length - 1), Capacity);
        }

    }

}
