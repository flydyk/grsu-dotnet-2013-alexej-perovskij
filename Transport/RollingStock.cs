using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport
{
    public abstract class RollingStock: ICouplable
    {
        public int WheelsCount { get; set; }
        public int Weight { get; set; }
        public int ID { get; set; }

        
        ICouplable frontCoupler;
        ICouplable backCoupler;

        /// <summary>
        /// Инициализирует новый объект класса RollingStock c пармаетрами по умолчанию
        /// </summary>
        public RollingStock() :
            this(4, 20000)
        {

        }
        /// <summary>
        /// Инициализирует новый объект класса RollingStock
        /// </summary>
        /// <param name="wheelsCount">Колличество колес у единицы подвижного состава</param>
        /// <param name="weight">Масса в килограммах</param>
        /// <param name="id">Идентификационный номер</param>
        public RollingStock(int wheelsCount, int weight)
        {
            WheelsCount = wheelsCount;
            Weight = weight;
        }
        public override string ToString()
        {
            return string.Format(
                "[ID: {0}, WheelCount: {1}, Weight: {2}]",
                ID, WheelsCount, Weight);
        }


        public virtual void Couple(ICouplable that, Coupler c)
        {
            switch (c)
            {
                case Coupler.Front:
                    if (FrontCoupler == null && that.BackCoupler == null)
                    {
                        FrontCoupler = that;
                        that.BackCoupler = this;
                        return;
                    }
                    break;
                case Coupler.Back:
                    if (BackCoupler == null && that.FrontCoupler == null)
                    {
                        BackCoupler = that;
                        that.FrontCoupler = this;
                        return;
                    }
                    break;
                default:
                    break;
            }
            throw new CouplingException("Coupler is occupied");
        }

        public virtual void Decouple(Coupler c)
        {
            switch (c)
            {
                case Coupler.Front:
                    FrontCoupler.BackCoupler = null;
                    FrontCoupler = null;
                    break;
                case Coupler.Back:
                    BackCoupler.FrontCoupler = null;
                    BackCoupler = null;
                    break;
                default:
                    break;
            }
        }

        public virtual ICouplable FrontCoupler
        {
            get
            {
                return frontCoupler;
            }
            set
            {
                frontCoupler = value;
            }
        }

        public virtual ICouplable BackCoupler
        {
            get
            {
                return backCoupler;
            }
            set
            {
                backCoupler = value;
            }
        }
    }



    [Serializable]
    public class CouplingException : Exception
    {
        public CouplingException() { }
        public CouplingException(string message) : base(message) { }
        public CouplingException(string message, Exception inner) : base(message, inner) { }
        protected CouplingException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
