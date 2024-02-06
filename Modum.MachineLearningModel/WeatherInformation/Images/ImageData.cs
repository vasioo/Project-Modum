using Microsoft.ML.Data;

namespace Modum.MachineLearningModel.WeatherInformation.Images
{
    public class ImageData
    {
        [LoadColumn(0)]
        public string ImagePath;

        [LoadColumn(1)]
        public string Label;
    }
}
