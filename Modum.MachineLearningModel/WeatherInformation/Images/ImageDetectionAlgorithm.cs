using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.ML;
using Modum.Models.MachineLearning.Weather;
using Modum.Services.Interfaces;
using Newtonsoft.Json;

namespace Modum.MachineLearningModel.WeatherInformation.Images
{
    public class ImageDetectionAlgorithm
    {
        private readonly IProductService _productService;
        private readonly ISubcategoryService _subcategoryService;
        public IConfiguration _configuration { get; set; }
        public ImageDetectionAlgorithm(IProductService productService, ISubcategoryService subcategoryService, IConfiguration  configuration)
        {
            _productService = productService;
            _subcategoryService = subcategoryService;
            _configuration = configuration;
        }

        //public async Task<IEnumerable<ImageData>> GetProductsImageContainerIds()
        //{
        //    var subcategories = _subcategoryService.IQueryableGetAllAsync();

        //    var products = _productService.IQueryableGetAllAsync();

        //    var imageDataList = await products
        //        .Select(x => new ImageData
        //        {
        //            ImagePath = x.ImageContainerId,
        //            Label = subcategories.Where(y=>y.Id==x.SubcategoryId).Select(z=>z.Name).FirstOrDefault()??"",
        //        })
        //        .ToListAsync();

        //    return imageDataList;
        //}

        //public async Task<List<string>> GetPredictions()
        //{
        //    var context = new MLContext();

        //    var enumerableData = await GetProductsImageContainerIds();

        //    // Load data with Cloudinary image paths
        //    var data = context.Data.LoadFromEnumerable(enumerableData,schema: null);

        //    // Define the pipeline
        //    var pipeline = context.Transforms.Conversion.MapValueToKey("Label")
        //        .Append(context.Transforms.Conversion.MapKeyToValue("Label"))
        //        .Append(context.Transforms.Conversion.MapKeyToValue("CloudinaryImagePath"));

        //    // Train the model
        //    var model = pipeline.Fit(data);

        //    // Loop through all predictions
        //    var predictions = context.Data.CreateEnumerable<ImagePrediction>(data, reuseRowObject: false);

        //    // Extract and return predicted labels
        //    return predictions.Select(prediction => prediction.PredictedLabel).ToList();
        //}

        public async void FilterAlgorithm()
        {
            //based on the weather api filter the products forecastly meaning if the next 20 days are going to be colder show warmer clothes

            //adding a property scale in the product class which will rank which products should be more upfront the alg will be updated weekly

            //a mapping between the image recognition and the kind of weather should be done for now i am returning the prediction of what the product is
            var configuredWeather = await GetWeatherInformation();

            //var imageData = await GetPredictions();

            if (configuredWeather != null)
            {

            }

        }
        private async Task<WeatherInfo> GetWeatherInformation()
        {
            string apiKey = _configuration["WEATHERAPIKEY"]!;
            string country = "Bulgaria";

            using (HttpClient client = new HttpClient())
            {
                string apiUrl = $"http://api.openweathermap.org/data/2.5/forecast?q={country}&appid={apiKey}";

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(jsonResponse)!;

                    double currentTemperature = weatherData!.Current.Temp;
                    string currentWeather = weatherData.Current.Weather[0].Description;

                    List<DailyWeatherInfo> dailyWeatherInfoList = weatherData.Daily.Select(daily =>
                        new DailyWeatherInfo
                        {
                            DayTemperature = daily.Temp.Day,
                            DaySummary = daily.Summary
                        }).ToList();

                    WeatherInfo weatherInfo = new WeatherInfo
                    {
                        CurrentTemperature = currentTemperature,
                        CurrentWeather = currentWeather,
                        DailyWeather = dailyWeatherInfoList
                    };

                    return weatherInfo;
                }
                else
                {
                    Console.WriteLine("Failed to retrieve weather data.");
                    return null!;
                }
            }
        }
    }
}
