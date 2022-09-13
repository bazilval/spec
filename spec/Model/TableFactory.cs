using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spec.Model
{
    public class TableFactory
    {
        public static string DiameterHeader = "Пруток МД-<DIAM>-<STEEL> <GOST>";
        public static string DetailsHeader = "_Детали";
        public static string EmbeddedsHeader = "_Металлоизделия";
        public static string MaterialsHeader = "_Материалы";
        public static void CreateElementTable(Element element)
        {
            if (!element.IsReady())
            {
                return;
            }
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
            var groupedDetails = element.Details.GroupBy(detail => new { Diameter = detail.GetDiameter(), Steel = detail.Steel },
                                                        detail => detail,
                                                        (group, details) => new { Group = group, Details = details.OrderBy(detail => detail.Mark,comparer) })
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
            var mark = detail.Type.IsSpecial ? $"{detail.Mark}*" : detail.Mark;
            var len = detail.GetLength();
            var count = (int)detail.Count;
            var mass = detail.GetDetailMass();

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
    public class MarkComparer : IComparer<string>
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
