using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spec.Model
{
    public class TableFactory
    {
        public static void CreateTables(AssemblyElement assembly)
        {
            if (!assembly.IsReady())
            {
                return;
            }
            assembly.Table = new Table();

            ElementTableFactory.CreateTable(assembly.Elements, assembly.Table);
            SteelTableFactory.CreateTable(assembly.Elements, assembly.Table);
        }

        public static void CreateTables(Element element)
        {
            if (!element.IsReady())
            {
                return;
            }
            element.Table = new Table();

            ElementTableFactory.CreateTable(element, element.Table);
            SteelTableFactory.CreateTable(element);
        }
        private class ElementTableFactory
        {
            public static string DetailsHeader = "_Детали";
            public static string EmbeddedsHeader = "_Металлоизделия";
            public static string MaterialsHeader = "_Материалы";
            public static void CreateTable(List<Element> elements, Table table)
            {
                foreach (var element in elements)
                {
                    CreateTable(element, table);
                    table.AddElementEmptyRow();
                }
            }
            public static void CreateTable(Element element, Table table)
            {
                AddElementHeader(table, $"_{element.Name}");
                //WriteEmbeddeds(element);
                //table.AddElementEmptyRow();
                WriteDetails(element, table);
                //WriteMaterials(element);
            }
            private static void WriteDetails(Element element, Table table)
            {
                var comparer = new MarkComparer();
                var groupedDetails = element.Details.GroupBy(detail => new DiameterGroup(detail.Steel, detail.Diameter, 0),
                                                            detail => detail,
                                                            (group, details) => new { DiameterGroup = group, Details = details.OrderBy(detail => detail.Mark, comparer) })
                                                    .OrderByDescending(gr => gr.DiameterGroup.steel.Name).ThenByDescending(gr => gr.DiameterGroup.diameter.Value);
                AddElementHeader(table, DetailsHeader);
                foreach (var group in groupedDetails)
                {
                    AddElementHeader(table, group.DiameterGroup.ToString());
                    foreach (var detail in group.Details)
                    {
                        AddDetail(table, detail);
                    }
                }
            }
            private static void AddDetail(Table table, Detail detail)
            {
                var mark = detail.IsSpecial ? $"{detail.Mark}*" : detail.Mark;
                var len = detail.Type.ToString();
                var count = detail.Count.ToString();
                var mass = detail.Mass.ToString();

                if (detail.Type.IsTotal)
                {
                    table.AddElementRow(new string[] { mark, "", len, "-", "-", $"{mass}кг" });
                }
                else
                {
                    table.AddElementRow(new string[] { mark, "", len, count, mass, "" });
                }
            }
            private static void AddElementHeader(Table table, string header)
            {
                table.AddElementRow(new string[] { "", "", header, "", "", "" });
            }
        }
        private class SteelTableFactory
        {
            public static void CreateTable(List<Element> elements, Table table)
            {
                var assembleSteelGroups = GetAssembleSteelGroups(elements);
                //if (element.Embeddeds.Count != 0)
                //{
                //    var embGroups = GetEmbGroups(element);
                //}
                AddHeaders(table, assembleSteelGroups);
                AddData(elements, table, assembleSteelGroups);
            }

            public static void CreateTable(Element element)
            {
                var table = element.Table;
                var steelGroups = GetSteelGroups(element);
                //if (element.Embeddeds.Count != 0)
                //{
                //    var embGroups = GetEmbGroups(element);
                //}
                AddHeaders(table, steelGroups);
                AddData(element, table, steelGroups, null);
            }
            private static void AddHeaders(Table table, List<SteelGroup> steelGroups)
            {
                int colCount = 2;
                colCount += steelGroups.Count();
                foreach (var gr in steelGroups)
                {
                    if (gr.diameterGroups.Count <= 2)
                    {
                        colCount += 3;
                    }
                    else
                    {
                        colCount += gr.diameterGroups.Count + 1;
                    }
                }

                table.AddSteelRows(colCount, 6);
                table.Steel[0][0] = "Ведомость расхода стали";
                table.Steel[1][0] = "Марка элемента";
                table.Steel[1][1] = "Изделия арматурные";
                table.Steel[2][1] = "Арматура класса";

                int currentCol = 1;
                WriteDetails(table, steelGroups, 5, ref currentCol, FillOption.Headers);
            }
            private static void AddData(Element element, Table table, List<SteelGroup> steelGroups, List<SteelGroup> assembleSteelGroups)
            {
                int colCount = table.Steel[0].Length;
                var currentRow = table.AddSteelRows(colCount, 1);
                table.Steel[currentRow][0] = element.Name;
                int currentCol = 1;
                if (assembleSteelGroups == null)
                {
                    WriteDetails(table, steelGroups, currentRow, ref currentCol, FillOption.Data);
                    //WriteEmbeddeds(table, steelGroups, currentRow, ref currentCol);
                }
                else
                {
                    steelGroups = GetSteelGroups(element);
                    WriteDetails(table, assembleSteelGroups, steelGroups, currentRow, ref currentCol);
                }
            }
            private static void AddData(List<Element> elements, Table table, List<SteelGroup> assembleSteelGroups)
            {
                foreach (var element in elements)
                {
                    AddData(element, table, null, assembleSteelGroups);
                }
            }
            private static void WriteDetails(Table table, List<SteelGroup> steelGroups, int currentRow, ref int currentCol, FillOption option)
            {
                foreach (var group in steelGroups)
                {
                    if (option == FillOption.Headers)
                    {
                        table.Steel[3][currentCol] = group.steel.Name;
                        table.Steel[4][currentCol] = group.steel.Gost;
                    }
                    var diameterGroups = group.diameterGroups;

                    var count = 0;
                    foreach (var diameterGroup in diameterGroups)
                    {
                        var diameter = diameterGroup.diameter;
                        table.Steel[currentRow][currentCol] =
                            (option == FillOption.Headers) ? diameter.ToString() : diameterGroup.mass.ToString();
                        count++;
                        currentCol++;
                    }
                    if (count == 1) currentCol++;

                    table.Steel[currentRow][currentCol] =
                                (option == FillOption.Headers) ? "Итого" : diameterGroups.Sum(x => x.mass).ToString();
                    currentCol++;
                }
                var text = (option == FillOption.Headers) ? "Всего" : steelGroups.Sum(x => x.diameterGroups.Sum(y => y.mass)).ToString();
                var row = (option == FillOption.Headers) ? 2 : currentRow;
                table.Steel[row][currentCol] = text;
            }
            enum FillOption { Headers, Data }

            private static void WriteDetails(Table table, List<SteelGroup> assemblySteelGroups, List<SteelGroup> steelGroups, int currentRow, ref int currentCol)
            {
                int groupIndex = 0;
                for (int i = 0; i < assemblySteelGroups.Count; i++)
                {
                    int diameterIndex = 0;
                    var assemblyGroup = assemblySteelGroups[i];
                    var group = steelGroups[groupIndex];

                    var assemblyDiameters = assemblyGroup.diameterGroups;
                    var diameters = group.diameterGroups;
                    if (assemblyDiameters.Count == 1)
                    {
                        table.Steel[currentRow][currentCol] = assemblyDiameters[0].Equals(diameters[0]) ? $"{diameters[0].mass}" : "-";
                        currentCol += 2;
                    }
                    else
                    {
                        for (int j = 0; j < assemblyDiameters.Count; j++)
                        {
                            if (assemblyDiameters[j].Equals(diameters[diameterIndex]))
                            {
                                var mass = diameters[diameterIndex].mass;
                                table.Steel[currentRow][currentCol] = $"{mass}";
                                if (diameterIndex < diameters.Count - 1)
                                {
                                    diameterIndex++;
                                }
                                currentCol++;
                            }
                            else
                            {
                                table.Steel[currentRow][currentCol] = "-";
                                currentCol++;
                            }
                        }
                    }
                    if (assemblyGroup.Equals(group))
                    {
                        table.Steel[currentRow][currentCol] = diameters.Sum(x => x.mass).ToString();
                        if (groupIndex < steelGroups.Count - 1)
                        {
                            groupIndex++;
                        }
                    }
                    else
                    {
                        table.Steel[currentRow][currentCol] = "-";
                    }
                    currentCol++;
                }
                table.Steel[currentRow][currentCol] = steelGroups.Sum(x => x.diameterGroups.Sum(y => y.mass)).ToString();
            }

            private static List<DiameterGroup> GetDiameterGroups(Element element)
            {
                var details = element.Details;
                List<DiameterGroup> groups = details.GroupBy(det => new { diameter = det.Diameter, steel = det.Steel },
                                                         det => det,
                                                         (d, det) => new DiameterGroup(d.steel, d.diameter, det.Sum(x => x.TotalMass))).ToList();
                return groups;
            }
            private static List<SteelGroup> GetSteelGroups(List<DiameterGroup> diameters)
            {
                return diameters.GroupBy(x => x.steel,
                                                          x => x,
                                                          (st, x) => new SteelGroup(st, x.OrderByDescending(y => y.diameter.Value).ToList()))
                                                 .OrderBy(gr => gr.steel.Name)
                                                 .ToList();
            }
            private static List<SteelGroup> GetSteelGroups(Element element)
            {
                List<DiameterGroup> diameters = GetDiameterGroups(element);
                List<SteelGroup> groups = GetSteelGroups(diameters);
                return groups;
            }
            private static List<SteelGroup> GetAssembleSteelGroups(List<Element> elements)
            {
                List<DiameterGroup> allDiameters = new List<DiameterGroup>();
                foreach (Element element in elements)
                {
                    var diameters = GetDiameterGroups(element);
                    allDiameters = allDiameters.Union(diameters).ToList();
                }
                return GetSteelGroups(allDiameters);
            }
            private class SteelGroup : IEquatable<SteelGroup>
            {
                public Steel steel;
                public List<DiameterGroup> diameterGroups;
                public SteelGroup(Steel steel, List<DiameterGroup> diameterGroups)
                {
                    this.steel = steel;
                    this.diameterGroups = diameterGroups;
                }

                public bool Equals(SteelGroup other)
                {
                    if (other is null) return false;
                    return this.steel == other.steel;
                }
                public override int GetHashCode() => (steel).GetHashCode();
            }
        }
        private class DiameterGroup : IEquatable<DiameterGroup>
        {
            public Steel steel;
            public Diameter diameter;
            public double mass;

            public DiameterGroup(Steel steel, Diameter diameter, double mass)
            {
                this.steel = steel;
                this.diameter = diameter;
                this.mass = mass;
            }
            public bool Equals(DiameterGroup other)
            {
                if (other is null) return false;
                return this.steel == other.steel && this.diameter == other.diameter;
            }
            public override int GetHashCode() => (steel, diameter).GetHashCode();
            public override string ToString() => $"Пруток МД-{diameter.Value}-{steel.ToString()}";
        }
        private class MarkComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                if (x.Length < y.Length)
                {
                    return -1;
                }
                if (x.Length > y.Length)
                {
                    return 1;
                }
                return x.CompareTo(y);
            }
        }
    }
}
