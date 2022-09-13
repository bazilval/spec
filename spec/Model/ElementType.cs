namespace spec.Model
{
    public abstract class ElementType
    {
        public string Name { get; }
        public abstract string GetDescripton();

        public ElementType(string name)
        {
            Name = name;
        }

    }
    public class UnknownType : ElementType
    {
        public UnknownType(string name) : base(name) { }
        public override string GetDescripton()
        {
            return $"Устройство монолитного ж/б элемента {Name}";
        }
    }
    public class WallType : ElementType
    {
        public int Thickness { get; set; }
        public int? Height { get; set; }

        public WallType(string name, int thickness, int? height) : base(name)
        {
            Thickness = thickness;
            Height = height;
        }
        public WallType(string name, int thickness) : this(name, thickness, null) { }

        public override string GetDescripton()
        {
            return $"Устройство монолитной ж/б стены {Name} {(Height != null ? $"высотой {Height}," : "")} толщиной {Thickness}";
        }
    }
    public class SlabType : ElementType
    {
        public int Thickness { get; set; }
        public double? HeightMark { get; set; }
        public SlabTypes type { get; set; }

        public SlabType(string name, int thickness, double? heightMark) : base(name)
        {
            Thickness = thickness;
            HeightMark = heightMark;
        }
        public SlabType(string name, int thickness) : this(name, thickness, null) { }

        public override string GetDescripton()
        {
            string descripton = $"Устройство монолитной ж/б плиты {Name}";
            switch (type)
            {
                case SlabTypes.UNKNOWN:
                    goto default;
                case SlabTypes.FLOOR:
                    return descripton +  $" перекрытия {(HeightMark != null ? $"на отм. {HeightMark:d3}, " : "")}толщиной {Thickness}";
                case SlabTypes.ROOF:
                    return descripton + $" покрытия {(HeightMark != null ? $"на отм. {HeightMark:d3}, " : "")}толщиной {Thickness}";
                default:
                    return descripton + $" {(HeightMark != null ? $"на отм. {HeightMark:d3}, " : "")}толщиной {Thickness}";
            }
        }
        public enum SlabTypes { UNKNOWN, FLOOR, ROOF };
    }
    
}