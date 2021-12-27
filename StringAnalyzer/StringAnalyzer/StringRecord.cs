using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherBreaker
{
    public class StringRecord
    {
        public string gramm;
        public double count;

        public static string filename = "D://Folya//docs//homeworks//7 sem//security//trigrams.txt";

        public StringRecord(string gramm, double count)
        {
            this.gramm = gramm;
            this.count = count;
        }
        /*
        public static List<StringRecord> trigramFreqEngl = new List<StringRecord>()
        {
            new StringRecord("the", 1.81),
            new StringRecord("and", 0.73),
            new StringRecord("ing", 0.72),
            new StringRecord("ent", 0.42),
            new StringRecord("ion", 0.42),
            new StringRecord("for", 0.34),
            new StringRecord("tha", 0.33),
            new StringRecord("tio", 0.31)
        };
        */

        public static List<string> alphabet = new List<string>()
        {
            "a",
            "b",
            "c",
            "d",
            "e",
            "f",
            "g",
            "h",
            "i",
            "j",
            "k",
            "l",
            "m",
            "n",
            "o",
            "p",
            "q",
            "r",
            "s",
            "t",
            "u",
            "v",
            "w",
            "x",
            "y",
            "z"
        };


        public static List<StringRecord> trigramFreqEngl = readFromFile();
        public static List<StringRecord> readFromFile()
        {
            StreamReader reader = new StreamReader(filename);
            List<StringRecord> result = new List<StringRecord>();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string gramm = GetLowerString(line.Split(' ')[0].ToCharArray());
                string number = line.Split(' ')[1];
                IFormatProvider format = new NumberFormatInfo { NumberDecimalSeparator = "." };
                decimal parsedNum = decimal.Parse(number, NumberStyles.Float, format);
                result.Add(new StringRecord(gramm, (double)parsedNum * 100));
            }

            List<StringRecord> reduced = new List<StringRecord>();
            int index = 0;
            string a, b, c;
            while (true)
            {
                a = alphabet[index];
                index++;
                if (index >= alphabet.Count)
                    break;
                b = alphabet[index];
                index++;
                if (index >= alphabet.Count)
                    break;
                c = alphabet[index];
                index++;
                if (index >= alphabet.Count)
                    break;
                foreach (var el in result)
                {
                    if(el.gramm.Contains(a) && el.gramm.Contains(b) && el.gramm.Contains(c))
                    {
                        reduced.Add(el);
                    }
                }
            }

            return reduced;
        }

        public static void PrintList(List<StringRecord> records)
        {
            foreach (var el in records)
            {
                Console.WriteLine(el.gramm + ": " + el.count);
            }
        }

        public static StringRecord CountPercents(string input, string substring)
        {
            double total = (double)input.Length / substring.Length;
            double count = 0;
            int index = 0;
            while (input.IndexOf(substring, index) >= 0)
            {
                index = input.IndexOf(substring, index) + substring.Length;
                count++;
            }
            return new StringRecord(substring, (count / total) * 100);
        }

        public static List<StringRecord> CountAllPercents(string input, List<StringRecord> example)
        {
            List<StringRecord> result = new List<StringRecord>();
            foreach (var el in example)
            {
                result.Add(CountPercents(input, el.gramm));
            }
            return result;
        }

        public static double GetDiff(StringRecord gramFreq, List<StringRecord> example)
        {
            foreach (var el in trigramFreqEngl)
            {
                if (gramFreq.gramm.Equals(el.gramm))
                {
                    return Math.Abs(gramFreq.count - el.count);
                }
            }
            throw new Exception("gramm not found");
        }

        public static List<StringRecord> GetDiffs(List<StringRecord> counted, List<StringRecord> example)
        {
            List<StringRecord> result = new List<StringRecord>();
            foreach (var el in counted)
            {
                result.Add(new StringRecord(el.gramm, GetDiff(el, example)));
            }
            return result;
        }

        public static double GetCountSum(List<StringRecord> freqs)
        {
            double result = 0;
            foreach (var el in freqs)
            {
                result += el.count;
            }
            return result;
        }

        public static string GetLowerString(char[] input)
        {
            char[] text = new char[input.Length];
            input.CopyTo(text, 0);
            for (int i = 0; i < text.Length; i++)
            {
                text[i] = char.ToLower(text[i]);
            }
            return new string(text);
        }

        public static double GetPresence(string input, List<StringRecord> gramms)
        {
            double count = 0;
            int index = 0;
            foreach (var el in gramms)
            {
                index = 0;
                while (input.IndexOf(el.gramm, index) >= 0)
                {
                    index = input.IndexOf(el.gramm, index);
                    count++;
                }
            }
            return count;
        }
    }
}
