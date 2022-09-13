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
            Diameter d16 = new Diameter("d16", 16, 1.578);
            Diameter d8 = new Diameter("d8", 8, 0.395);

            Steel A500 = new Steel("A500C", "ГОСТ 34028-2016");
            Steel A400 = new Steel("A400", "ГОСТ 34028-2016");
            Steel A240 = new Steel("A240", "ГОСТ 34028-2016");

            var project = new Project();

            var wall = ElementFactory.CreateWall("СТм1", 1, 500, 5000);
            project.AddElement(wall);
            Console.WriteLine(wall.Type.GetDescripton());

            for (int i = 1; i < 5; i++)
            {
                var detail = DetailFactory.CreateRegularDetail(wall, i.ToString(), A500, d16, i * 5, i * 400);
                wall.Add(detail);
            }

            for (int i = 5; i < 1000; i++)
            {
                var detail = DetailFactory.CreateTotalDetail(wall, i.ToString(), A500, d32, i * 40000);
                wall.Add(detail);
            }
            //for (int i = 10; i < 13; i++)
            //{
            //    var detail = DetailFactory.CreateAverageDetail(wall, i.ToString(), A400, d16, i * 5, i * 400, i * 405);
            //    wall.Add(detail);
            //}
            //for (int i = 13; i < 15; i++)
            //{
            //    var detail = DetailFactory.CreateRegularDetail(wall, i.ToString(), A240, d8, i * 5, i * 40);
            //    wall.Add(detail);
            //}

            TableFactory.CreateElementTable(wall);

            var table = wall.Table.Element;

            foreach (var row in table)
            {
                foreach (var col in row)
                {
                    Console.Write(col);
                    Console.Write("\t\t");
                }
                Console.WriteLine("");
            }


            Console.ReadKey();
        }
    }
}
