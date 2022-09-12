using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spec.Model
{
    internal class Detail
    {
        private IDetailType type;
        private Steel steel;
        private int count;
        private string mark;

        public IDetailType Type { get => type; set { type = value; OnChange(); } }
        public Steel Steel { get => steel; set { steel = value; OnChange(); } }
        public int Count { get => count; set { count = value; ; OnChange(); } }
        public string Description { get; set; }
        public string Mark
        {
            get => mark;
            set {
                if (!ParentElement.IsMarkExist(value))
                {
                    mark = value;
                    OnChange();
                }
                else
                {
                    throw new ArgumentException($"Марка \"{value}\" уже существует!");
                }
            }
        }
        public Element ParentElement { get; }

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
        void OnChange()
        {
            ParentElement.OnChange();
        }

    }

}
