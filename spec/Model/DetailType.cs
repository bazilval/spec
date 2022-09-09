using System;

namespace spec.Model
{
    public abstract class IDetailType
    {
        public IDetailType(Diameter diam, double len, bool isSpecial, bool isAverage, bool isTotal)
        {
            Diameter = diam;
            Length = len;
            IsSpecial = isSpecial;
            IsAverage = isAverage;
            IsTotal = isTotal;
        }
        public double Length { get; set; }
        public Diameter Diameter { get; set; }
        public bool IsSpecial { get; set; }
        public bool IsTotal { get; set; }
        public bool IsAverage { get; set; }
    }

    public class RegularType : IDetailType
    {
        public RegularType(Diameter diameter, double len) : base(diameter, len, false, false, false) { }
    }
    public class TotalType : IDetailType
    {
        public TotalType(Diameter diameter, double len) : base(diameter, len, false, false, true) { }
    }
    public class AverageType : IDetailType
    {
        public double MinLength { get; set; }
        public double MaxLength { get; set; }
        public AverageType(Diameter diameter, double minLen, double maxLen) : base(diameter, Math.Round((minLen + maxLen) / 2), true, true, false)
        {
            MinLength = minLen;
            MaxLength = maxLen;
        }
    }
    public class UniversalSpecialType : IDetailType
    {
        public UniversalSpecialType(Diameter diam, double len) : base(diam, len, true, false, false) { }
    }
}