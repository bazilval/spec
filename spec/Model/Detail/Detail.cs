using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spec.Model
{
    public class Detail
    {
        private IDetailType type;
        private Steel steel;
        private int count;

        public Element ParentElement { get; }
        public IDetailType Type { get => type; set { type = value; OnChange(); } }
        public Steel Steel { get => steel; set { steel = value; OnChange(); } }
        public Diameter Diameter { get => Type.Diameter; set { Type.Diameter = value; OnChange(); } }
        public int Count { get => count; set { count = value; OnChange(); } }
        public int Length { get => (int)Type.Length; set { Type.Length = value; OnChange(); } }
        public double Mass { get => Math.Round(Diameter.Mass * Length / 1000, 2); }
        public double TotalMass { get => IsTotal ? Mass : Mass * Count; }
        public string Description { get; set; }
        public bool IsSpecial { get => Type.IsSpecial; }
        public bool IsTotal { get => Type.IsTotal; }
        public bool IsAverage { get => Type.IsAverage; }
        public string Mark { get; set; }
        public bool IsReady => (TotalMass != 0);

        public Detail(Element element, string mark, IDetailType type, Steel steel, int count)
        {
            ParentElement = element;
            Mark = mark;
            Type = type;
            Steel = steel;
            Count = count;
        }
        public Detail(Element element, string mark, Steel steel, Diameter diameter, int count, double len) : this(element, mark, new RegularType(diameter, len), steel, count) { }
        public Detail(Element element, string mark, Steel steel, Diameter diameter, double len) : this(element, mark, new TotalType(diameter, len), steel, 1) { }
        public Detail(Element element, string mark, Steel steel, Diameter diameter, int count, double minLen, double maxLen) : this(element, mark, new AverageType(diameter, minLen, maxLen), steel, count) { }
        void OnChange()
        {
            ParentElement.OnChange();
        }
    }
}
