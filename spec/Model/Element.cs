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
        public Table Table { get; set; }
        public SteelTable SteelTable { get; set; }

        

    }
}
