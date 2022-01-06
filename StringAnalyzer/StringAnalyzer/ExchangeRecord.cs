using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherBreaker
{
    public class ExchangeRecord
    {
        public char a;
        public char b;

        public ExchangeRecord(char a, char b)
        {
            this.a = a;
            this.b = b;
        }

        public static List<ExchangeRecord> KeyFromString(string key)
        {
            List<ExchangeRecord> result = GetZeroExchangeList(CharRecord.englLiteralsFreq);
            key = key.ToLower();
            for (int i = 0; i < key.Length; i++)
            {
                result[i].b = key[i];
            }
            return result;
        }

        public static List<ExchangeRecord> GenExchangeList(List<CharRecord> alphabet)
        {
            Random rnd = new Random();
            List<ExchangeRecord> result = GetZeroExchangeList(alphabet);

            for (int i = 0; i < 200; i++)
            {
                result = ChangeExchangeList(result, rnd);
            }
            return result;
        }

        public static List<ExchangeRecord> GetZeroExchangeList(List<CharRecord> alphabet)
        {
            List<ExchangeRecord> result = new List<ExchangeRecord>();
            for (int i = 0; i < alphabet.Count; i++)
            {
                result.Add(new ExchangeRecord(alphabet[i].Character, alphabet[i].Character));
            }
            return result;
        }

        public static List<ExchangeRecord> ChangeExchangeList(List<ExchangeRecord> list, Random rnd)
        {
            List<ExchangeRecord> result = new List<ExchangeRecord>();
            result.AddRange(list);
            int aI = rnd.Next(result.Count);
            int bI = rnd.Next(result.Count);
            char buffer = result[aI].b;
            result[aI].b = result[bI].b;
            result[bI].b = buffer;

            return result;
        }

        public static List<ExchangeRecord> GetEmptyKey(int size)
        {
            List<ExchangeRecord> result = new List<ExchangeRecord>();
            for (int i = 0; i < size; i++)
            {
                result.Add(new ExchangeRecord(StringRecord.alphabet[i][0], '-'));
            }
            return result;
        }

        public static string KeyToString(List<ExchangeRecord> keys)
        {
            string result = "";
            foreach (var el in keys)
            {
                result += el.b;
            }
            return result;
        }

        public static void Show(List<ExchangeRecord> recs)
        {
            foreach (var el in recs)
            {
                Console.WriteLine($"{el.a} - {el.b}");
            }
        }
    }
}
