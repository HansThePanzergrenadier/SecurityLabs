using System;
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
            CharRecord.SortingMode sortingMode = CharRecord.SortingMode.ByChar;
            StreamReader reader;
            List<CharRecord> counted;


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
                                sortingMode = CharRecord.SortingMode.ByChar;
                                break;
                            case "n":
                                sortingMode = CharRecord.SortingMode.ByNumber;
                                break;
                            case "x":
                                sortingMode = CharRecord.SortingMode.None;
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
                counted = CharRecord.CountRecords(input);
                if (mergeFlag)
                {
                    counted = CharRecord.MergeSameLetters(counted);
                }

                switch (sortingMode)
                {
                    case CharRecord.SortingMode.ByChar:
                        counted = CharRecord.SortRecordsByChar(counted);
                        break;
                    case CharRecord.SortingMode.ByNumber:
                        counted = CharRecord.SortRecordsByNumber(counted);
                        break;
                    case CharRecord.SortingMode.None:
                        //nothing to do
                        break;
                    default:
                        counted = CharRecord.SortRecordsByChar(counted);
                        break;
                }

                if (percFlag)
                {
                    counted = CharRecord.ConvertToPercentage(counted);
                }

                //display data
                if (advancedOutputFlag)
                {
                    CharRecord.DrawGraphicsWithExample(counted);
                }
                else
                {
                    CharRecord.DrawGraphics(counted);
                }
            }
            else
            {
                //there is some to test
                //Console.WriteLine("Key: " + Decipher.GetXorKey(Decipher.DecodeFromBase64(input)));
                //Console.WriteLine(Decipher.BreakXorVigenere(Decipher.DecodeFromBase64(input)));
                //CharRecord.DrawGraphicsWithExampleAll(CharRecord.CountRecords(Decipher.SplitIntoGroups(Decipher.DecodeFromBase64(input), 3)[1]));
                foreach (var el in Decipher.GetXorKeyCandidates(Decipher.DecodeFromBase64(input)))
                {
                    Console.WriteLine(el);
                }
            }

            Console.ReadKey();
        }
    }
}
