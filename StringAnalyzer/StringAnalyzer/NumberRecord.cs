using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherBreaker
{
    public class NumberRecord
    {
        public double Key;
        public double Count;

        public NumberRecord(double Key, double Count)
        {
            this.Key = Key;
            this.Count = Count;
        }

        public static void DrawSimple(List<NumberRecord> recs)
        {
            foreach (var el in recs)
            {
                Console.WriteLine("{0} - {1}", el.Key, el.Count);
            }
        }

        public static void DrawGraphics(List<NumberRecord> recs)
        {
            foreach (NumberRecord el in recs)
            {
                Console.Write(el.Key);
                Console.CursorLeft = 5;
                Console.Write("| {0:F2}", el.Count);
                Console.CursorLeft = 12;
                Console.Write("| ");
                for (int i = 0; i < el.Count; i++)
                {
                    Console.Write("█");
                }

                Console.WriteLine();
            }
        }
    }
}
