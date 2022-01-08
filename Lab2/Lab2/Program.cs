using System;
using System.Collections.Generic;
using System.IO;

namespace Lab2
{
    class Program
    {

        static void Main(string[] args)
        {
            List<string> text = new List<string>();

            string filename = "D://Folya//docs//homeworks//7 sem//security//lab2in.txt";
            StreamReader reader = new StreamReader(filename);
            while (!reader.EndOfStream)
            {
                text.Add(reader.ReadLine());
            }

            List<List<string>> results = XorEachOther(text);
            DisplayTexts(XorEveryLineWithKey("The", results));
        }

        static void DisplayTexts(List<List<string>> texts)
        {
            for (int i = 0; i < texts.Count; i++)
            {
                Console.WriteLine($"Line {i}:");
                for (int j = 0; j < texts[i].Count; j++)
                {
                    Console.WriteLine($"{j}: {texts[i][j]}");
                }
                Console.WriteLine("");
            }
        }

        static string XorLine(string longer, string shorter)
        {
            string result = "";

            for (int i = 0, j = 0; i < longer.Length; i++)
            {
                result += (char)(longer[i] ^ shorter[j]);
                j++;
                if (j >= shorter.Length)
                {
                    j = 0;
                }
            }

            return result;
        }

        static List<string> XorOneAndAll(string line, List<string> cipherLines)
        {
            List<string> result = new List<string>();

            foreach (var el in cipherLines)
            {
                if (line.Length > el.Length)
                {
                    result.Add(XorLine(line, el));
                }
                else
                {
                    result.Add(XorLine(el, line));
                }
            }

            return result;
        }

        static List<List<string>> XorEachOther(List<string> cipherLines)
        {
            List<List<string>> result = new List<List<string>>();

            foreach (var el in cipherLines)
            {
                result.Add(XorOneAndAll(el, cipherLines));
            }

            return result;
        }

        static double GetEnglishness(List<string> text)
        {
            int totalNChars = 0;
            foreach (var el in text)
            {
                totalNChars += el.Length;
            }

            int readableNChars = 0;
            foreach (var stringEl in text)
            {
                foreach (var charEl in stringEl)
                {
                    if (char.IsLetter(charEl) || char.IsPunctuation(charEl))
                    {
                        readableNChars++;
                    }
                }
            }

            return (((double)readableNChars) / totalNChars) * 100;
        }

        static List<List<string>> XorEveryLineWithKey(string key, List<List<string>> results)
        {
            List<List<string>> result = new List<List<string>>();

            for (int i = 0; i < results.Count; i++)
            {
                result.Add(XorOneAndAll(key, results[i]));
            }

            return result;
        }
    }
}
