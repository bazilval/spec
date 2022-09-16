namespace spec.Model
{
    public class Steel
    {
        public string Name { get; }
        public string Gost { get; }
        public Steel(string name, string gost)
        {
            Name = name;
            Gost = gost;
        }
        public override string ToString() => $"{Name} {Gost}";
    }
}