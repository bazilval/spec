using System;
using System.Collections.Generic;
using System.Linq;

namespace spec.Model
{
    public class AssemblyElement
    {
        public string Name { get; set; }
        public List<Element> Elements { get; set; }
        public Table Table { get; set; }
        public AssemblyElement(string name, List<Element> elements)
        {
            Name = name;
            Elements = elements;
            Table = null;
        }
        internal bool IsReady()
        {
            return Elements.All(element => element.IsReady());
        }
    }
}