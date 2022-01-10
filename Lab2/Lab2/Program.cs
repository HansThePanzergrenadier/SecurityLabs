using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

            List<byte[]> converted = ConvertFromHex(text);
            List<List<byte[]>> results = XorEachOther(converted);
            byte[] key = Encoding.UTF8.GetBytes("The undiscovered ");
            List<List<byte[]>> keyed = XorEveryLineWithKey(key, results);
            DisplayTexts(keyed);
        }

        static void DisplayTexts(List<List<byte[]>> texts)
        {
            for (int i = 0; i < texts.Count; i++)
            {
                Console.WriteLine($"Line {i}:");
                for (int j = 0; j < texts[i].Count; j++)
                {
                    string line = Encoding.UTF8.GetString(texts[i][j]);
                    Console.WriteLine($"{j}: {line}");
                }
                Console.WriteLine("");
            }
        }

        static byte[] XorLine(byte[] longer, byte[] shorter)
        {
            byte[] result = new byte[longer.Length];

            for (int i = 0, j = 0; i < longer.Length; i++)
            {
                result[i] = (byte)(longer[i] ^ shorter[j]);
                j++;
                if (j >= shorter.Length)
                {
                    j = 0;
                }
            }

            return result;
        }

        static List<byte[]> XorOneAndAll(byte[] line, List<byte[]> cipherLines)
        {
            List<byte[]> result = new List<byte[]>();

            foreach (var el in cipherLines)
            {
                int lineLength = Math.Min(el.Length, line.Length);
                byte[] lineCut = new byte[lineLength];
                byte[] elCut = new byte[lineLength];
                Array.Copy(line, lineCut, lineLength);
                Array.Copy(el, elCut, lineLength);
                result.Add(XorLine(lineCut, elCut));
            }

            return result;
        }

        static List<List<byte[]>> XorEachOther(List<byte[]> cipherLines)
        {
            List<List<byte[]>> result = new List<List<byte[]>>();

            foreach (var el in cipherLines)
            {
                result.Add(XorOneAndAll(el, cipherLines));
            }

            return result;
        }

        static List<byte[]> XorOneKeyAndLines(byte[] key, List<byte[]> cipherLines)
        {
            List<byte[]> result = new List<byte[]>();

            foreach (var el in cipherLines)
            {
                result.Add(XorLine(el, key));
            }

            return result;
        }

        static List<List<byte[]>> XorEveryLineWithKey(byte[] key, List<List<byte[]>> results)
        {
            List<List<byte[]>> result = new List<List<byte[]>>();

            for (int i = 0; i < results.Count; i++)
            {
                result.Add(XorOneKeyAndLines(key, results[i]));
            }

            return result;
        }

        static List<byte[]> ConvertFromHex(List<string> hexLines)
        {
            List<byte[]> result = new List<byte[]>();

            foreach (var el in hexLines)
            {
                byte[] newLine = new byte[el.Length / 2];
                for (int i = 0, j = 0; i < el.Length; i += 2, j++)
                {
                    newLine[j] = Convert.ToByte(string.Concat(el[i], el[i + 1]), 16);
                }
                result.Add(newLine);
            }

            return result;
        }
    }
}
