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
        public Element(string name, ElementType type, List<Detail> details, List<Embedded> embeddeds, List<Material> materials)
        {
            Name = name;
            Type = type;
            Table = null;
            SteelTable = null;
            Details = details;
            Embeddeds = embeddeds;
            Materials = materials;
        }
        public Element(string name, ElementType type): this(name, type, new List<Detail>(), new List<Embedded>(), new List<Material>()) { }
        public Element(string name, ElementType type, List<Detail> details) : this(name, type, details, new List<Embedded>(), new List<Material>()) { }
        public Element(string name, ElementType type, List<Detail> details, List<Embedded> embeddeds) : this(name, type, details, embeddeds, new List<Material>()) { }
        public Element(string name, ElementType type, List<Detail> details, List<Material> materials) : this(name, type, details, new List<Embedded>(), materials) { }
        public Element(string name, ElementType type, List<Embedded> embeddeds) : this(name, type, new List<Detail>(), embeddeds, new List<Material>()) { }
        public Element(string name, ElementType type, List<Embedded> embeddeds, List<Material> materials) : this(name, type, new List<Detail>(), embeddeds, materials) { }
        public Element(string name, ElementType type, List<Material> materials) : this(name, type, new List<Detail>(), new List<Embedded>(), materials) { }

        public void Add(Detail detail)
        {
            if (detail == null) return;
            if (Marks.Add(detail.Mark))
            {
                Details.Add(detail);
            }
            else
            {
                throw new ArgumentException($"Марка \"{detail.Mark}\" уже существует!");
            }
        }




    }
}
