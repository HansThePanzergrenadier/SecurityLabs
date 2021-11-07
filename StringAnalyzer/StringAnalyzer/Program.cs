﻿using System;
using System.Collections.Generic;
using System.IO;

namespace CipherBreaker
{
    class Program
    {
        static void Main(string[] args)
        {
            String filename = "D://Folya//docs//homeworks//7 sem//security//test.txt";
            char[] input;
            bool pasteFlag = false, mergeFlag = true, percFlag = true, advancedOutputFlag = true, testFlag = false;
            Record.SortingMode sortingMode = Record.SortingMode.ByChar;
            StreamReader reader;
            List<Record> counted;


            //if .exe is runned without batfile or console, we should determine filename manually
            if (args.Length == 0)
            {
                Console.WriteLine("Take text from default filepath? y/n");
                if (Console.ReadLine() != "y")
                {
                    do
                    {
                        Console.WriteLine("Enter txt filename to analyze");
                        filename = Console.ReadLine();
                    }
                    while (!File.Exists(filename));
                }

                Console.WriteLine("Test mode? y/n");
                if (Console.ReadLine() == "y")
                {
                    testFlag = true;
                }
                else
                {
                    testFlag = false;


                    Console.WriteLine("Change options? y/n");
                    if (Console.ReadLine() == "y")
                    {
                        Console.WriteLine("Enable merging upper and lower letters? y/n");
                        if (Console.ReadLine() == "y")
                        {
                            mergeFlag = true;
                        }
                        else
                        {
                            mergeFlag = false;
                        }

                        Console.WriteLine("Show results in percentage? y/n");
                        if (Console.ReadLine() == "y")
                        {
                            percFlag = true;
                        }
                        else
                        {
                            percFlag = false;
                        }

                        Console.WriteLine("Select sorting mode");
                        Console.WriteLine("   - enter 'c' to sort by characters");
                        Console.WriteLine("   - enter 'n' to sort by number");
                        Console.WriteLine("   - enter 'x' to disable sorting");
                        switch (Console.ReadLine())
                        {
                            case "c":
                                sortingMode = Record.SortingMode.ByChar;
                                break;
                            case "n":
                                sortingMode = Record.SortingMode.ByNumber;
                                break;
                            case "x":
                                sortingMode = Record.SortingMode.None;
                                break;
                            default:
                                break;
                        }

                        Console.WriteLine("Show advanced output? y/n");
                        if (Console.ReadLine() == "y")
                        {
                            advancedOutputFlag = true;
                        }
                        else
                        {
                            advancedOutputFlag = false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Program will run on default options");
                    }
                }
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

            if (!testFlag)
            {

                //simply count each founded symbol
                counted = Record.CountRecords(input);
                if (mergeFlag)
                {
                    counted = Record.MergeSameLetters(counted);
                }

                switch (sortingMode)
                {
                    case Record.SortingMode.ByChar:
                        counted = Record.SortRecordsByChar(counted);
                        break;
                    case Record.SortingMode.ByNumber:
                        counted = Record.SortRecordsByNumber(counted);
                        break;
                    case Record.SortingMode.None:
                        //nothing to do
                        break;
                    default:
                        counted = Record.SortRecordsByChar(counted);
                        break;
                }

                if (percFlag)
                {
                    counted = Record.ConvertToPercentage(counted);
                }

                //display data
                if (advancedOutputFlag)
                {
                    Record.DrawGraphicsWithExample(counted);
                }
                else
                {
                    Record.DrawGraphics(counted);
                }
            }
            else
            {
                //there is some to test
                Console.WriteLine(new string(Decipher.AttackCaesarXor(input)));
            }

            Console.ReadKey();
        }
    }
}
