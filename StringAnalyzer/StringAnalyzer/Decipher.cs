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
            for(int i = 0; i < 256; i++)
            {
                char[] result = CaesarXor(text, i);
                if(Record.CheckEnglish(new string(result)))
                {
                    return result;
                }
            }
            throw new Exception("Failed to attack");
        }
    }
}
