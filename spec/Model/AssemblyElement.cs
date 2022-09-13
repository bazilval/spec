using System.Collections.Generic;

namespace spec.Model
{
    public class AssemblyElement
    {
        public string Name { get; set; }
        public List<Element> Elements { get; set; }
        public AssemblyTable Table { get; set; }
        public AssemblyTable SteelTable { get; set; }
    }
}