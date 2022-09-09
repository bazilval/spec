namespace spec.Model
{
    public abstract class IDetailType
    {
        public double Length { get; set; }
        public bool IsSpecial { get; set; }
        public bool IsTotal { get; set; }
        public bool IsAverage { get; set; }
    }

    public class RegularType : IDetailType
    {
        public RegularType(double len)
        {
            Length = len;
            IsSpecial = false;
            IsTotal = false;
            IsAverage = false;
        }

    }
    public class TotalType : IDetailType
    {
        public TotalType(double len)
        {
            Length = len;
            IsSpecial = false;
            IsTotal = true;
            IsAverage = false;
        }
    }

}