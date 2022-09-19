using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spec.Model
{
    public class EmbeddedFactory
    {
        public static Embedded CreateRegularStrip(Element element, string mark, Steel steel, int count, int thickness, int width, int length)
        {
            var type = new RegularStripType(steel, thickness, width, length);
            return new Embedded(element, mark, type, steel, count);
        }
        public static Embedded CreateWideStrip(Element element, string mark, Steel steel, int count, int thickness, int width, int length)
        {
            var type = new WideStripType(steel, thickness, width, length);
            return new Embedded(element, mark, type, steel, count);
        }
    }
}
