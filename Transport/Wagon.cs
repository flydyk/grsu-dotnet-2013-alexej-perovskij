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

        /// <summary>
        /// Вместимость вагона
        /// </summary>
        public int Capacity
        {
            get { return capacity; }
        }
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
        public Wagon(int capacity) :
            base()
        {
            this.capacity = capacity;
        }
    }
}
