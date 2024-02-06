using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modum.Models.MachineLearning
{
    public class FeedbackDataModel
    {
        [Column(Order =0)]
        public byte ItemScale { get; set; }
        //The item scale sends information based on the weather api if the software should evaluate the products

        [Column(Order =1)]
        public string FeedbackText { get; set; }
    }
}
