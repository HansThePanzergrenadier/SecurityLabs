using System;
using System.Collections.Generic;
using System.IO;

namespace CypherBreaker
{
    class Program
    {
        static void Main(string[] args)
        {
            String filename;
            String cypherMode;
            char[] input;
            bool pasteFlag = false;
            StreamReader reader;


            //if .exe is runned without batfile or console, we should determine filename manually
            if (args.Length == 0)
            {
                do
                {
                    Console.WriteLine("Enter txt filename to attack or: ");
                    Console.WriteLine("   - enter 'q' to close program ");
                    Console.WriteLine("   - enter 't' to paste complete text to attack ");
                    filename = Console.ReadLine();
                    if (filename == "q")
                    {
                        return;
                    }
                    else if (filename == "t")
                    {
                        pasteFlag = true;
                        break;
                    }
                }
                while (!File.Exists(filename));
                
                if (!pasteFlag)
                    Console.WriteLine("File found, beginning analysis...");
            }
            else
            {
                filename = args.ToString();
            }

            //we can paste whole text just in console as well
            if (!pasteFlag)
            {
                reader = new StreamReader(filename);
                input = reader.ReadToEnd().ToCharArray();
            }
            else
            {
                Console.WriteLine("Paste your text as a single line: ");
                input = Console.ReadLine().ToCharArray();
            }

            Console.WriteLine("Select cypher type: ");
            Console.WriteLine("   - 'c' - ceasar");

            cypherMode = Console.ReadLine();

            
            switch (cypherMode)
            {
                case "c":
                    Decypher.ShowCaesarXor(input);
                    break;
                default:
                    break;
            }
            

            Console.ReadKey();
        }


    }
}
