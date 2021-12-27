using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    class XSalsa20Breaker
    {
        public static byte[] Xor(byte[] input, byte[] key)
        {
            byte[] output = new byte[input.Length];

            for (int i = 0; i < output.Length; i++)
            {
                output[i] = (byte)(input[i] ^ key[i % key.Length]);
            }

            return output;
        }

        public static void ProceedAnalisys(List<byte[]> ciphers, string str)
        {
            float bestTextPercent = 0;
            List<string> bestText = new List<string>(ciphers.Count);

            for (int first = 0; first < ciphers.Count; first++)
            {
                List<string> currentText = new List<string>(ciphers.Count);

                for (int second = 0; second < ciphers.Count; second++)
                {
                    int minLength = Math.Min(ciphers[first].Length, ciphers[second].Length);
                    byte[] cipherArrayFirst = new byte[minLength];
                    Array.Copy(ciphers[first], cipherArrayFirst, minLength);
                    byte[] cipherArraySecond = new byte[minLength];
                    Array.Copy(ciphers[second], cipherArraySecond, minLength);

                    byte[] xorArr = Xor(cipherArrayFirst, cipherArraySecond);
                    byte[] wordArr = StringToByteArray(str);
                    byte[] resultArr = Xor(xorArr, wordArr);
                    string resultStr = ByteArrayToString(resultArr);
                    currentText.Add(resultStr);
                    Console.WriteLine($"[{first}][{second}]: \t" + resultStr);
                }
                Console.WriteLine();

                StringBuilder stringBuilder = new StringBuilder();
                foreach (string line in currentText)
                {
                    int length = Math.Min(str.Length, line.Length);
                    stringBuilder.Append(line.Substring(0, length));
                }

                float currentTextPercent = GetTextPercent(stringBuilder.ToString());
                if (bestTextPercent < currentTextPercent)
                {
                    bestTextPercent = currentTextPercent;
                    bestText = currentText;
                }
            }

            Console.WriteLine("RESULT:");
            for (int i = 0; i < bestText.Count; i++)
            {
                int length = Math.Min(str.Length, bestText[i].Length);
                Console.Write($"[{i}]:\t");
                //Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(bestText[i].Substring(0, length));
                Console.ResetColor();
                Console.WriteLine(bestText[i].Substring(length));
            }
        }

        public static float GetTextPercent(string str)
        {
            float count = 0;

            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                if ((ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z') || ch == '.' || ch == ',' || ch == ' ' || ch == '\'' || ch == ':' || ch == '-')
                {
                    count++;
                }
            }

            return count / str.Length * 100.0f;
        }

        public static string ByteArrayToString(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }


        public static string ByteArrayToHexString(byte[] byteArr)
        {
            return BitConverter.ToString(byteArr).Replace("-", string.Empty).ToLower();
        }
        

        public static byte[] StringToByteArray(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }


        public static byte[] HexStringToByteArray(string hStr)
        {
            return Enumerable.Range(0, hStr.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hStr.Substring(x, 2), 16))
                             .ToArray();
        }

        
    }


}
