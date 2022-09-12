using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spec.Model
{
    internal static class ElementFactory
    {
        public static Element CreateUnknown(string name, int count)
        {
            ElementType unknown = new UnknownType(name);
            return new Element(name, unknown, count);
        }
        public static Element CreateWall(string name, int count, int thickness, int? height)
        {
            ElementType wall = new WallType(name, thickness, height);
            return new Element(name, wall, count);
        }
        public static Element CreateSlab(string name, int count, int thickness, double? heightMark)
        {
            ElementType slab = new SlabType(name, thickness, heightMark);
            return new Element(name, slab, count);
        }

    }
}
