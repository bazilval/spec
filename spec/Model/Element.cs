using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spec.Model
{
    public class Element
    {
        public string Name { get; set; }
        public ElementType Type { get; set; }
        public Table Table { get; set; }
        public List<Detail> Details { get; set; }
        public List<Embedded> Embeddeds { get; set; }
        public List<Material> Materials { get; set; }
        public HashSet<string> Marks { get; set; }
        //public HashSet<Diameter> Diameters { get; set; }
        public int Count { get; set; }
        public Element(string name, ElementType type, int count, List<Detail> details, List<Material> materials, List<Embedded> embeddeds, HashSet<string> marks)
        {
            Name = name;
            Type = type;
            Count = count;
            Table = null;
            Details = details;
            Embeddeds = embeddeds;
            Materials = materials;
            Marks = marks;
        }
        public Element(string name, ElementType type, int count) : this(name, type, count, new List<Detail>(), new List<Material>(), new List<Embedded>(), new HashSet<string>()) { }
        public void Add(Detail detail)
        {
            if (detail == null) return;
            if (Marks.Add(detail.Mark))
            {
                Details.Add(detail);
                //Diameters.Add(detail.GetDiameter());
                OnChange();
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
                OnChange();
            }
        }
        public void OnChange()
        {

        }
        public bool IsMarkExist(string mark)
        {
            return Marks.Contains(mark);
        }
        public bool IsReady()
        {
            return Details.All(x=>x.IsReady());
        }
    }
}
