using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Modum.Models.MachineLearning.FeedbackOfProducts;

namespace Modum.Models.MachineLearning.Weather
{
    internal class CurrentWeather
    {
        public long Dt { get; set; }
        public double Temp { get; set; }
        public List<WeatherDescription> Weather { get; set; }
    }
}
