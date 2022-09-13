using System;
using System.Collections.Generic;

namespace spec.Model
{
    public class Table
    {
        public static string[] EmptyRow = new string[6];
        public List<string[]> Element { get; set; }
        public List<string[]> Steel { get; set; }

        public Table()
        {
            Element = new List<string[]>();
            Steel = new List<string[]>();
        }
        public void AddElementRow(string[] row)
        {
            Element.Add(row);
        }
        public void AddElementEmptyRow()
        {
            Element.Add(EmptyRow);
        }

    }
}