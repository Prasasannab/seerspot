using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeerSpot3.Models;
using Newtonsoft.Json;
using System.Diagnostics;

namespace SeerSpot3.Controllers
{
    public class HomeController : Controller
    {

        string StorageName = "seerspot";
        string StorageKey = "PYQSJwzgabBe3TWGhC0/Im2DQkzzGXSahrrd/5TQxxMnl9BizZkzI4jhkvB7aUxQ8LljwoDA28grbb644ugSPA==";
        string TableName = "CurrentValue";

        static List<Object> updatedArray = new List<Object>();

        public ActionResult Index()
        {
            string jsonData;
            AzureTables.GetAllEntity(StorageName, StorageKey, TableName, out jsonData);
            Debug.WriteLine(jsonData);

            ModelView convert = JsonConvert.DeserializeObject<ModelView>(jsonData);

            List<ModelViewDetails> details = cleanData(convert.Value);
            future(details);

            Debug.WriteLine(updatedArray);

            TestModel model = new TestModel();
            model.DataList = updatedArray;
            return View(model);
        }

        public static List<Object> future(List<ModelViewDetails> data, int interval = 1)
        {
            DateTime endtime = DateTime.Now;


            DateTime starttime = data[0].Timestamp;

            for (int i = 0; i < data.Count;)
            {


                if (endtime != null || endtime < data[i].Timestamp)
                {
                    starttime = data[i].Timestamp;
                    endtime = GetUpdatedTime(interval, data[i].Timestamp);
                }



                List<ModelViewDetails> filteredArray = data.FindAll(d => (d.Timestamp >= starttime && d.Timestamp < endtime));


                //[time, low, open, close, high]

                //[10, 5, 9, 20, 6]

                List<Object> array = new List<Object>();

                array.Add(ReduceMins(starttime) + " " + endtime);//time

                array.Add(filteredArray[0].StockCurrentValue);//open
                array.Add(filteredArray[filteredArray.Count - 1].StockCurrentValue);//close
                                                                                    //[time,min, open, close, max]

                filteredArray.Sort(new Sorter());
                array.Insert(1, filteredArray[filteredArray.Count - 1].StockCurrentValue);//min
                array.Add(filteredArray[0].StockCurrentValue);//max

                //[[],[],[],[],[]]

                updatedArray.Add(array.ToArray());
                //}//0, 5
                if (filteredArray.Count > 0)
                {
                    i = i + filteredArray.Count;
                }
                else
                {
                    i = i + 1;
                }
            }
            return updatedArray;
        }

        public static DateTime GetUpdatedTime(int future, DateTime date)
        {

            return new DateTime(date.Year, date.Month, date.Day, (date.Hour + future), 0, 0);
        }

        public static DateTime ReduceMins(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0); ;
        }


        public static List<ModelViewDetails> cleanData(List<ModelViewDetails> stocks)
        {
            List<ModelViewDetails> cleansedData = stocks;
            if (cleansedData.Count > 0)
            {
                foreach (ModelViewDetails stock in cleansedData)
                {
                    if (!String.IsNullOrEmpty(stock.Currentvalue))
                    {
                        int index = stock.Currentvalue.IndexOf('$');
                        if (index != -1)
                        {

                            Double value = 0.0;
                            bool isSuccess = Double.TryParse(stock.Currentvalue.Substring((index + 1)), out value);

                            stock.StockCurrentValue = value;
                        }
                    }
                }
            }
            return cleansedData;
        }
    }

    public class Sorter : IComparer<ModelViewDetails>
    {
        public int Compare(ModelViewDetails x, ModelViewDetails y)
        {
            return y.StockCurrentValue.CompareTo(x.StockCurrentValue);
        }
    }
}