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

            DisplayTexts(XorEachOther(text));
            
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
            
            for(int i = 0, j = 0; i < longer.Length; i++)
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

            foreach(var el in cipherLines)
            {
                if(line.Length > el.Length)
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

        
    }
}
