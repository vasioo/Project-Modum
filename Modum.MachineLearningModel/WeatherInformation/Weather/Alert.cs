using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modum.Models.MachineLearning.Weather
{
    internal class Alert
    {
        public string Event { get; set; }
        public long Start { get; set; }
        public long End { get; set; }
        public string Description { get; set; }
    }
}
