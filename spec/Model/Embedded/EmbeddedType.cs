using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spec.Model
{
    public abstract class IEmbeddedType
    {
        public string Gost { get; set; }
        public string Name { get; set; }
        public Steel Steel { get; set; }
        public abstract double Mass { get; }
        public abstract string DescriptionLength { get; }
        public abstract string DescriptionShort { get; }
        public abstract string DescriptionFull { get; }
        protected IEmbeddedType(string gost, string name, Steel steel)
        {
            Gost = gost;
            Name = name;
            Steel = steel;
        }
    }
    public class StripType : IEmbeddedType
    {
        public static double Density = 0.00785;
        public int Thickness { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public override double Mass => Math.Round(Density * Thickness * Width * Length / 1000, 2);
        public override string DescriptionFull => $"{Name} {Thickness}х{Width} {Gost} {Steel}";
        public override string DescriptionShort => $"{Name} {Thickness}х{Width}";
        public override string DescriptionLength => $"L = {Length}";
        public StripType(Steel steel, string gost, int thickness, int width, int length) : base(gost, "Полоса", steel)
        {
            Thickness = thickness;
            Width = width;
            Length = length;
        }
    }
    public class RegularStripType : StripType
    {
        public RegularStripType(Steel steel, int thickness, int width, int length) : base(steel, "ГОСТ 103-2006", thickness, width, length) { }
    }
    public class WideStripType : StripType
    {
        public WideStripType(Steel steel, int thickness, int width, int length) : base(steel, "ГОСТ 82-70", thickness, width, length) { }
    }
}
