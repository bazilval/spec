using System.Collections.Generic;

namespace spec.Model
{
    public class AssemblyElement
    {
        public string Name { get; set; }
        public List<Element> Elements { get; set; }
        public Table Table { get; set; }
    }
}