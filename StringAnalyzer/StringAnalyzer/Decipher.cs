using System;
using System.Collections.Generic;

namespace CipherBreaker
{
    class Decipher
    {
        public static char[] CaesarAdd(char[] input, int offset)
        {
            char[] result = new char[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = (char)(input[i] + offset);
            }
            return result;
        }

        public static char[] CaesarXor(char[] input, int offset)
        {
            char[] result = new char[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = (char)(input[i] ^ offset);
            }
            return result;
        }

        public static void ShowCaesarAdd(char[] input)
        {
            List<CharRecord> offsets = new List<CharRecord>();
            for (int i = 0; i < 256; i++)
            {
                char[] result = CaesarAdd(input, i);
                List<CharRecord> counted = CharRecord.CountRecords(result);
                offsets.Add(new CharRecord((char)i, CharRecord.GetMaxFreqDifference(CharRecord.Normalize(counted))));
            }
            CharRecord.DrawGraphics(offsets);
        }

        public static void ShowCaesarXor(char[] input)
        {
            List<CharRecord> offsets = new List<CharRecord>();
            for (int i = 0; i < 256; i++)
            {
                char[] result = CaesarAdd(input, i);
                List<CharRecord> counted = CharRecord.CountRecords(result);
                offsets.Add(new CharRecord((char)i, CharRecord.GetMaxFreqDifference(CharRecord.Normalize(counted))));
            }
            CharRecord.DrawGraphics(offsets);
        }

        public static char[] AttackCaesarXor(char[] text)
        {
            double[] press = new double[256];
            double maxPresence = 0;
            int maxPresIndex = 0;
            for (int i = 0; i < 256; i++)
            {
                char[] result = CaesarXor(text, i);
                press[i] = CharRecord.GetBigramsPresence(new string(result));
                if (press[i] > maxPresence)
                {
                    maxPresence = press[i];
                    maxPresIndex = i;
                }
            }
            return CaesarXor(text, maxPresIndex);
        }

        public static char[] MoveBy(char[] input, int offset)
        {
            char[] result = new char[input.Length];
            for (int i = 0, j = 0; i < input.Length; i++)
            {
                if (i + offset < result.Length)
                {
                    result[i + offset] = input[i];
                }
                else
                {
                    result[j] = input[i];
                    j++;
                }
            }
            return result;
        }

        public static List<NumberRecord> GetCoins(char[] input, int maxKeyLength)
        {
            List<NumberRecord> coins = new List<NumberRecord>();
            for (int i = 1; i < maxKeyLength; i++)
            {
                char[] moved = MoveBy(input, i);
                double coinCounter = 0;
                for (int j = 0; j < input.Length; j++)
                {
                    if (input[j].Equals(moved[j]))
                    {
                        coinCounter++;
                    }
                }
                coins.Add(new NumberRecord(i, (coinCounter / input.Length) * 100));
            }
            return coins;
        }

        public static List<NumberRecord> GetCriticalCoins(List<NumberRecord> coins, double threshold)
        {
            List<NumberRecord> critCoins = new List<NumberRecord>();
            double critThreshold = threshold;
            for (int i = 1; i < coins.Count - 1; i++)
            {
                double mid = coins[i].Count;
                double left = coins[i - 1].Count;
                double right = coins[i + 1].Count;
                if (mid - left > critThreshold && mid - right > critThreshold)
                {
                    critCoins.Add(coins[i]);
                }
            }
            return critCoins;
        }

        /*
         * threshold - in percents. 5% is OK
         */
        public static int GetKeyLength(char[] input, int maxKeyLength, double threshold)
        {
            if (maxKeyLength > input.Length)
            {
                maxKeyLength = input.Length;
            }
            List<NumberRecord> coins = GetCriticalCoins(GetCoins(input, maxKeyLength), threshold);
            return (int)coins[0].Key;
        }

        public static List<char[]> SplitIntoGroups(char[] text, int ngroups)
        {
            List<List<char>> groups = new List<List<char>>();
            for (int i = 0; i < ngroups; i++)
            {
                int j = i;
                groups.Add(new List<char>());
                for (; j < text.Length; j += ngroups)
                {
                    groups[i].Add(text[j]);
                }
            }
            List<char[]> result = new List<char[]>();
            foreach(var el in groups)
            {
                result.Add(el.ToArray());
            }
            return result;
        }

        public static char GetSummingKeyLetter(char[] group)
        {
            List<CharRecord> normalized = CharRecord.Normalize(CharRecord.CountRecords(group));
            double[] maxDiffs = new double[256];
            for(int i = 0; i < 256; i++)
            {
                maxDiffs[i] = CharRecord.GetMaxFreqDifference(CharRecord.MoveFrequenciesBy(normalized, i));
            }
            int minDiffIndex = -1;
            double minDiff = double.MaxValue;
            for(int i = 0; i < maxDiffs.Length; i++)
            {
                if(Math.Abs(maxDiffs[i]) < Math.Abs(minDiff))
                {
                    minDiff = maxDiffs[i];
                    minDiffIndex = i;
                }
            }
            return Convert.ToChar(minDiffIndex);
        }

        public static string GetSummingKey(char[] text)
        {
            string result = "";
            List<char[]> groups = SplitIntoGroups(text, GetKeyLength(text, 20, 5));
            foreach(var el in groups)
            {
                result += GetSummingKeyLetter(el);
            }
            return result;
        }

        public static char GetXorKeyLetter(char[] group)
        {
            double[] maxDiffs = new double[256];
            for (int i = 0; i < 256; i++)
            {
                maxDiffs[i] = 
                    CharRecord.GetMaxFreqDifference(
                        CharRecord.Normalize(
                            CharRecord.CountRecords(
                                CaesarXor(group, i))));
            }
            int minDiffIndex = -1;
            double minDiff = double.MaxValue;
            for (int i = 0; i < maxDiffs.Length; i++)
            {
                if (Math.Abs(maxDiffs[i]) < Math.Abs(minDiff))
                {
                    minDiff = maxDiffs[i];
                    minDiffIndex = i;
                }
            }
            return Convert.ToChar(minDiffIndex);
        }

        public static string GetXorKey(char[] text)
        {
            string result = "";
            List<char[]> groups = SplitIntoGroups(text, GetKeyLength(text, 20, 5));
            foreach (var el in groups)
            {
                result += GetXorKeyLetter(el);
            }
            return result;
        }

        public static char[] DecryptByXorKey(char[] text, string key)
        {
            char[] result = new char[text.Length];
            char[] keyArr = key.ToCharArray();
            for(int i = 0, j = 0; i < text.Length; i++, j++)
            {
                if(j >= keyArr.Length)
                {
                    j = 0;
                }
                result[i] = (char)(text[i] ^ keyArr[j]);
            }
            return result;
        }

        public static char[] BreakXorVigenere(char[] text)
        {
            return DecryptByXorKey(text, "L0l");
        }

        public static char[] DecodeFromBase64(char[] text)
        {
            byte[] converted = Convert.FromBase64CharArray(text, 0, text.Length);
            char[] result = new char[converted.Length];
            for(int i = 0; i < converted.Length; i++)
            {
                result[i] = (char)converted[i];
            }
            return result;
        }
    }
}

