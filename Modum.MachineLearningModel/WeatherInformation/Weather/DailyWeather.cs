using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Modum.Models.MachineLearning.FeedbackOfProducts;

namespace Modum.Models.MachineLearning.Weather
{
    internal class DailyWeather
    {
        public long Dt { get; set; }
        public Temperature Temp { get; set; }
        public string Summary { get; set; }
        public List<WeatherDescription> Weather { get; set; }
    }
}
