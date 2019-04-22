using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liu.Model
{
    public class Work
    {
        public Work()
        {
            predecesors = new List<Work> ();
            succesors = new List<Work>();
            Completed = false;
            Executed = 0;
        }

        public Work(int id, int arrival, int execultion, int duetime) : this()
        {
            Id = id;
            Arrival = arrival;
            Execution = execultion;
            DueTime = duetime;
        }

        public int Id { get; set; }
        public int Arrival { get; set; }
        public int Execution { get; set; }
        public int DueTime { get; set; }
        public int CorrectedDueTime { get; set; }
        public bool Completed { get; set; }
        public int Executed { get; set; }
        public int Lateness { get; set; }
        public List<Work> predecesors;
        public List<Work> succesors;
    }
}
