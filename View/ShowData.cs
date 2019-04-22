using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Msagl.GraphViewerGdi;
using Microsoft.Msagl.Drawing;
using System.Threading;
using Liu.Model;

namespace Liu.View
{
    class ShowData
    {

        //form
        Form form = new Form();
        Form formTwo = new Form();
        //view
        TableLayoutPanel table = new TableLayoutPanel();
        GViewer viewer = new GViewer();
        //graph
        Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
        public Model.Graph grap { get; set; }

        List<EdgeHelper> edges = new List<EdgeHelper>();

        private bool CheckEdges(EdgeHelper helper)
        {
            foreach(EdgeHelper i in edges)
            {
                if (i.One == helper.One && i.Two == helper.Two)
                    return false;
            }
            return true;
        }

        private void Add(Work work)
        {
            EdgeHelper edge = new EdgeHelper();
            foreach (Work task in work.succesors)
            {
                edge.One = work.Id;
                edge.Two = task.Id;

                if (CheckEdges(edge))
                {
                    edges.Add(edge);
                    edge = new EdgeHelper();
                }

                Add(task);
            }
        }

        public void AddEdge()
        {
            foreach(EdgeHelper helper in edges){
                graph.AddEdge(helper.One.ToString(), helper.Two.ToString());
            }
        }

        public void CreateGraph()
        {
            graph.Attr.LayerDirection = LayerDirection.LR;

            Add(grap.Works[1]);
            Add(grap.Works[2]);

            AddEdge();

            viewer.Graph = graph;

            form.SuspendLayout();
            viewer.Dock = DockStyle.Fill;
            form.Controls.Add(viewer);
            form.ResumeLayout();

            Thread thread = new Thread(() => form.ShowDialog());
            thread.Start();
        }

        public void ShowResult(List<Work> order)
        {
            float width = 100 / order.Count();
            int i = 1;
            formTwo.SuspendLayout();
            table.ColumnCount = order.Count();
            table.RowCount = 1;
            table.Dock = DockStyle.Top;
            /*
            foreach(Work work in order)
            {
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            }
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 0F));
            //table.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            */
            foreach (Work work in order)
            {
                table.Controls.Add(new System.Windows.Forms.Label() { Text = i.ToString() }, i + 1, 0);
                i++;
            }

            formTwo.Controls.Add(table);
            
            formTwo.ResumeLayout();

            Thread threadTwo = new Thread(() => formTwo.ShowDialog());
            threadTwo.Start();
        }

        public void ShowinCmd(List<Work> order)
        {


            Console.Write("|Czas    |");
            int i = 1;
            foreach(Work work in order)
            {
                if(i < 10)
                    Console.Write("  {0} |", i);
                else
                    Console.Write(" {0} |", i);

                i++;
            }
            Console.Write("\n|Zadania |");
            foreach (Work work in order)
            {
                if (work != null)
                {
                    SwitchColor(work.Id);
                    Console.Write(" Z{0} ", work.Id);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("|");
                }
                else
                {
                    Console.Write("XXXX|");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine();

        }

        private void SwitchColor(int i)
        {
            switch (i)
            {
                case 1:
                    Console.BackgroundColor = ConsoleColor.Blue;
                    break;
                case 2:
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;
                case 3:
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    break;
                case 4:
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    break;
                case 5:
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    break;
                case 6:
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    break;
                case 7:
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    break;
                default:
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
            }
        }
    }
}
