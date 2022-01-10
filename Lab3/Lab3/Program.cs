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
            
            Account testAcc = CreateAcc(linkBase);
            Console.WriteLine($"id: {testAcc.ID}");
            Console.WriteLine($"money: {testAcc.Money}");
            Console.WriteLine($"deletionTime: {testAcc.DeletionTime}");

            BetResponse test = CreateBet(linkBase, "Lcg", testAcc.ID, 10, 500);
            Console.WriteLine($"message: {test.Message}");
            Console.WriteLine($"accID: {test.Account.ID}");
            Console.WriteLine($"realNumber: { test.RealNumber}");
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

        static Account CreateAcc(string uriBase)
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
                    ID = rnd.Next(1488);
                    request = uriBase + "createacc?id=" + ID.ToString();
                    string responseJson = SendRequestGetResponse(request);

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

        static BetResponse CreateBet(string uriBase, string mode, string playerID, int betAmount, int betNumber)
        {
            BetResponse result = null;
            string request = $"{uriBase}play{mode}?id={playerID}&bet={betAmount}&number={betNumber}";

            while(result == null)
            {
                string response = SendRequestGetResponse(request);
                result = JsonConvert.DeserializeObject<BetResponse>(response);
            }

            return result;
        }
    }
}
