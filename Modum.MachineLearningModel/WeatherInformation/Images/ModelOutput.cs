using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modum.MachineLearningModel.WeatherInformation.Images
{
    internal class ModelOutput
    {
        [ColumnName("grid")]
        public float[] PredictedLabels;
    }
}
