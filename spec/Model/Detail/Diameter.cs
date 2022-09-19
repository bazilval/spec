namespace spec.Model
{
    public class Diameter
    {
        public string Name { get; }
        public int Value { get; }
        public double Mass { get; }
        public double BendRadius { get; }
        public Diameter(string name, int value, double mass)
        {
            Name = name;
            Value = value;
            Mass = mass;
            BendRadius = value * 5;
        }
        public override string ToString() => $"Ø{Value}";
    }
}