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

        public static List<ExchangeRecord> GenExchangeList(List<CharRecord> alphabet)
        {
            Random rnd = new Random();
            List<ExchangeRecord> result = new List<ExchangeRecord>();
            for (int i = 0; i < alphabet.Count; i++)
            {
                result.Add(new ExchangeRecord(alphabet[i].Character, alphabet[i].Character));
            }
            
            for (int i = 0; i < 200; i++)
            {
                result = ChangeExchangeList(result, rnd);
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
    }
}
