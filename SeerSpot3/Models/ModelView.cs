using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeerSpot3.Models
{
    public class ModelView
    {
        public List<ModelViewDetails> Value { get; set; }
    }

    public class ModelViewDetails
    {
        public DateTime Timestamp { get; set; }

        public string Currentvalue { get; set; }

        public Double StockCurrentValue { get; set; }

    }
    public class Stocks
    {
        public ModelViewDetails[] value;
    }

    public class ChartData
    {
        public string Timestamp { get; set; }

        public Double Max { get; set; }

        public Double Min { get; set; }

        public Double Open { get; set; }

        public Double Close { get; set; }
    }

    public class TestModel
    {
        public List<Object> DataList { get; set; }
    }
}