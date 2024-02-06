using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modum.Models.MachineLearning.Weather
{
    internal class WeatherData
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string Timezone { get; set; }
        public int TimezoneOffset { get; set; }
        public CurrentWeather Current { get; set; }
        public List<DailyWeather> Daily { get; set; }
        public List<Alert> Alerts { get; set; }
    }
}
