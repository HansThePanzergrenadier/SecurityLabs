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
            List<Record> offsets = new List<Record>();
            for (int i = 0; i < 256; i++)
            {
                char[] result = CaesarAdd(input, i);
                List<Record> counted = Record.CountRecords(result);
                offsets.Add(new Record((char)i, Record.GetMaxFreqDifference(counted)));
            }
            Record.DrawGraphics(offsets);
        }

        public static void ShowCaesarXor(char[] input)
        {
            List<Record> offsets = new List<Record>();
            for (int i = 0; i < 256; i++)
            {
                char[] result = CaesarAdd(input, i);
                List<Record> counted = Record.CountRecords(result);
                offsets.Add(new Record((char)i, Record.GetMaxFreqDifference(counted)));
            }
            Record.DrawGraphics(offsets);
        }

        public static char[] AttackCaesarXor(char[] text)
        {
            double[] press = new double[256];
            double maxPresence = 0;
            int maxPresIndex = 0;
            for(int i = 0; i < 256; i++)
            {
                char[] result = CaesarXor(text, i);
                press[i] = Record.GetBigramsPresence(new string(result));
                if(press[i] > maxPresence)
                {
                    maxPresence = press[i];
                    maxPresIndex = i;
                }
            }
            return CaesarXor(text, maxPresIndex);
        }
    }
}
