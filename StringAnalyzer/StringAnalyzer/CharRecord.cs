using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherBreaker
{
    public class CharRecord 
    {
        public char Character;
        public double Count;

        

        public static string[] bigrams = new string[]
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
            "oo"
        };
        public static List<CharRecord> englLiteralsFreq = new List<CharRecord>()
        {
            new CharRecord('a', 8.167),
            new CharRecord('b', 1.492),
            new CharRecord('c', 2.782),
            new CharRecord('d', 4.253),
            new CharRecord('e', 12.702),
            new CharRecord('f', 2.228),
            new CharRecord('g', 2.015),
            new CharRecord('h', 6.094),
            new CharRecord('i', 6.966),
            new CharRecord('j', 0.153),
            new CharRecord('k', 0.772),
            new CharRecord('l', 4.025),
            new CharRecord('m', 2.406),
            new CharRecord('n', 6.749),
            new CharRecord('o', 7.507),
            new CharRecord('p', 1.929),
            new CharRecord('q', 0.095),
            new CharRecord('r', 5.987),
            new CharRecord('s', 6.327),
            new CharRecord('t', 9.056),
            new CharRecord('u', 2.758),
            new CharRecord('v', 0.978),
            new CharRecord('w', 2.360),
            new CharRecord('x', 0.150),
            new CharRecord('y', 1.974),
            new CharRecord('z', 0.074)
        };

        public static List<CharRecord> englLiteralsFreqCap = new List<CharRecord>()
        {
            new CharRecord('A', 8.167),
            new CharRecord('B', 1.492),
            new CharRecord('C', 2.782),
            new CharRecord('D', 4.253),
            new CharRecord('E', 12.702),
            new CharRecord('F', 2.228),
            new CharRecord('G', 2.015),
            new CharRecord('H', 6.094),
            new CharRecord('I', 6.966),
            new CharRecord('J', 0.153),
            new CharRecord('K', 0.772),
            new CharRecord('L', 4.025),
            new CharRecord('M', 2.406),
            new CharRecord('N', 6.749),
            new CharRecord('O', 7.507),
            new CharRecord('P', 1.929),
            new CharRecord('Q', 0.095),
            new CharRecord('R', 5.987),
            new CharRecord('S', 6.327),
            new CharRecord('T', 9.056),
            new CharRecord('U', 2.758),
            new CharRecord('V', 0.978),
            new CharRecord('W', 2.360),
            new CharRecord('X', 0.150),
            new CharRecord('Y', 1.974),
            new CharRecord('Z', 0.074)
        };

        public static List<CharRecord> englLiteralsFreqAll = new List<CharRecord>()
        {
            new CharRecord('A', 8.167),
            new CharRecord('B', 1.492),
            new CharRecord('C', 2.782),
            new CharRecord('D', 4.253),
            new CharRecord('E', 12.702),
            new CharRecord('F', 2.228),
            new CharRecord('G', 2.015),
            new CharRecord('H', 6.094),
            new CharRecord('I', 6.966),
            new CharRecord('J', 0.153),
            new CharRecord('K', 0.772),
            new CharRecord('L', 4.025),
            new CharRecord('M', 2.406),
            new CharRecord('N', 6.749),
            new CharRecord('O', 7.507),
            new CharRecord('P', 1.929),
            new CharRecord('Q', 0.095),
            new CharRecord('R', 5.987),
            new CharRecord('S', 6.327),
            new CharRecord('T', 9.056),
            new CharRecord('U', 2.758),
            new CharRecord('V', 0.978),
            new CharRecord('W', 2.360),
            new CharRecord('X', 0.150),
            new CharRecord('Y', 1.974),
            new CharRecord('Z', 0.074),
            new CharRecord('a', 8.167),
            new CharRecord('b', 1.492),
            new CharRecord('c', 2.782),
            new CharRecord('d', 4.253),
            new CharRecord('e', 12.702),
            new CharRecord('f', 2.228),
            new CharRecord('g', 2.015),
            new CharRecord('h', 6.094),
            new CharRecord('i', 6.966),
            new CharRecord('j', 0.153),
            new CharRecord('k', 0.772),
            new CharRecord('l', 4.025),
            new CharRecord('m', 2.406),
            new CharRecord('n', 6.749),
            new CharRecord('o', 7.507),
            new CharRecord('p', 1.929),
            new CharRecord('q', 0.095),
            new CharRecord('r', 5.987),
            new CharRecord('s', 6.327),
            new CharRecord('t', 9.056),
            new CharRecord('u', 2.758),
            new CharRecord('v', 0.978),
            new CharRecord('w', 2.360),
            new CharRecord('x', 0.150),
            new CharRecord('y', 1.974),
            new CharRecord('z', 0.074)
        };

        public enum SortingMode
        {
            ByNumber,
            ByChar,
            None
        }

        public CharRecord(char Character, double Count)
        {
            this.Character = Character;
            this.Count = Count;
        }

        public static bool Contains(List<CharRecord> recs, char symbol)
        {
            foreach (var el in recs)
            {
                if (el.Character.Equals(symbol))
                {
                    return true;
                }
            }
            return false;
        }

        public static void MergingAdd(List<CharRecord> recs, CharRecord rec)
        {
            if (Contains(recs, rec.Character))
            {
                for (int i = 0; i < recs.Count; i++)
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

        public static void DrawSimple(List<CharRecord> counted)
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

        public static List<CharRecord> CountRecords(char[] text)
        {
            List<CharRecord> recs = new List<CharRecord>();
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
                    CharRecord el = recs[j];
                    if (el.Character.Equals(ch))
                    {
                        existsFlag = true;
                        break;
                    }
                }

                if (!existsFlag)
                {
                    recs.Add(new CharRecord(ch, count));
                }
            }
            return recs;
        }

        public static List<CharRecord> SortRecordsByChar(List<CharRecord> input)
        {
            List<CharRecord> recs = input;
            if (recs.Count > 0)
            {
                List<CharRecord> result = new List<CharRecord>();
                while (recs.Count > 0)
                {
                    CharRecord max = recs[0];
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

        public static List<CharRecord> SortRecordsByNumber(List<CharRecord> input)
        {
            List<CharRecord> recs = input;
            if (recs.Count > 0)
            {
                List<CharRecord> result = new List<CharRecord>();
                while (recs.Count > 0)
                {
                    CharRecord max = recs[0];
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

        public static CharRecord GetMaxRecord(List<CharRecord> recs)
        {
            CharRecord max = new CharRecord('.', double.MinValue);
            foreach (CharRecord el in recs)
            {
                if (el.Count > max.Count)
                {
                    max = el;
                }
            }
            return max;
        }

        public static CharRecord GetMinRecord(List<CharRecord> recs)
        {
            CharRecord min = new CharRecord('.', double.MaxValue);
            foreach (CharRecord el in recs)
            {
                if (el.Count < min.Count)
                {
                    min = el;
                }
            }
            return min;
        }

        public static List<CharRecord> MergeSameLetters(List<CharRecord> recs)
        {
            List<CharRecord> res1 = new List<CharRecord>();
            List<CharRecord> res2 = new List<CharRecord>();
            res1.AddRange(recs);
            if (res1.Count > 0)
            {
                foreach (CharRecord el in res1)
                {
                    if (Char.IsUpper(el.Character))
                    {
                        el.Character = Char.ToLower(el.Character);
                    }
                }
                foreach (CharRecord el in res1)
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

        public static List<CharRecord> ConvertToPercentage(List<CharRecord> recs)
        {
            List<CharRecord> result = new List<CharRecord>();
            double total = 0;
            foreach (CharRecord el in recs)
            {
                total += el.Count;
            }
            foreach (CharRecord el in recs)
            {
                result.Add(new CharRecord(el.Character, (el.Count / total) * 100));
            }
            return result;
        }

        public static List<CharRecord> TakeLetters(List<CharRecord> recs)
        {
            List<CharRecord> result = new List<CharRecord>();
            foreach (CharRecord el in recs)
            {
                if (char.IsLetter(el.Character))
                {
                    result.Add(el);
                }
            }
            return result;
        }

        public static List<CharRecord> AddMissingLetters(List<CharRecord> recs)
        {
            List<CharRecord> result = new List<CharRecord>();
            result.AddRange(recs);
            foreach (CharRecord el in englLiteralsFreq)
            {
                if (!Contains(result, el.Character))
                {
                    result.Add(new CharRecord(el.Character, 0));
                }
            }
            return result;
        }

        public static List<CharRecord> AddAllMissingLetters(List<CharRecord> recs)
        {
            List<CharRecord> result = new List<CharRecord>();
            result.AddRange(recs);
            foreach (CharRecord el in englLiteralsFreqAll)
            {
                if (!Contains(result, el.Character))
                {
                    result.Add(new CharRecord(el.Character, 0));
                }
            }
            return result;
        }

        public static List<CharRecord> Normalize(List<CharRecord> recs)
        {
            List<CharRecord> result = new List<CharRecord>();
            result.AddRange(recs);
            result = ConvertToPercentage(
                TakeLetters(
                    SortRecordsByChar(
                        AddMissingLetters(
                            MergeSameLetters(result)))));
            return result;
        }

        public static List<CharRecord> NormalizeAllRegs(List<CharRecord> recs)
        {
            List<CharRecord> result = new List<CharRecord>();
            result.AddRange(recs);
            result = TakeLetters(
                        ConvertToPercentage(
                            SortRecordsByChar(
                                AddAllMissingLetters(result))));
            return result;
        }

        public static void DrawGraphics(List<CharRecord> recs)
        {
            foreach (CharRecord el in recs)
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

        public static void DrawGraphics(List<CharRecord> recs, int leftOffset)
        {
            foreach (CharRecord el in recs)
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

        public static void DrawGraphicsWithExample(List<CharRecord> recs)
        {
            int startY = Console.CursorTop;
            DrawGraphics(Normalize(recs));
            Console.CursorTop = startY;
            DrawGraphics(englLiteralsFreq, 30);
        }

        public static void DrawGraphicsWithExampleAll(List<CharRecord> recs)
        {
            int startY = Console.CursorTop;
            DrawGraphics(NormalizeAllRegs(recs));
            Console.CursorTop = startY;
            DrawGraphics(englLiteralsFreqAll, 30);
        }

        /*
         * check if english using frequency analysis
         * recs - usually it`s what Normalize() returns
         * range - threshold in percents. 5% is good enough
         */
        public static bool CheckLetterFreq(List<CharRecord> recs, double range)
        {
            bool result = false;
            //List<CharRecord> exam = Normalize(recs);
            List<CharRecord> exam = recs;
            for (int i = 0; i < englLiteralsFreq.Count; i++)
            {
                CharRecord en = englLiteralsFreq[i];
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

        /*
         * returns a list of differences in percents between original text letters frequencies and english ones
         * recs - usually it`s what Normalize() returns
         */
        public static List<CharRecord> GetFreqDifference(List<CharRecord> recs)
        {
            List<CharRecord> diffs = new List<CharRecord>();
            //List<CharRecord> exam = Normalize(recs);
            List<CharRecord> exam = recs;
            for (int i = 0; i < englLiteralsFreq.Count; i++)
            {
                CharRecord eng = englLiteralsFreq[i];
                CharRecord rec = exam[i];
                diffs.Add(new CharRecord(eng.Character, eng.Count - rec.Count));
            }
            return diffs;
        }

        public static List<CharRecord> GetFreqDifferenceAll(List<CharRecord> recs)
        {
            List<CharRecord> diffs = new List<CharRecord>();
            //List<CharRecord> exam = Normalize(recs);
            List<CharRecord> exam = recs;
            for (int i = 0; i < englLiteralsFreqAll.Count; i++)
            {
                CharRecord eng = englLiteralsFreqAll[i];
                CharRecord rec = exam[i];
                diffs.Add(new CharRecord(eng.Character, eng.Count - rec.Count));
            }
            return diffs;
        }

        /*
         * returns a max difference in percents between original text letters frequencies and english ones
         * recs - usually it`s what Normalize() returns
         */
        public static double GetMaxFreqDifference(List<CharRecord> recs)
        {
            List<CharRecord> diffs = GetFreqDifference(recs);
            double result = 0;
            foreach (CharRecord el in diffs)
            {
                if (Math.Abs(el.Count) > Math.Abs(result))
                {
                    result = el.Count;
                }
            }
            return result;
        }

        public static double GetAveFreqDifferenceAll(List<CharRecord> recs)
        {
            List<CharRecord> diffs = GetFreqDifferenceAll(recs);
            double sum = 0;
            foreach (CharRecord el in diffs)
            {
                sum += el.Count;
            }
            return sum/diffs.Count;
        }

        public static double GetBigramsPresence(string text)
        {
            double size = text.Length / 2;
            string[] splitted = text.Split(bigrams, StringSplitOptions.None);
            double count = splitted.Length - 1;
            return (count / size) * 100;
        }

        public static bool CheckEnglish(string text, double threshold)
        {
            return GetBigramsPresence(text) > threshold;
        }

        /*
         * returns shifted list of letter frequencies
         * freqs - it`s usually what Normalize() returns
         */
        public static List<CharRecord> MoveFrequenciesBy(List<CharRecord> freqs, int offset)
        {
            List<CharRecord> moved = new List<CharRecord>();
            List<CharRecord> inter = freqs;
            for (int i = 0, j = 0; i < freqs.Count; i++)
            {
                if (i + offset < freqs.Count)
                {
                    moved.Add(inter[i + offset]);
                }
                else
                {
                    moved.Add(inter[j]);
                    j++;
                }
            }
            return moved;
        }

        public static double GetDifference(CharRecord a, CharRecord b)
        {
            return Math.Abs(a.Count - b.Count);
        }

        //doesnt work with exchange cipher
        public static CharRecord FindNearestExchange(CharRecord input, List<CharRecord> alphabet)
        {
            double minDiff = double.MaxValue;
            int minIndex = -1;
            for (int i = 0; i < alphabet.Count; i++)
            {
                CharRecord el = alphabet[i];
                if (minDiff > GetDifference(el, input))
                {
                    minDiff = GetDifference(el, input);
                    minIndex = i;
                }
            }
            return alphabet[minIndex];
        }

        public static char[] GetLowerChars(char[] input)
        {
            char[] lower = new char[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                lower[i] = char.ToLower(input[i]);
            }
            return lower;
        }
    }
}