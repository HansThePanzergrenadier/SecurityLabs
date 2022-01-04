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
        public static string filenameWrite = "D://Folya//docs//homeworks//7 sem//security//trigramsReduced.txt";

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
        /*
        public static Dictionary<string, double> trigramFreqEngl = new Dictionary<string, double>()
        {
            new StringRecord("the", 5.00),
            new StringRecord("and", 0.83),
            new StringRecord("ing", 0.41),
            new StringRecord("ent", 0.83),
            new StringRecord("ion", 1.25),
            new StringRecord("for", 0.42),
            new StringRecord("tha", 0.42),
            new StringRecord("tio", 1.25)
        };
        */
        //public static Dictionary<string, double> trigramFreqEngl = FromShort();
        static Dictionary<string, double> FromShort()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();
            result.Add("the", 5.00);
            result.Add("and", 0.83);
            result.Add("ing", 0.41);
            result.Add("ent", 0.83);
            result.Add("ion", 1.25);
            result.Add("for", 0.42);
            result.Add("tha", 0.42);
            result.Add("tio", 1.25);
            return result;
        }
        

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


        public static Dictionary<string, double> trigramFreqEngl = readFromFile();
        static Dictionary<string, double> readFromFile()
        {
            StreamReader reader = new StreamReader(filename);
            Dictionary<string, double> result = new Dictionary<string, double>();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string gramm = GetLowerString(line.Split(' ')[0].ToCharArray());
                string number = line.Split(' ')[1];
                IFormatProvider format = new NumberFormatInfo { NumberDecimalSeparator = "." };
                decimal parsedNum = decimal.Parse(number, NumberStyles.Float, format);
                result.Add(gramm, (double)parsedNum * 100);
            }

            return result;
        }

        public static Dictionary<string, double> ReduceTrigrams(Dictionary<string, double> trigrams, char[] text)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();
            string strText = new string(text);

            foreach(var el in trigrams)
            {
                if (strText.Contains(el.Key))
                {
                    result.Add(el.Key, el.Value);
                }
            }

            return result;
        }

        public static Dictionary<string, double> ReduceTrigramsLeastValuable(Dictionary<string, double> trigrams, int volume)
        {
            Dictionary<string, double> sorted = new Dictionary<string, double>();
            Dictionary<string, double> trigs = new Dictionary<string, double>(trigrams);

            while(trigs.Count > 0)
            {
                KeyValuePair<string, double> max = new KeyValuePair<string, double>(" ", double.MinValue);
                foreach(var el in trigs)
                {
                    if(el.Value > max.Value)
                    {
                        max = el;
                    }
                }
                sorted.Add(max.Key, max.Value);
                trigs.Remove(max.Key);
            }

            Dictionary<string, double> result = new Dictionary<string, double>();
            for(int i = 0; i < volume; i++)
            {
                KeyValuePair<string, double> key = sorted.ElementAt(i);
                result.Add(key.Key, key.Value);
            }

            return result;
        }

        public static void SaveToFile(Dictionary<string, double> trigrams)
        {
            StreamWriter writer = new StreamWriter(filenameWrite);
            
            foreach(var el in trigrams)
            {
                string line = $"{el.Key} {el.Value:0.############E-0}";
                writer.WriteLine(line);
            }
            writer.Close();
        }

        public static void PrintList(Dictionary<string, double> records)
        {
            foreach (var el in records)
            {
                Console.WriteLine(el.Key + ": " + el.Value);
            }
        }

        public static double CountPercents(string input, string substring)
        {
            double total = (double)input.Length / substring.Length;
            double count = 0;
            int index = 0;
            while (input.IndexOf(substring, index) >= 0)
            {
                index = input.IndexOf(substring, index) + substring.Length;
                count++;
            }
            return (count / total) * 100;
        }

        public static Dictionary<string, double> CountAllPercents(string input, Dictionary<string, double> example)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();
            foreach (var el in example)
            {
                result.Add(el.Key, CountPercents(input, el.Key));
            }
            return result;
        }

        public static double GetDiff(KeyValuePair<string, double> gramFreq, Dictionary<string, double> example)
        {
            if (example.ContainsKey(gramFreq.Key))
            {
                example.TryGetValue(gramFreq.Key, out double exValue);
                return Math.Abs(gramFreq.Value - exValue);
            }
            else
            {
                throw new Exception("NGramm not found");
            }
        }

        public static Dictionary<string, double> GetDiffs(Dictionary<string, double> counted, Dictionary<string, double> example)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();
            foreach (var el in counted)
            {
                result.Add(el.Key, GetDiff(el, example));
            }
            return result;
        }

        public static double GetCountSum(Dictionary<string, double> freqs)
        {
            double result = 0;
            foreach (var el in freqs)
            {
                result += el.Value;
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
