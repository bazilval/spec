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
        public string Description { get; set; }
        public string Mark { get; set; }

        public Detail(string mark, IDetailType type, Steel steel, int count)
        {
            Mark = mark;
            Type = type;
            Steel = steel;
            Count = count;
        }
        public Detail(string mark, Steel steel, Diameter diameter, int count, double len) : this(mark, new RegularType(diameter, len), steel, count) { }
        public Detail(string mark, Steel steel, Diameter diameter, double len) : this(mark, new TotalType(diameter, len), steel, 1) { }
        public Detail(string mark, Steel steel, Diameter diameter, int count, double minLen, double maxLen) : this(mark, new AverageType(diameter, minLen, maxLen), steel, count) { }
        public double GetLength() => Type.Length;
        public Diameter GetDiameter() => Type.Diameter;
        public double GetDetailMass() => Math.Round(GetDiameter().Mass * GetLength() / 1000, 2);
        public double GetTotalMass()
        {
            if (Type.IsTotal)
            {
                return GetDetailMass();
            }
            return GetDetailMass() * Count;
        }
    }

}
