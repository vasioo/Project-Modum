using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Modum.Models.MachineLearning.FeedbackOfProducts;

namespace Modum.Models.MachineLearning.Weather
{
    internal class WeatherInfo
    {
        public double CurrentTemperature { get; set; }
        public string CurrentWeather { get; set; }
        public List<DailyWeatherInfo> DailyWeather { get; set; }
    }
}
