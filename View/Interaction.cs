using Liu.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Liu.View
{
    public class Interaction
    {
        private bool IsValid = true;
        private Work work = new Work();
        private Helper helper;
        private List<int> workIds = new List<int>();
        private List<Helper> helpers = new List<Helper>();

        Graph graph = new Graph
        {
            Works = new SortedDictionary<int, Work>()
        };

        private void CheckId(int id)
        {
            if (id <= 0)
            {
                Console.WriteLine("ID lower or eqaul 0");
                IsValid = false;
            }
        }

        private void CheckValue(int value)
        {
            if (value < 0)
            {
                Console.WriteLine("Value lower than 0");
                IsValid = false;
            }
        }

        private void AssignConnections()
        {
            foreach (Helper helper in helpers){
                if (helper.Succesor)
                {
                    graph.Works[helper.WorkId].succesors.Add(graph.Works[helper.Connect]);
                }else
                    graph.Works[helper.WorkId].predecesors.Add(graph.Works[helper.Connect]);
            }

        }

        private void ValueHandel(string name, string value)
        {
            switch (name)
            {
                case "id":
                    CheckId(Int32.Parse(value));
                    work.Id = Int32.Parse(value);
                    workIds.Add(Int32.Parse(value));
                    break;

                case "arrival":
                    CheckValue(Int32.Parse(value));
                    work.Arrival = Int32.Parse(value);
                    break;

                case "execution":
                    CheckValue(Int32.Parse(value));
                    work.Execution = Int32.Parse(value);
                    break;

                case "duedate":
                    CheckValue(Int32.Parse(value));
                    work.DueTime = Int32.Parse(value);
                    break;

                case "succesor-id":
                    try
                    {
                        CheckId(Int32.Parse(value));
                        helper = new Helper
                        {
                            WorkId = work.Id,
                            Connect = Int32.Parse(value),
                            Succesor = true
                        };
                        helpers.Add(helper);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Succesor ID is lower or equal 0");
                        IsValid = false;
                    }
                    break;

                case "predecesor-id":
                    try
                    {
                        CheckId(Int32.Parse(value));
                        helper = new Helper
                        {
                            WorkId = work.Id,
                            Connect = Int32.Parse(value),
                            Succesor = false
                        };
                        helpers.Add(helper);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Predecesor ID is lower or equal 0");
                        IsValid = false;
                    }
                    break;
            }
        }

        private void CloseHandler(string value)
        {
            switch (value)
            {
                case "node":
                    graph.Works.Add(work.Id, work);
                    work = new Work();
                    break;
            }
        }

        public Graph ReadXML(string filename) {

            string name = "";
            StringBuilder sb = new StringBuilder(@"E:\DotNet\Liu\Liu\Resources\");
            sb.Append(filename);

            XmlReaderSettings settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Parse
            };
            XmlReader reader = XmlReader.Create(sb.ToString(), settings);

            reader.MoveToContent();

            while (reader.Read() && IsValid)
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        name = reader.Name;
                        break;
                    case XmlNodeType.Text:
                        ValueHandel(name, reader.Value);
                        break;
                    case XmlNodeType.EndElement:
                        CloseHandler(reader.Name);
                        break;
                }
            }

            AssignConnections();
           // CheckValid();
            return graph;
        }

        public void Welcome()
        {
            Console.WriteLine("Zmodyfikowany algorytm Liu");
        }

        public void WriteTasks(Graph graph)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("==================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" Wczytane zadania");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("==================\n");
            int i;
            Work work;
            List<int> keys = graph.Works.Keys.ToList();
            for(i = 0; i < keys.Count(); i++)
            {
                work = graph.Works[keys[i]];
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Zadanie nr {0}", work.Id);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Czas wejścia do systemu: {0}", work.Arrival);
                Console.WriteLine("Czas wykonywania: {0}", work.Execution);
                Console.WriteLine("Termin zakończenia: {0}", work.DueTime);
                Console.WriteLine();
            }
        }

        public void WriteNewDueDate(Graph graph)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=============================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Nowy czas zakończenia zadania");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=============================\n");
            int i;
            Work work;
            List<int> keys = graph.Works.Keys.ToList();
            for (i = 0; i < keys.Count(); i++)
            {
                work = graph.Works[keys[i]];
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Zadanie nr {0}", work.Id);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Czas wejścia do systemu: {0}", work.Arrival);
                Console.WriteLine("Czas wykonywania: {0}", work.Execution);
                Console.WriteLine("Stary termin zakończenia: {0}", work.DueTime);
                Console.WriteLine("Nowy termin zakończenia: {0}", work.CorrectedDueTime);
                Console.WriteLine();
            }
        }

        public void WriteLatenes(Graph graph)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("====================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Opóźnienia dla zadań");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("====================\n");
            int i;
            Work work;
            List<int> keys = graph.Works.Keys.ToList();
            for (i = 0; i < keys.Count(); i++)
            {
                work = graph.Works[keys[i]];
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Zadanie nr {0}", work.Id);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Czas wejścia do systemu: {0}", work.Arrival);
                Console.WriteLine("Czas wykonywania: {0}", work.Execution);
                Console.WriteLine("Stary termin zakończenia: {0}", work.DueTime);
                Console.WriteLine("Nowy termin zakończenia: {0}", work.CorrectedDueTime);
                Console.WriteLine("Opóźnienie: {0}", work.Lateness);
                Console.WriteLine();
            }
        }

        public void WriteTime(int time)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Całkowity czas wykonywania wszystkich zadań: {0}\n", time);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void WriteLatenessMax(int lateness)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nMaksymalne opóźnienie: {0}", lateness);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
