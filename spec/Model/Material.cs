namespace spec.Model
{
    public class Material
    {
        public string MaterialType { get; set; }
        public double Value { get; set; }
        public double Square { get; set; }
        public double Mass { get; set; }

        public Material(string name, string description, double value, double square, double mass)
        {
            Value = value;
            Square = square;
            Mass = mass;
        }

    }
}