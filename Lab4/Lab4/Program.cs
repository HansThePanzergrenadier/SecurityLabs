using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace PasswordGenerator
{
    class Program
    {
        public static void Main(string[] args)
        {

        }

        static string stupid100Path = "D://Folya//docs//homeworks//7 sem//security//Lab4StupidPass.txt";
        static string stupid100kPath = "D://Folya//docs//homeworks//7 sem//security//Lab4StupidPass100k.txt";
        static List<string> StupidPass100 = FromFile(stupid100Path);
        static List<string> StupidPass100k = FromFile(stupid100kPath);
        static List<string> TrulyRandom;

        static List<string> FromFile(string path)
        {
            StreamReader reader = new StreamReader(path);
            List<string> result = new List<string>();


            while (!reader.EndOfStream)
            {
                result.Add(reader.ReadLine().Trim());
            }

            return result;
        }

        static List<string> GenTrulyRandom(int count, int length)
        {
            List<string> result = new List<string>();

            for (int i = 0; i < count; i++)
            {
                string pass = "";
                for (int j = 0; j < length; j++)
                {
                    pass += (char)RandomNumberGenerator.GetInt32(33, 127);
                }
                result.Add(pass);
            }

            return result;
        }

        static List<string> GenCombinedStupid(List<string> vocab, int count)
        {
            List<string> result = new List<string>();

            for (int i = 0; i < count; i++)
            {
                string pass = "";
                int index1 = RandomNumberGenerator.GetInt32(vocab.Count);
                int index2 = RandomNumberGenerator.GetInt32(vocab.Count);

                pass = vocab[index1] + vocab[index2];
                result.Add(pass);
            }

            return result;
        }
    }
}
