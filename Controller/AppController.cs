using Liu.Model;
using Liu.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liu.Controller
{
    public class AppController
    {
        private Graph graph;
        private Interaction interaction = new Interaction();
        private static int time = 0;
        private List<Work> order = new List<Work>();
        private ShowData data = new ShowData();

        public void GetGraph(string filename)
        {
            graph = interaction.ReadXML(filename);
        }

        private bool CheckCompleted()
        {
            List<int> keys = graph.Works.Keys.ToList();
            foreach(int key in keys)
            {
                if (graph.Works[key].Completed == false)
                {
                    return false;
                }
            }
            return true;
        }

        private List<int> InSystem(int time)
        {
            List<int> keys = graph.Works.Keys.ToList();
            List<int> ready = new List<int>();
            bool completed;
            foreach(int key in keys)
            {

                if (graph.Works[key].Arrival <= time )
                    if(graph.Works[key].Completed == false)
                    {
                        completed = true;
                        foreach (Work work in graph.Works[key].predecesors)
                        {
                            if (!work.Completed)
                                completed = false;
                        }
                        if(completed)
                            ready.Add(key);
                    }
                        
            }
            return ready;
        }

        private void IncreaseExecuted(int key)
        {
            graph.Works[key].Executed++;
            if (graph.Works[key].Execution <= graph.Works[key].Executed)
            {
                graph.Works[key].Completed = true;
                graph.Works[key].Lateness = time - graph.Works[key].DueTime + 1;
            }
        }

        public void OrderWorks()
        {
            List<int> keys;
            int current = -1;
            while (!CheckCompleted())
            {
                keys = InSystem(time);

                if (keys.Count() > 0)
                    current = keys[0];

                foreach (int key in keys)
                {
                    if (graph.Works[key].CorrectedDueTime < graph.Works[current].CorrectedDueTime)
                        current = key;
                }

                if (keys.Count() > 0)
                {
                    order.Add(graph.Works[current]);
                    IncreaseExecuted(current);
                }
                else
                {
                    order.Add(null);
                }
                time++;
            }
        }
        

        public int GetNewDueTime(int key, int dueTime)
        {
            int time, lowest = dueTime;
            Work temp = graph.Works[key];

            if (temp.succesors.Count <= 0)
                return temp.DueTime;
            else
            {
                if (temp.DueTime < dueTime)
                    lowest = temp.DueTime;

                foreach(Work successor in temp.succesors)
                {
                    time = GetNewDueTime(successor.Id, dueTime);
                    if (time < lowest)
                        lowest = time;
                }
            }
            return lowest;
        }

        private void AssignCorrectedDueTime(int key)
        {
            graph.Works[key].CorrectedDueTime = GetNewDueTime(key, graph.Works[key].DueTime);
        }

        public void CalculateDueTime()
        {
            List<int> keys = graph.Works.Keys.ToList();

            foreach(int value in keys)
            {
                AssignCorrectedDueTime(value);
            }
        }

        public void WriteWorks()
        {
            interaction.WriteTasks(graph);
        }

        public void WriteNewDueTime()
        {
            interaction.WriteNewDueDate(graph);
        }

        public void WriteLateness()
        {
            interaction.WriteLatenes(graph);
        }

        public void WriteTime()
        {
            interaction.WriteTime(time);
        }

        public void WriteMaxLateness()
        {
            int max = graph.Works[1].Lateness;
            List<int> keys = graph.Works.Keys.ToList();
            foreach(int key in keys)
            {
                if (max < graph.Works[key].Lateness)
                    max = graph.Works[key].Lateness;
            }

            interaction.WriteLatenessMax(max);
        }

        public void ShopGraph()
        {
            data.grap = graph;

            data.CreateGraph();
        }

        public void ShowResult()
        {
            data.grap = graph;

            data.ShowinCmd(order);
        }
    }
}
