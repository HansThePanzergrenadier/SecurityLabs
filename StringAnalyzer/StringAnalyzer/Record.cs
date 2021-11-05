using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypherBreaker
{
    public class Record
    {
        public char Character;
        public double Count;
        public static String[] bigrams = new string[]
        {
            "th",
            "he",
            "in",
            "en",
            "nt",
            "re",
            "er",
            "an",
            "ti",
            "es",
            "on",
            "at",
            "se",
            "nd",
            "or",
            "ar",
            "al",
            "te",
            "co",
            "de",
            "to",
            "ra",
            "et",
            "ed",
            "it",
            "sa",
            "em",
            "ro",
            "nn",
            "gg",
            "mm",
            "ss",
            "ll",
            "rr",
            "oo",
        };
        public static List<Record> englLiteralsFreq = new List<Record>()
        {
            new Record('a', 8.167),
            new Record('b', 1.492),
            new Record('c', 2.782),
            new Record('d', 4.253),
            new Record('e', 12.702),
            new Record('f', 2.228),
            new Record('g', 2.015),
            new Record('h', 6.094),
            new Record('i', 6.966),
            new Record('j', 0.153),
            new Record('k', 0.772),
            new Record('l', 4.025),
            new Record('m', 2.406),
            new Record('n', 6.749),
            new Record('o', 7.507),
            new Record('p', 1.929),
            new Record('q', 0.095),
            new Record('r', 5.987),
            new Record('s', 6.327),
            new Record('t', 9.056),
            new Record('u', 2.758),
            new Record('v', 0.978),
            new Record('w', 2.360),
            new Record('x', 0.150),
            new Record('y', 1.974),
            new Record('z', 0.074)
        };

        public enum SortingMode
        {
            ByNumber,
            ByChar,
            None
        }

        public Record(char Character, double Count)
        {
            this.Character = Character;
            this.Count = Count;
        }

        public static bool Contains(List<Record> recs, char symbol)
        {
            foreach(var el in recs)
            {
                if (el.Character.Equals(symbol))
                {
                    return true;
                }
            }
            return false;
        }

        public static void MergingAdd(List<Record> recs, Record rec)
        {
            if(Contains(recs, rec.Character))
            {
                for(int i = 0; i < recs.Count; i++)
                {
                    if (recs[i].Character.Equals(rec.Character))
                    {
                        recs[i].Count += rec.Count;
                    }
                }
            }
            else
            {
                recs.Add(rec);
            }
        }

        public static void DrawSimple(SortedDictionary<char, int> counted)
        {
            foreach (var el in counted)
            {
                Console.WriteLine("{0} - {1}", el.Key, el.Value);
            }
        }

        public static void DrawSimple(List<Record> counted)
        {
            foreach (var el in counted)
            {
                Console.WriteLine("{0} - {1}", el.Character, el.Count);
            }
        }

        public static SortedDictionary<char, int> CountSortedDict(char[] input)
        {
            SortedDictionary<char, int> counted = new SortedDictionary<char, int>();

            foreach (char a in input)
            {
                if (!counted.ContainsKey(a))
                {
                    int count = 0;
                    foreach (char b in input)
                    {
                        if (a.Equals(b))
                        {
                            count++;
                        }
                    }

                    counted.Add(a, count);
                }
            }

            return counted;
        }

        public static List<Record> CountRecords(char[] text)
        {
            List<Record> recs = new List<Record>();
            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];
                int count = 0;
                for (int j = 0; j < text.Length; j++)
                {
                    if (ch.Equals(text[j]))
                    {
                        count++;
                    }
                }

                bool existsFlag = false;
                for (int j = 0; j < recs.Count; j++)
                {
                    Record el = recs[j];
                    if (el.Character.Equals(ch))
                    {
                        existsFlag = true;
                        break;
                    }
                }

                if (!existsFlag)
                {
                    recs.Add(new Record(ch, count));
                }
            }
            return recs;
        }

        public static List<Record> SortRecordsByChar(List<Record> input)
        {
            List<Record> recs = input;
            if (recs.Count > 0)
            {
                List<Record> result = new List<Record>();
                while (recs.Count > 0)
                {
                    Record max = recs[0];
                    for (int i = 0; i < recs.Count; i++)
                    {
                        if (recs[i].Character > max.Character)
                        {
                            max = recs[i];
                        }
                    }
                    recs.Remove(max);
                    result.Add(max);
                }
                result.Reverse();
                return result;
            }
            else
            {
                throw new Exception("list is empty");
            }
        }

        public static List<Record> SortRecordsByNumber(List<Record> input)
        {
            List<Record> recs = input;
            if (recs.Count > 0)
            {
                List<Record> result = new List<Record>();
                while (recs.Count > 0)
                {
                    Record max = recs[0];
                    for (int i = 0; i < recs.Count; i++)
                    {
                        if (recs[i].Count > max.Count)
                        {
                            max = recs[i];
                        }
                    }
                    recs.Remove(max);
                    result.Add(max);
                }
                return result;
            }
            else
            {
                throw new Exception("list is empty");
            }
        }

        public static List<Record> MergeSameLetters(List<Record> recs)
        {
            List<Record> res1 = new List<Record>();
            List<Record> res2 = new List<Record>();
            res1.AddRange(recs);
            if (res1.Count > 0)
            {
                foreach (Record el in res1)
                {
                    if (Char.IsUpper(el.Character))
                    {
                        el.Character = Char.ToLower(el.Character);
                    }
                }
                foreach(Record el in res1)
                {
                    MergingAdd(res2, el);
                }
                
                return res2;
            }
            else
            {
                throw new Exception("list is empty");
            }
        }

        public static List<Record> ConvertToPercentage(List<Record> recs)
        {
            List<Record> result = new List<Record>();
            double total = 0;
            foreach (Record el in recs)
            {
                total += el.Count;
            }
            foreach (Record el in recs)
            {
                result.Add(new Record(el.Character, (el.Count / total) * 100));
            }
            return result;
        }

        public static List<Record> TakeLetters(List<Record> recs)
        {
            List<Record> result = new List<Record>();
            foreach (Record el in recs)
            {
                if (char.IsLetter(el.Character))
                {
                    result.Add(el);
                }
            }
            return result;
        }

        public static List<Record> Normalize(List<Record> recs)
        {
            List<Record> result = new List<Record>();
            result.AddRange(recs);
            result = ConvertToPercentage(
                TakeLetters(
                    SortRecordsByChar(
                        MergeSameLetters(result))));
            return result;
        }

        public static void DrawGraphics(List<Record> recs)
        {
            foreach (Record el in recs)
            {
                Console.Write(el.Character);
                Console.CursorLeft = 3;
                Console.Write("| {0:F2}", el.Count);
                Console.CursorLeft = 10;
                Console.Write("| ");
                for (int i = 0; i < el.Count; i++)
                {
                    Console.Write("█");
                }

                Console.WriteLine();
            }
        }

        public static void DrawGraphics(List<Record> recs, int leftOffset)
        {
            foreach (Record el in recs)
            {
                Console.CursorLeft = leftOffset;
                Console.Write(el.Character);
                Console.CursorLeft = leftOffset + 3;
                Console.Write("| {0:F2}", el.Count);
                Console.CursorLeft = leftOffset + 10;
                Console.Write("| ");
                for (int i = 0; i < el.Count; i++)
                {
                    Console.Write("█");
                }

                Console.WriteLine();
            }
        }

        public static void DrawGraphicsWithExample(List<Record> recs)
        {
            int startY = Console.CursorTop;
            DrawGraphics(Normalize(recs));
            Console.CursorTop = startY;
            DrawGraphics(englLiteralsFreq, 30);
        }

        public static bool CheckLetterFreq(List<Record> recs, double range)
        {
            bool result = false;
            List<Record> exam = Normalize(recs);
            for (int i = 0; i < englLiteralsFreq.Count; i++)
            {
                Record en = englLiteralsFreq[i];
                if (exam[i].Count > en.Count - range && exam[i].Count < en.Count + range)
                {
                    result = true;
                }
                else
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        public static List<Record> GetFreqDifference(List<Record> recs)
        {
            List<Record> diffs = new List<Record>();
            List<Record> exam = Normalize(recs);
            for (int i = 0; i < englLiteralsFreq.Count; i++)
            {
                Record eng = englLiteralsFreq[i];
                Record rec = recs[i];
                diffs.Add(new Record(eng.Character, eng.Count - rec.Count));
            }
            return diffs;
        }

        public static double GetMaxFreqDifference(List<Record> recs)
        {
            List<Record> diffs = GetFreqDifference(recs);
            double result = 0;
            foreach (Record el in recs)
            {
                if (Math.Abs(el.Count) > Math.Abs(result))
                {
                    result = el.Count;
                }
            }
            return result;
        }

        public static int GetBigramsPresence(String text)
        {
            String[] splitted = text.Split(bigrams, StringSplitOptions.None);
            return splitted.Length - 1;
        }

        public static bool CheckEnglish(String text, double threshold)
        {
            int size = text.Length / 2;
            int count = GetBigramsPresence(text);
            return (count / size) * 100 > threshold;
        }

        public static bool CheckEnglish(String text)
        {
            double size = text.Length / 2;
            double count = GetBigramsPresence(text);
            double containment = (count / size) * 100;
            return containment > 0.5;
        }
    }
}
