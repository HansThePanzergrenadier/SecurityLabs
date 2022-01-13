using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;

namespace PasswordGenerator
{
    class Program
    {
        public static void Main(string[] args)
        {
            //ToFile(GeneratePasswords(100000, 10, 60, 5), input100kPass);

            ToFile(HashWeak(FromFile(input100kPass)), outputWeakPath);
            //ToFile(HashStrong(FromFile(input100kPass)), outputStrongPath);
        }

        static string outputStrongPath = "D://Folya//docs//homeworks//7 sem//security//Lab4OutStrong.txt";
        static string outputWeakPath = "D://Folya//docs//homeworks//7 sem//security//Lab4OutWeak.txt";
        static string stupid100Path = "D://Folya//docs//homeworks//7 sem//security//Lab4StupidPass.txt";
        static string stupid100kPath = "D://Folya//docs//homeworks//7 sem//security//Lab4StupidPass100k.txt";
        static string input100kPass = "D://Folya//docs//homeworks//7 sem//security//Lab4Pass100k.txt";
        static List<string> StupidPass100 = FromFile(stupid100Path);
        static List<string> StupidPass100k = FromFile(stupid100kPath);
        static List<string> TrulyRandom;
        static List<string> Combined;

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

        static void ToFile(List<string> output, string path)
        {
            StreamWriter writer = new StreamWriter(path);

            foreach (var el in output)
            {
                writer.WriteLine(el);
            }

            writer.Close();
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


        static List<string> GeneratePasswords(int count, int ultraStupidPercent, int stupidPercent, int securePercent)
        {
            List<string> result = new List<string>();
            int combinedPercent = 100 - (ultraStupidPercent + stupidPercent + securePercent);
            TrulyRandom = GenTrulyRandom((int)(count * ((double)securePercent / 100) * 1.5), 8);
            Combined = GenCombinedStupid(StupidPass100k, (int)(count * ((double)combinedPercent / 100) * 1.5));
            int c = 0, d = 0;

            for (int i = 0; i < count; i++)
            {
                int currentSelection = RandomNumberGenerator.GetInt32(100);

                if (currentSelection < ultraStupidPercent)
                {
                    int a = RandomNumberGenerator.GetInt32(StupidPass100.Count);
                    result.Add(StupidPass100[a]);
                }
                else if (currentSelection >= ultraStupidPercent && currentSelection < ultraStupidPercent + stupidPercent)
                {
                    int a = RandomNumberGenerator.GetInt32(StupidPass100k.Count);
                    result.Add(StupidPass100k[a]);
                }
                else if (currentSelection >= ultraStupidPercent + stupidPercent && currentSelection < ultraStupidPercent + stupidPercent + securePercent)
                {
                    result.Add(TrulyRandom[c]);
                    c++;
                }
                else
                {
                    result.Add(Combined[d]);
                    d++;
                }
            }

            return result;
        }

        static List<string> HashStrong(List<string> passwords)
        {
            List<string> result = new List<string>();

            //
            for(int i = 0; i < passwords.Count; i++)
            {
                //
            }

            return result;
        }

        static List<string> HashWeak(List<string> passwords)
        {
            List<string> result = new List<string>();

            //
            for (int i = 0; i < passwords.Count; i++)
            {
                //
            }

            return result;
        }
    }
}
