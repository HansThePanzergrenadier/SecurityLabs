using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Numerics;
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
            /*
            Account testAcc = CreateAcc(linkBase);
            Console.WriteLine($"id: {testAcc.ID}");
            Console.WriteLine($"money: {testAcc.Money}");
            Console.WriteLine($"deletionTime: {testAcc.DeletionTime}");

            BetResponse test = CreateBet(linkBase, "Lcg", testAcc.ID, 10, 500);
            Console.WriteLine($"message: {test.Message}");
            Console.WriteLine($"accID: {test.Account.ID}");
            Console.WriteLine($"realNumber: { test.RealNumber}");
            */
            BrekLcg(linkBase);
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

            while (result == null)
            {
                string response = SendRequestGetResponse(request);
                result = JsonConvert.DeserializeObject<BetResponse>(response);
            }

            return result;
        }

        static long CalcIncrement(List<long> threeX, long m, long a)
        {
            return (threeX[1] - threeX[0] * a) % m;
        }

        static long CalcMul(List<long> threeX, long m)
        {
            return ((threeX[2] - threeX[1]) * ModInverse(threeX[1] - threeX[0], m)) % m;
        }

        static void BrekLcg(string uriBase)
        {
            long m = 4294967296;
            List<long> seeds = new List<long>();
            Account acc = CreateAcc(uriBase);
            long multiplier, increment;

            while (true)
            {
                seeds.Clear();

                for (int i = 0; i < 3; i++)
                {
                    BetResponse bet = CreateBet(uriBase, "Lcg", acc.ID, 5, 0);
                    acc = bet.Account;
                    seeds.Add(bet.RealNumber);
                }

                multiplier = (int)CalcMul(seeds, m);
                increment = (int)CalcIncrement(seeds, m, multiplier);

                if (seeds[1] == (seeds[0] * multiplier + increment) % m && seeds[2] == (seeds[1] * multiplier + increment) % m)
                {
                    break;
                }
            }

            //Console.WriteLine($"a: {multiplier}");
            //Console.WriteLine($"c: {increment}");

            acc = BankruptCasinoLcg(1000000, acc, multiplier, increment, m, uriBase);

            Console.WriteLine($"id: {acc.ID}");
            Console.WriteLine($"money: {acc.Money}");
            Console.WriteLine($"deletionTime: {acc.DeletionTime}");
        }

        static long ModInverse(long val, long mod)
        {
            long i = mod, v = 0, d = 1;
            while (val > 0)
            {
                long t = i / val, x = val;
                val = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= mod;
            if (v < 0) v = (v + mod) % mod;
            return v;
        }

        static long GenNext(long seed, long a, long c, long m)
        {
            return (seed * a + c) % m;
        }

        static Account BankruptCasinoLcg(long threshold, Account acc, long a, long c, long m, string uriBase)
        {
            BetResponse bet = CreateBet(uriBase, "Lcg", acc.ID, 5, 0);
            long currentNum = bet.RealNumber;
            acc = bet.Account;
            while (acc.Money < threshold)
            {
                int next = (int)GenNext(currentNum, a, c, m);
                bet = CreateBet(uriBase, "Lcg", acc.ID, acc.Money / 2, next);
                acc = bet.Account;
                currentNum = bet.RealNumber;
            }

            Console.WriteLine($"message: {bet.Message}");
            return acc;
        }
    }
}
