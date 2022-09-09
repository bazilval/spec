using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spec.Model
{
    internal class Detail
    {
        public IDetailType Type { get; set; }
        public Steel Steel { get; set; }
        public int Count { get; set; }
        public Diameter Diameter { get; set; }

        public Detail(Steel steel, Diameter diameter, int count, double len)
        {
            Steel = steel;
            Diameter = diameter;
            Count = count;
            Type = new RegularType(len);
        }
        public Detail(Steel steel, Diameter diameter, double len)
        {
            Steel = steel;
            Diameter = diameter;
            Count = 1;
            Type = new TotalType(len);
        }
        public Detail(IDetailType type, Steel steel, Diameter diameter, int count)
        {
            Steel = steel;
            Diameter = diameter;
            Count = count;

        }

        public double GetLength()
        {
            return Type.GetLength();
        }
        public double GetDetailMass()
        {
            return Math.Round(Diameter.Mass * GetLength()/1000, 2);
        }
        public double GetTotalMass()
        {
            if (isTotal)
            {
                return GetDetailMass();
            }

            return GetDetailMass() * Count;
        }
    }

    
}
