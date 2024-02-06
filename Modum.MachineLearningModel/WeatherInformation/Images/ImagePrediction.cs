using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modum.MachineLearningModel.WeatherInformation.Images
{
    internal class ImagePrediction : ImageData
    {
        public float[] Score;
        public string PredictedLabel;
    }
}
