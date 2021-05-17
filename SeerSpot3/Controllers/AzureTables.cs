using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace SeerSpot3.Controllers
{
    public class AzureTables
    {
        public class Stock
        {
            public DateTime Timestamp = DateTime.Now;
         
            public string Currentvalue;

            public Double StockCurrentValue;
        }

        public class StockValue
        {
            public DateTime time = DateTime.Now;

            public string price;
        }

        public class Stocks
        {
            public Stock[] value;
        }

        private static List<StockValue> ConvertOBJ(Stocks array)
        {
            List<StockValue> list = new List<StockValue>();
            foreach (Stock arrayValues in array.value)
            {
                StockValue values = new StockValue();
                values.time = arrayValues.Timestamp;
                values.price = arrayValues.Currentvalue;
                list.Add(values);
            }


            return list;
        }
        public static int GetAllEntity(string storageAccount, string accessKey, string resourcePath, out string jsonData)
        {
            string uri = @"https://" + storageAccount + ".table.core.windows.net/" + resourcePath;

            // Web request   
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);

            int query = resourcePath.IndexOf("?");
            if (query > 0)
            {
                resourcePath = resourcePath.Substring(0, query);
            }

            request = getRequestHeaders("GET", request, storageAccount, accessKey, resourcePath);



            // Execute the request  
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    //   HttpResponseMessage response = 
                    using (System.IO.StreamReader r = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        jsonData = r.ReadToEnd();
                        Console.WriteLine("nsjsnjns , " + jsonData);


                        Stocks readTask = JsonConvert.DeserializeObject<Stocks>(jsonData);
                        List<StockValue> stockValues = ConvertOBJ(readTask);
                        Stock[] stockData = cleanData(readTask.value);
                        Console.WriteLine("nsjsnjns , " + readTask.value);
                        //List<Item> items = JsonConvert.DeserializeObject<List<Item>>(response);
                        return (int)response.StatusCode;
                    }
                }
            }
            catch (WebException ex)
            {
                // get the message from the exception response  
                using (System.IO.StreamReader sr = new System.IO.StreamReader(ex.Response.GetResponseStream()))
                {
                    jsonData = sr.ReadToEnd();
                    // Log res if required  
                }

                return (int)ex.Status;
            }
        }

        public static Stock[] cleanData(Stock[] stocks)
        {
            Stock[] cleansedData = stocks;
            if (cleansedData.Length > 0)
            {
                foreach (Stock stock in cleansedData)
                {
                    if (!String.IsNullOrEmpty(stock.Currentvalue))
                    {
                        int index = stock.Currentvalue.IndexOf('$');
                        //if (index != -1)
                        //{
                        //    // String value = stock.Currentvalue.Substring((index + 1));
                        //    Double value = 0.0;
                        //    bool isSuccess = Double.TryParse(stock.Currentvalue.Substring((index + 1)), out value);
                        //    // if (isSuccess)
                        //    //  {
                        //    stock.StockCurrentValue = value;
                        //    //  }
                        //    // stock.Currentvalue = value;

                        //}
                        //else
                        //{
                            Double value = 0.0;
                            bool isSuccess = Double.TryParse(index == -1 ? stock.Currentvalue : stock.Currentvalue.Substring((index + 1)), out value);
                            // if (isSuccess)
                            //  {
                            stock.StockCurrentValue = value;
                     //   }
                    }
                }


            }
            return cleansedData;
        }


        public static HttpWebRequest getRequestHeaders(string requestType, HttpWebRequest Newrequest, string storageAccount, string accessKey, string resource, int Length = 0)
        {
            HttpWebRequest request = Newrequest;

            switch (requestType.ToUpper())
            {
                case "GET":
                    request.Method = "GET";
                    request.ContentType = "application/json";
                    request.ContentLength = Length;
                    request.Accept = "application/json;odata=nometadata";
                    request.Headers.Add("x-ms-date", DateTime.UtcNow.ToString("R", System.Globalization.CultureInfo.InvariantCulture));
                    request.Headers.Add("x-ms-version", "2015-04-05");
                    request.Headers.Add("Accept-Charset", "UTF-8");
                    request.Headers.Add("MaxDataServiceVersion", "3.0;NetFx");
                    request.Headers.Add("DataServiceVersion", "1.0;NetFx");
                    break;

            }

            string sAuthorization = getAuthToken(request, storageAccount, accessKey, resource);
            request.Headers.Add("Authorization", sAuthorization);
            return request;
        }

        public static string getAuthToken(HttpWebRequest request, string storageAccount, string accessKey, string resource)
        {
            try
            {
                string sAuthTokn = "";

                string stringToSign = request.Headers["x-ms-date"] + "\n";

                stringToSign += "/" + storageAccount + "/" + resource;

                HMACSHA256 hasher = new HMACSHA256(Convert.FromBase64String(accessKey));

                sAuthTokn = "SharedKeyLite " + storageAccount + ":" + Convert.ToBase64String(hasher.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));

                return sAuthTokn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}