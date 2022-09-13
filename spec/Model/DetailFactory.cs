using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spec.Model
{
    public static class DetailFactory
    {
        public static Detail CreateRegularDetail(Element element, string mark, Steel steel, Diameter diameter, int count, double len)
        {
            return new Detail(element, mark, steel, diameter, count, len);
        }
        public static Detail CreateTotalDetail(Element element, string mark, Steel steel, Diameter diameter, double len)
        {
            return new Detail(element, mark, steel, diameter, len);
        }
        public static Detail CreateAverageDetail(Element element, string mark, Steel steel, Diameter diameter, int count, double minLen, double maxLen)
        {
            return new Detail(element, mark, steel, diameter, count, minLen, maxLen);
        }
    }
}