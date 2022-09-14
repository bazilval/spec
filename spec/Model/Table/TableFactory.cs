using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spec.Model
{
    public class TableFactory
    {
        public static void CreateTables(Element element)
        {
            if (!element.IsReady())
            {
                return;
            }
            element.Table = new Table();

            ElementTableFactory.CreateTable(element);
            SteelTableFactory.CreateTable(element);
        }
        private class ElementTableFactory
        {
            public static string DiameterHeader = "Пруток МД-<DIAM>-<STEEL> <GOST>";
            public static string DetailsHeader = "_Детали";
            public static string EmbeddedsHeader = "_Металлоизделия";
            public static string MaterialsHeader = "_Материалы";
            public static void CreateTable(Element element)
            {
                var table = element.Table;

                AddElementHeader(table, $"_{element.Name}");
                //WriteEmbeddeds(element);
                table.AddElementEmptyRow();
                WriteDetails(element);
                //WriteMaterials(element);
            }
            private static void WriteDetails(Element element)
            {
                var table = element.Table;
                var comparer = new MarkComparer();
                var groupedDetails = element.Details.GroupBy(detail => new { Diameter = detail.Diameter, Steel = detail.Steel },
                                                            detail => detail,
                                                            (group, details) => new { Group = group, Details = details.OrderBy(detail => detail.Mark, comparer) })
                                                    .OrderByDescending(gr => gr.Group.Steel.Name).ThenByDescending(gr => gr.Group.Diameter.Value);
                AddElementHeader(table, DetailsHeader);
                foreach (var group in groupedDetails)
                {
                    var diam = group.Group.Diameter;
                    var steel = group.Group.Steel;
                    var header = (DiameterHeader.Replace("<DIAM>", diam.Value.ToString())
                                                .Replace("<STEEL>", steel.Name)
                                                .Replace("<GOST>", steel.Gost));
                    AddElementHeader(table, header);
                    foreach (var detail in group.Details)
                    {
                        AddDetail(table, detail);
                    }
                }
            }
            private static void AddDetail(Table table, Detail detail)
            {
                var mark = detail.IsSpecial ? $"{detail.Mark}*" : detail.Mark;
                var len = detail.Length;
                var count = (int)detail.Count;
                var mass = detail.Mass;

                if (detail.Type.IsTotal)
                {
                    AddTotalDetail(table, mark, len, mass);
                }
                else if (detail.Type.IsAverage)
                {
                    AddAverageDetail(table, mark, len, count, mass);
                }
                else
                {
                    AddRegularDetail(table, mark, len, count, mass);
                }
            }

            private static void AddElementHeader(Table table, string header)
            {
                table.AddElementRow(new string[] { "", "", header, "", "", "" });
            }
            private static void AddRegularDetail(Table table, string mark, int len, int count, double mass)
            {
                table.AddElementRow(new string[] { mark, "", $"L = {len}", count.ToString(), mass.ToString(), "" });
            }
            private static void AddAverageDetail(Table table, string mark, int len, int count, double mass)
            {
                table.AddElementRow(new string[] { mark, "", $"Lср. = {len}", count.ToString(), mass.ToString(), "" });
            }
            private static void AddTotalDetail(Table table, string mark, int len, double mass)
            {
                table.AddElementRow(new string[] { mark, "", $"Lобщ. = {len}", "-", "-", $"{mass}кг" });
            }
        }
        private class SteelTableFactory
        {
            public static void CreateTable(Element element)
            {
                var table = element.Table;
                var steelGroups = GetSteelGroups(element);
                //if (element.Embeddeds.Count != 0)
                //{
                //    var embGroups = GetEmbGroups(element);
                //}
                AddHeaders(table, steelGroups);
                AddData(element, table, steelGroups);
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
                for (int i = 0; i < steelGroups.Count; i++)
                {
                    var group = steelGroups[i];
                    var diameters = group.diameterGroups;
                    table.Steel[3][currentCol] = group.steel.Name;
                    table.Steel[4][currentCol] = group.steel.Gost;
                    if (diameters.Count == 1)
                    {
                        table.Steel[5][currentCol] = $"Ø{diameters[0].diameter.Value}";
                        currentCol += 2;
                    }
                    else
                    {
                        for (int j = 0; j < diameters.Count; j++)
                        {
                            var diameter = diameters[j].diameter;
                            table.Steel[5][currentCol] = $"Ø{diameter.Value}";
                            currentCol++;
                        }
                    }
                    table.Steel[5][currentCol] = "Итого";
                    currentCol++;
                }
                table.Steel[2][currentCol] = "Всего";
            }
            private static void AddData(Element element, Table table, List<SteelGroup> steelGroups)
            {
                int colCount = table.Steel[0].Length;
                table.AddSteelRows(colCount, 1);
                var currentRow = table.Steel.Count - 1;

                table.Steel[currentRow][0] = element.Name;

                int currentCol = 1;
                WriteDetails(table, steelGroups, currentRow, ref currentCol);
                //WriteEmbeddeds(table, steelGroups, currentRow, ref currentCol);
            }

            private static void WriteDetails(Table table, List<SteelGroup> steelGroups, int currentRow, ref int currentCol)
            {
                for (int i = 0; i < steelGroups.Count; i++)
                {
                    var group = steelGroups[i];
                    var diameters = group.diameterGroups;
                    if (diameters.Count == 1)
                    {
                        table.Steel[currentRow][currentCol] = $"{diameters[0].mass}";
                        currentCol += 2;
                    }
                    else
                    {
                        for (int j = 0; j < diameters.Count; j++)
                        {
                            var mass = diameters[j].mass;
                            table.Steel[currentRow][currentCol] = $"{mass}";
                            currentCol++;
                        }
                    }
                    table.Steel[currentRow][currentCol] = diameters.Sum(x => x.mass).ToString();
                    currentCol++;
                }
                table.Steel[currentRow][currentCol] = steelGroups.Sum(x => x.diameterGroups.Sum(y => y.mass)).ToString();
            }

            private static List<SteelGroup> GetSteelGroups(Element element)
            {
                var details = element.Details;
                List<SteelGroup> groups = details.GroupBy(det => new { diameter = det.Diameter, steel = det.Steel },
                                                          det => det,
                                                          (d, det) => new DiameterGroup(d.steel, d.diameter, det.Sum(x => x.TotalMass)))
                                                 .GroupBy(x => x.steel,
                                                          x => x,
                                                          (st, x) => new SteelGroup(st, x.OrderByDescending(y => y.diameter.Value).ToList()))
                                                 .OrderBy(gr => gr.steel.Name)
                                                 .ToList();
                return groups;
            }

            private class DiameterGroup
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
            }
            private class SteelGroup
            {
                public Steel steel;
                public List<DiameterGroup> diameterGroups;

                public SteelGroup(Steel steel, List<DiameterGroup> diameterGroups)
                {
                    this.steel = steel;
                    this.diameterGroups = diameterGroups;
                }
            }
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
