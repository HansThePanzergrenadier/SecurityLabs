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
                offsets.Add(new CharRecord((char)i, CharRecord.GetMaxFreqDifference(counted)));
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
                offsets.Add(new CharRecord((char)i, CharRecord.GetMaxFreqDifference(counted)));
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

        public static int GetKeyLength(char[] input, int maxKeyLength, double threshold)
        {
            if (maxKeyLength > input.Length)
            {
                maxKeyLength = input.Length;
            }
            List<NumberRecord> coins = GetCriticalCoins(GetCoins(input, maxKeyLength), threshold);
            return (int)coins[0].Key;
        }
    }
}

