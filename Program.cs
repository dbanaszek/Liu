using Liu.Controller;
using Liu.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liu
{
    class Program
    {
        static void Main(string[] args)
        {
            AppController controller = new AppController();
            controller.GetGraph("example.xml");
            controller.WriteWorks();
            controller.CalculateDueTime();
            controller.WriteNewDueTime();
            controller.OrderWorks();
            controller.WriteLateness();
            controller.WriteMaxLateness();
            controller.WriteTime();
            controller.ShopGraph();
            controller.ShowResult();
            Console.WriteLine("\nNaciśnij dowolny klawisz aby zakończyć...");
            Console.ReadLine();
        }
    }
}
