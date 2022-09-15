using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using spec.Model;

namespace SpecTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Diameter d32 = new Diameter("d32", 32, 6.313);
            Diameter d25 = new Diameter("d25", 25, 3.853);
            Diameter d16 = new Diameter("d16", 16, 1.578);
            Diameter d8 = new Diameter("d8", 8, 0.395);

            Steel A500 = new Steel("A500C", "ГОСТ 34028-2016");
            Steel A400 = new Steel("A400", "ГОСТ 34028-2016");
            Steel A240 = new Steel("A240", "ГОСТ 34028-2016");

            var project = new Project();

            var wall = ElementFactory.CreateWall("СТм1", 1, 500, 5000);
            project.AddElement(wall);
            Console.WriteLine(wall.Type.GetDescripton());
            int a = 1;

            for (int i = 1; i < 5; i++)
            {
                var detail = DetailFactory.CreateRegularDetail(wall, (a+i).ToString(), A500, d16, i * 5, i * 400);
                wall.Add(detail);
            }

            for (int i = 5; i < 10; i++)
            {
                var detail = DetailFactory.CreateTotalDetail(wall, (a + i).ToString(), A500, d32, i * 40000);
                wall.Add(detail);
            }
            for (int i = 10; i < 13; i++)
            {
                var detail = DetailFactory.CreateAverageDetail(wall, (a + i).ToString(), A400, d16, i * 5, i * 400, i * 405);
                wall.Add(detail);
            }
            for (int i = 13; i < 15; i++)
            {
                var detail = DetailFactory.CreateRegularDetail(wall, (a + i).ToString(), A240, d8, i * 5, i * 40);
                wall.Add(detail);
            }

            var wall2 = ElementFactory.CreateWall("СТм2", 1, 250, 4500);
            project.AddElement(wall2);
            Console.WriteLine(wall2.Type.GetDescripton());

            wall2.Add(DetailFactory.CreateRegularDetail(wall2, "1", A400, d25, 10, 1000));
            wall2.Add(DetailFactory.CreateTotalDetail(wall2, "2", A400, d32, 150000));
            List<Element> elements = new List<Element> { wall, wall2 };
            for (int i = 3; i < 40; i++)
            {
                elements.Add(project.DuplicateElement(wall2,$"СТм{i}"));
            }

            var assembly = new AssemblyElement("Сборка 1", elements);
            project.AddAssembly(assembly);
            TableFactory.CreateTables(assembly);

            var table1 = assembly.Table.Element;
            var table2 = assembly.Table.Steel;

            Print(table1);
            Console.WriteLine();
            Print(table2);

            Console.ReadKey();

        }
        private static void Print(List<string[]> table)
        {
            foreach (var row in table)
            {
                foreach (var col in row)
                {
                    Console.Write(col);
                    Console.Write("\t\t");
                }
                Console.WriteLine("");
            }
        }
    }
}
