namespace spec.Model
{
    public class Embedded
    {
        private IEmbeddedType type;
        private Steel steel;
        private int count;
        public Element ParentElement { get; }
        public IEmbeddedType Type { get => type; set { type = value; OnChange(); } }
        public Steel Steel { get => steel; set { steel = value; OnChange(); } }
        public int Count { get => count; set { count = value; OnChange(); } }
        public double Mass { get => Type.Mass; }
        public double TotalMass { get => Mass * Count; }
        public string Mark { get; set; }
        public string Description { get; set; }
        public bool IsReady => (TotalMass != 0);
        public Embedded(Element element, string mark, IEmbeddedType type, Steel steel, int count)
        {
            ParentElement = element;
            Mark = mark;
            Type = type;
            Steel = steel;
            Count = count;
        }
        void OnChange()
        {
            ParentElement.OnChange();
        }
    }

    
}