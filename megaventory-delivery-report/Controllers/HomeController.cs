using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;




namespace megaventory_delivery_report.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        static int MONTHS = 0;
       static List<Dictionary<string, object>> SalesOrders = new List<Dictionary<string, object>>();
        static List<Dictionary<string, object>> PurchaseOrders = new List<Dictionary<string, object>>();
        static List<Dictionary<string, object>> WorkOrders = new List<Dictionary<string, object>>();
        static List<Dictionary<string, object>> SupplierClients = new List<Dictionary<string, object>>();
        static int MONTH = 1;
        static String DATE_TO_BE_PROCESSED = "01/01/2019 00:00";
        public static void getTimeData(int months)
        {
            MONTHS = months;
             DateTime now = DateTime.Now;
            if (months > 0)
                now = now.AddMonths(months * -1);
            else
                now = now.AddMonths(-1);

            string dateToBeProcessed = "";
            if (now.Day < 10)
                dateToBeProcessed = "0" + now.Day;
            else
                dateToBeProcessed = ""+now.Day;
            if (now.Month < 10)
                dateToBeProcessed += "/0" + now.Month;
            else
                dateToBeProcessed += "/" + now.Month;
            dateToBeProcessed += "/" + now.Year;
            if (now.Hour < 10)
                dateToBeProcessed += " 0" + now.Hour;
            else
                dateToBeProcessed += " " + now.Hour;
            if (now.Minute < 10)
                dateToBeProcessed += ":0" + now.Minute;
            else
                dateToBeProcessed += ":" + now.Minute;
            DATE_TO_BE_PROCESSED = dateToBeProcessed;

        }
        public ActionResult report(string APIKEY)
        {
            
            return View();
        }


        public ActionResult PurchaseOrderGet(string APIKEY)
        {
            HomeController.getTimeData(MONTHS);
            string url = "https://api.megaventory.com/v2017a/json/reply/PurchaseOrderGet?APIKEY=" + APIKEY + "&Filters=[{FieldName:PurchaseOrderDate,SearchOperator:LessThan,SearchValue:" + DATE_TO_BE_PROCESSED + "}]";
            
            object [] list =  GetDictionaryObjects(url);
            
            foreach (object obj2 in list)
            {
                Dictionary<string, object> item = (Dictionary<string, object>)obj2;
               PurchaseOrders.Add(item);
            }
            Session["PurchaseOrders"] = PurchaseOrders;
            return View();
        }


        public ActionResult SalesOrderGet(int months, string APIKEY)
        {

            HomeController.getTimeData(months);
           
            string url = "https://api.megaventory.com/v2017a/json/reply/SalesOrderGet?APIKEY="+APIKEY+"&Filters=[{FieldName:SalesOrderDate,SearchOperator:GreaterThan,SearchValue:" + DATE_TO_BE_PROCESSED +"}]";
            object[] list = GetDictionaryObjects(url);
            foreach (object obj2 in list)
            {
                Dictionary<string, object> source = (Dictionary<string, object>)obj2;
                DateTime obj = DateTime.Now.Date;

                SalesOrders.Add(source);
               
            }
            Session["SalesOrders"] = SalesOrders;
            return View();
        }


        public ActionResult SupplierClientGet(string APIKEY)
        {
            string url = "https://api.megaventory.com/v2017a/json/reply/SupplierClientGet?APIKEY="+APIKEY;
            
            object[] list = GetDictionaryObjects(url);
            foreach (object obj2 in list)
            {
                Dictionary<string, object> item = (Dictionary<string, object>)obj2;
                SupplierClients.Add(item);
            }
            Session["SupplierClients"] = SupplierClients;
            return View();
        }


        public ActionResult WorkOrderGet(string APIKEY)
        {
            HomeController.getTimeData(MONTHS);
            string url = "https://api.megaventory.com/v2017a/json/reply/WorkOrderGet?APIKEY=" + APIKEY + "&Filters=[{FieldName:WorkOrderStartDate,SearchOperator:GreaterThan,SearchValue:" + DATE_TO_BE_PROCESSED + "}]";
            
             object [] list =  GetDictionaryObjects(url);
            foreach (object obj2 in list)
            {
                Dictionary<string, object> source = (Dictionary<string, object>)obj2;
                WorkOrders.Add(source);
              
            }
            Session["WorkOrders"] = WorkOrders;
            return View();
        } 

        public object[] GetDictionaryObjects(string url)
        {
            HttpClient client1 = new HttpClient();
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            JavaScriptSerializer serializer = new JavaScriptSerializer {
                MaxJsonLength = int.MaxValue
            };
            object[] list = (object[])(from x in (Dictionary<string, object>)serializer.DeserializeObject(new StreamReader(this.GetRequest(url, "GET", true).GetResponse().GetResponseStream()).ReadToEnd().ToString()) select x.Value).ToList<object>().ElementAt<object>(0);
            return list;
        }

        private HttpWebRequest GetRequest(string url, string httpMethod = "GET", bool allowAutoRedirect = true)
        {
            Uri uri1 = new Uri(url);
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            
            request.UserAgent = "Mozilla/4.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
            request.Timeout = Convert.ToInt32(new TimeSpan(0, 10, 0).TotalMilliseconds);
            request.Method = httpMethod;
            return request;
        }
         
        public ActionResult Index()
        {
            return View();
        }

    }
}
