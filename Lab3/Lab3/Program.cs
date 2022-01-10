using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Lab3
{
    class Program
    {
        public static void Main()
        {
            string linkBase = "http://95.217.177.249/casino/";

            Account test = CreateNewAcc(linkBase);

            Console.WriteLine($"id: {test.ID}");
            Console.WriteLine($"money: {test.Money}");
            Console.WriteLine($"deletionTime: {test.DeletionTime}");
        }

        static string SendRequestGetResponse(string request)
        {
            WebRequest webReq = WebRequest.Create(request);
            webReq.Method = "GET";

            using WebResponse webResp = webReq.GetResponse();
            using Stream webStream = webResp.GetResponseStream();

            using StreamReader reader = new StreamReader(webStream);
            string recieved = reader.ReadToEnd();

            return recieved;
        }

        static Account CreateNewAcc(string uriBase)
        {
            Account result = null;
            Random rnd = new Random();
            int ID = 0;
            string request;

            bool succsessFlag = true;
            do
            {
                try
                {
                    ID = rnd.Next(10000);
                    request = uriBase + "createacc?id=" + ID.ToString();
                    string responseJson = SendRequestGetResponse(uriBase);

                    result = JsonConvert.DeserializeObject<Account>(responseJson);

                    succsessFlag = true;
                }
                catch (System.Net.WebException)
                {
                    succsessFlag = false;
                }
            } while (!succsessFlag);

            return result;
        }
    }
}
