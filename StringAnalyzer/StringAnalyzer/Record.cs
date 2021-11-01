using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringAnalyzer
{
    class Record
    {
        public char Character;
        public double Count;

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

        public static void MergeSameLetters(List<Record> recs)
        {
            if (recs.Count > 0)
            {
                for (int i = 0; i < recs.Count; i++)
                {
                    Record a = recs[i];
                    char aC = a.Character;
                    if (Char.IsLetter(aC) && Char.IsUpper(aC))
                    {
                        for (int j = 0; j < recs.Count; j++)
                        {
                            Record b = recs[j];
                            Char bC = b.Character;
                            if (Char.IsLetter(bC) && Char.IsLower(bC) && Char.ToUpper(bC).Equals(aC))
                            {
                                a.Count += b.Count;
                                recs.Remove(b);
                            }
                        }
                    }
                }
                foreach(Record el in recs)
                {
                    if (Char.IsUpper(el.Character))
                    {
                        el.Character = Char.ToLower(el.Character);
                    }
                }
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
            foreach(Record el in recs)
            {
                result.Add(new Record(el.Character, (el.Count / total) * 100));
            }
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
    }
}
