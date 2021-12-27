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

            List<byte[]> lines = new List<byte[]>(text.Count);
            for (int i = 0; i < text.Count; i++)
            {
                lines.Add(XSalsa20Breaker.HexStringToByteArray(text[i]));
            }
            string word = "For who would bear the whips and scorns of time,";

            XSalsa20Breaker.ProceedAnalisys(lines, word);
        }
    }
}
