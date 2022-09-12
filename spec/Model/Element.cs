using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spec.Model
{
    internal class Element
    {
        public string Name { get; set; }
        public ElementType Type { get; set; }
        public ElementTable Table { get; set; }
        public ElementTable SteelTable { get; set; }
        public List<Detail> Details { get; set; }
        public List<Embedded> Embeddeds { get; set; }
        public List<Material> Materials { get; set; }
        public HashSet<string> Marks { get; set; }
        //public HashSet<Diameter> Diameters { get; set; }
        public int Count { get; set; }
        public Element(string name, ElementType type, int count)
        {
            Name = name;
            Type = type;
            Count = count;
            Table = null;
            SteelTable = null;
            Details = new List<Detail>();
            Embeddeds = new List<Embedded>();
            Materials = new List<Material>();
        }
        public void Add(Detail detail)
        {
            if (detail == null) return;
            if (Marks.Add(detail.Mark))
            {
                Details.Add(detail);
                //Diameters.Add(detail.GetDiameter());
            }
            else
            {
                throw new ArgumentException($"Марка \"{detail.Mark}\" уже существует!");
            }
        }
        public void Remove(Detail detail)
        {
            if (detail == null) return;

            if (Details.Remove(detail))
            {
                Marks.Remove(detail.Mark);
            }
        }
        public void OnChange()
        {

        }
        public bool IsMarkExist(string mark)
        {
            return Marks.Contains(mark);
        }



    }
}
