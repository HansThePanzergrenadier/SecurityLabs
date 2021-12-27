using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithm = GeneticSharp.Domain.GeneticAlgorithm;

namespace CipherBreaker
{
    class Decipher
    {
        public static char[] CaesarAdd(char[] input, int offset)
        {
            char[] result = new char[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = (char)(input[i] + offset);
            }
            return result;
        }

        public static char[] CaesarXor(char[] input, int offset)
        {
            char[] result = new char[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = (char)(input[i] ^ offset);
            }
            return result;
        }

        public static void ShowCaesarAdd(char[] input)
        {
            List<CharRecord> offsets = new List<CharRecord>();
            for (int i = 0; i < 256; i++)
            {
                char[] result = CaesarAdd(input, i);
                List<CharRecord> counted = CharRecord.CountRecords(result);
                offsets.Add(new CharRecord((char)i, CharRecord.GetMaxFreqDifference(CharRecord.Normalize(counted))));
            }
            CharRecord.DrawGraphics(offsets);
        }

        public static void ShowCaesarXor(char[] input)
        {
            List<CharRecord> offsets = new List<CharRecord>();
            for (int i = 0; i < 256; i++)
            {
                char[] result = CaesarAdd(input, i);
                List<CharRecord> counted = CharRecord.CountRecords(result);
                offsets.Add(new CharRecord((char)i, CharRecord.GetMaxFreqDifference(CharRecord.Normalize(counted))));
            }
            CharRecord.DrawGraphics(offsets);
        }

        public static char[] AttackCaesarXor(char[] text)
        {
            double[] press = new double[256];
            double maxPresence = 0;
            int maxPresIndex = 0;
            for (int i = 0; i < 256; i++)
            {
                char[] result = CaesarXor(text, i);
                press[i] = CharRecord.GetBigramsPresence(new string(result));
                if (press[i] > maxPresence)
                {
                    maxPresence = press[i];
                    maxPresIndex = i;
                }
            }
            return CaesarXor(text, maxPresIndex);
        }

        public static char[] MoveBy(char[] input, int offset)
        {
            char[] result = new char[input.Length];
            for (int i = 0, j = 0; i < input.Length; i++)
            {
                if (i + offset < result.Length)
                {
                    result[i + offset] = input[i];
                }
                else
                {
                    result[j] = input[i];
                    j++;
                }
            }
            return result;
        }

        public static List<NumberRecord> GetCoins(char[] input, int maxKeyLength)
        {
            List<NumberRecord> coins = new List<NumberRecord>();
            for (int i = 1; i < maxKeyLength; i++)
            {
                char[] moved = MoveBy(input, i);
                double coinCounter = 0;
                for (int j = 0; j < input.Length; j++)
                {
                    if (input[j].Equals(moved[j]))
                    {
                        coinCounter++;
                    }
                }
                coins.Add(new NumberRecord(i, (coinCounter / input.Length) * 100));
            }
            return coins;
        }

        public static List<NumberRecord> GetCriticalCoins(List<NumberRecord> coins, double threshold)
        {
            List<NumberRecord> critCoins = new List<NumberRecord>();
            double critThreshold = threshold;
            for (int i = 1; i < coins.Count - 1; i++)
            {
                double mid = coins[i].Count;
                double left = coins[i - 1].Count;
                double right = coins[i + 1].Count;
                if (mid - left > critThreshold && mid - right > critThreshold)
                {
                    critCoins.Add(coins[i]);
                }
            }
            return critCoins;
        }

        /*
         * threshold - in percents. 5% is OK
         */
        public static int GetKeyLength(char[] input, int maxKeyLength, double threshold)
        {
            if (maxKeyLength > input.Length)
            {
                maxKeyLength = input.Length;
            }
            List<NumberRecord> coins = GetCriticalCoins(GetCoins(input, maxKeyLength), threshold);
            return (int)coins[0].Key;
        }

        public static List<char[]> SplitIntoGroups(char[] text, int ngroups)
        {
            List<List<char>> groups = new List<List<char>>();
            for (int i = 0; i < ngroups; i++)
            {
                int j = i;
                groups.Add(new List<char>());
                for (; j < text.Length; j += ngroups)
                {
                    groups[i].Add(text[j]);
                }
            }
            List<char[]> result = new List<char[]>();
            foreach (var el in groups)
            {
                result.Add(el.ToArray());
            }
            return result;
        }

        public static char GetSummingKeyLetter(char[] group)
        {
            List<CharRecord> normalized = CharRecord.Normalize(CharRecord.CountRecords(group));
            double[] maxDiffs = new double[256];
            for (int i = 0; i < 256; i++)
            {
                maxDiffs[i] = CharRecord.GetMaxFreqDifference(CharRecord.MoveFrequenciesBy(normalized, i));
            }
            int minDiffIndex = -1;
            double minDiff = double.MaxValue;
            for (int i = 0; i < maxDiffs.Length; i++)
            {
                if (Math.Abs(maxDiffs[i]) < Math.Abs(minDiff))
                {
                    minDiff = maxDiffs[i];
                    minDiffIndex = i;
                }
            }
            return Convert.ToChar(minDiffIndex);
        }

        public static string GetSummingKey(char[] text)
        {
            string result = "";
            List<char[]> groups = SplitIntoGroups(text, GetKeyLength(text, 20, 5));
            foreach (var el in groups)
            {
                result += GetSummingKeyLetter(el);
            }
            return result;
        }

        public static char GetXorKeyLetter(char[] group)
        {
            double[] aveDiffs = new double[256];
            for (int i = 0; i < 256; i++)
            {
                aveDiffs[i] =
                    CharRecord.GetAveFreqDifferenceAll(
                        CharRecord.NormalizeAllRegs(
                            CharRecord.CountRecords(
                                CaesarXor(group, i))));
            }
            int minDiffIndex = -1;
            double minDiff = double.MaxValue;
            for (int i = 0; i < aveDiffs.Length; i++)
            {
                if (Math.Abs(aveDiffs[i]) < Math.Abs(minDiff))
                {
                    minDiff = aveDiffs[i];
                    minDiffIndex = i;
                }
            }
            return Convert.ToChar(minDiffIndex);
        }

        public static List<char> GetXorKeyLetterCandidates(char[] group)
        {
            double[] aveDiffs = new double[256];
            List<char> cands = new List<char>();
            for (int i = 0; i < 256; i++)
            {
                aveDiffs[i] =
                    CharRecord.GetAveFreqDifferenceAll(
                        CharRecord.NormalizeAllRegs(
                            CharRecord.CountRecords(
                                CaesarXor(group, i))));
            }
            int minDiffIndex = -1;
            double minDiff = double.MaxValue;
            for (int i = 0; i < aveDiffs.Length; i++)
            {
                if (Math.Abs(aveDiffs[i]) < Math.Abs(minDiff))
                {
                    minDiff = aveDiffs[i];
                    minDiffIndex = i;
                }
            }
            cands.Add(Convert.ToChar(minDiffIndex));
            //look for same diffs
            for (int i = minDiffIndex + 1; i < aveDiffs.Length; i++)
            {
                if (aveDiffs[i] == minDiff)
                {
                    cands.Add(Convert.ToChar(i));
                }
            }
            return cands;
        }

        public static string GetXorKey(char[] text)
        {
            string result = "";
            List<char[]> groups = SplitIntoGroups(text, GetKeyLength(text, 20, 5));
            foreach (var el in groups)
            {
                result += GetXorKeyLetter(el);
            }
            return result;
        }

        public static List<string> GetXorKeyCandidates(char[] text)
        {
            List<string> inter = new List<string>();
            List<string> keys = new List<string>();
            List<char[]> groups = SplitIntoGroups(text, GetKeyLength(text, 20, 5));
            foreach (var el in groups)
            {
                inter.Add(new string(GetXorKeyLetterCandidates(el).ToArray()));
            }

            GetCombination(inter.ToArray(), "", 0, keys);

            return keys;
        }

        public static void GetCombination(string[] str, string partial, int p, List<string> result)
        {
            if (p == str.Length)
            {
                result.Add(partial);
                return;
            }
            for (int i = 0; i != str[p].Length; i++)
            {
                GetCombination(str, partial + str[p][i], p + 1, result);
            }
        }

        public static char[] DecryptByXorKey(char[] text, string key)
        {
            char[] result = new char[text.Length];
            char[] keyArr = key.ToCharArray();
            for (int i = 0, j = 0; i < text.Length; i++, j++)
            {
                if (j >= keyArr.Length)
                {
                    j = 0;
                }
                result[i] = (char)(text[i] ^ keyArr[j]);
            }
            return result;
        }

        public static char[] BreakXorVigenere(char[] text)
        {
            return DecryptByXorKey(text, GetXorKey(text));
        }

        public static List<string> BreakXorVigenereAllVariants(char[] text)
        {
            List<string> result = new List<string>(); 
            List<string> keys = GetXorKeyCandidates(text);
            foreach(var el in keys)
            {
                result.Add(new string(DecryptByXorKey(text, el)));
            }
            return result;
        }

        public static char[] DecodeFromBase64(char[] text)
        {
            string converted = Encoding.UTF8.GetString(Convert.FromBase64String(new string(text)));
            char[] result = converted.ToCharArray();
            return result;
        }

        public static char[] ExchangeChars(char[] input, List<ExchangeRecord> exchangeList)
        {
            char[] result = new char[input.Length];
            input.CopyTo(result, 0);
            for (int i = 0; i < exchangeList.Count; i++)
            {
                for (int j = 0; j < result.Length; j++)
                {
                    if (exchangeList[i].a.Equals(result[j]))
                    {
                        result[j] = exchangeList[i].b;
                    }
                }
            }
            return result;
        }

        public static char[] BreakExchange(char[] input, List<CharRecord> alphabet, int retriesIfUseless)
        {
            List<ExchangeRecord> key = ExchangeRecord.GenExchangeList(alphabet);
            char[] text = new char[input.Length];
            input.CopyTo(text, 0);
            double englishness = 100;
            int uselessExchangingCounter = retriesIfUseless;
            Random rnd = new Random();
            do
            {
                List<ExchangeRecord> candidate = ExchangeRecord.ChangeExchangeList(key, rnd);
                //double newEnglishness = CharRecord.GetBigramsPresence(new string(ExchangeChars(text, candidate)));
                //double newEnglishness = StringRecord.GetPresence(StringRecord.GetLowerString(ExchangeChars(text, candidate)), StringRecord.trigramFreqEngl);
                List<StringRecord> counted = StringRecord.CountAllPercents(StringRecord.GetLowerString(ExchangeChars(text, candidate)), StringRecord.trigramFreqEngl);
                List<StringRecord> diffs = StringRecord.GetDiffs(counted, StringRecord.trigramFreqEngl);
                double newEnglishness = StringRecord.GetCountSum(diffs);
                if (newEnglishness < englishness)
                {
                    englishness = newEnglishness;
                    key = candidate;
                    uselessExchangingCounter = retriesIfUseless;
                }
                else if (newEnglishness == englishness)
                {
                    uselessExchangingCounter--;
                }
                //uselessExchangingCounter--;
            } while (uselessExchangingCounter > 0);

            Console.WriteLine(englishness);
            StringRecord.PrintList(StringRecord.CountAllPercents(StringRecord.GetLowerString(ExchangeChars(text, key)), StringRecord.trigramFreqEngl));
            ExchangeRecord.Show(key);
            return ExchangeChars(text, key);
        }

        public static string BreakMonoSum(string text)
        {
            var elite = new EliteSelection();
            
            var xover = new PositionBasedCrossover();
            
            var mut = new TworsMutation();
            var fitf = new SubstitutionFitness(text);

            var chromo = new SubstitutionChromosome();

            var pop = new Population(200, 250, chromo);

            var ga = new Algorithm(pop, fitf, elite, xover, mut)
            {
                Termination = new GenerationNumberTermination(500),
                CrossoverProbability = 0.4f,
                MutationProbability = 0.85f
            };

            ga.Start();

            Console.WriteLine($"Key: {ga.BestChromosome}");

            return ((SubstitutionChromosome)ga.BestChromosome).ToString();
        }

        public static void BreakPolySub(string text)
        {
            for (int i = 2; i < 6; i++)
            {
                Console.WriteLine($"finding n-gram diffs, n = {i}");
                var trigramDistances = FrequencyAnalysis.GetNGramDistances(i, text);
                foreach (var distance in trigramDistances.Take(5))
                {
                    Console.WriteLine($"{distance.Key}: {string.Join(", ", distance.Value)}");
                }
                Console.WriteLine();
            }
            var keyLength = Utils.GetInt("key length", text.Length, 1);

            var rows = text.Chunk(keyLength).ToArray();
            var columns = Enumerable
                .Range(0, keyLength)
                .Select(i => rows.Select(row => row
                        .ToCharArray()
                        .ElementAt(i))
                    .ToArray())
                .Select(columnLetters => new string(columnLetters));

            Console.WriteLine("Columns: ");
            var keys = new List<string>();
            // use genetic algorithm to every column
            foreach (var column in columns)
            {
                Console.WriteLine(column);
                var key = BreakMonoSum(column);
                keys.Add(key);
            }
            Console.WriteLine();

            (char plain, char cipher)[][] knownLetters =
            {
               new[] { ('C', 'U'), ('T', 'Z'), ('I', 'B'), ('B', 'E'), ('Y', 'X'), ('S', 'M'), ('R', 'L'), ('D', 'I'), ('L', 'O'), ('O', 'K'), ('N', 'F'), ('P', 'N'), ('E', 'J'), ('A', 'T'), ('H', 'P'), ('K', 'A'), ('W', 'Q'), ('Q', 'D'), ('G', 'S'), ('U', 'R'), ('M', 'H'), ('F', 'Y'), ('J', 'V') },
               new[] { ('A', 'Y'), ('C', 'J'), ('T', 'K'), ('I', 'D'), ('S', 'O'), ('H', 'I'), ('E', 'E'), ('O', 'M'), ('N', 'T'), ('F', 'U'), ('D', 'B'), ('B', 'Q'), ('U', 'Z'), ('R', 'H'), ('G', 'N'), ('M', 'X'), ('L', 'L'), ('Y', 'V'), ('M', 'P'), ('W', 'A'), ('Z', 'C'), ('K', 'W'), ('P', 'R') },
               new[] { ('P', 'X'), ('L', 'T'), ('A', 'O'), ('T', 'R'), ('L', 'T'), ('E', 'A'), ('N', 'U'), ('S', 'Y'), ('U', 'G'), ('B', 'Q'), ('O', 'I'), ('I', 'H'), ('D', 'P'), ('H', 'Z'), ('K', 'L'), ('Y', 'M'), ('R', 'D'), ('W', 'W'), ('G', 'K'), ('X', 'B'), ('F', 'S'), ('C', 'F'), ('M', 'J') },
               new[] { ('I', 'C'), ('L', 'H'), ('P', 'E'), ('L', 'H'), ('A', 'L'), ('H', 'K'), ('G', 'P'), ('U', 'X'), ('O', 'U'), ('E', 'N'), ('N', 'A'), ('C', 'G'), ('S', 'Q'), ('R', 'S'), ('B', 'J'), ('T', 'Y'), ('W', 'Z'), ('X', 'V'), ('N', 'T'), ('Y', 'M'), ('J', 'B'), ('M', 'D'), ('D', 'O'), ('V', 'W'), ('F', 'R'), ('Z', 'I') }
            };


            for (int i = 0; i < keys.Count(); i++)
            {
                string key = keys[i];
                string newKey = key;
                for (int j = 0; j < knownLetters[i].Length; j++)
                {
                    (char plain, char cipher) knownLetter = knownLetters[i][j];
                    newKey = newKey.ChangeKeyChar(knownLetter.plain, knownLetter.cipher);
                }
                keys[i] = newKey;
            }

            Console.WriteLine("\nKeys: ");
            foreach (var key in keys)
            {
                Console.WriteLine(key);
            }
            Console.WriteLine();

            var decipheredText = PolySubstitutionCipher.Decrypt(keys.ToArray(), text);

            for (int i = 1; i <= decipheredText.Length; i++)
            {
                if ((i - 1) % 100 == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine(text.Substring(i - 1, text.Substring(i - 1).Length >= 100 ? 100 : text.Substring(i - 1).Length % 100));
                    Console.WriteLine(string.Concat(Enumerable.Repeat("1234", 25)));
                }

                char ch = text[i - 1];

                bool firstAlphabet = i % 4 == 1 && knownLetters[0].Any(kl => kl.cipher == ch);

                bool secondAlphabet = i % 4 == 2 && knownLetters[1].Any(kl => kl.cipher == ch);

                bool thirdAlphabet = i % 4 == 3 && knownLetters[2].Any(kl => kl.cipher == ch);

                bool fourthAlphabet = i % 4 == 0 && knownLetters[3].Any(kl => kl.cipher == ch);
                
                if (firstAlphabet || secondAlphabet || thirdAlphabet || fourthAlphabet)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(decipheredText[i - 1]);
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(decipheredText[i - 1]);
                    Console.ForegroundColor = ConsoleColor.Red;
                }
            }
            Console.WriteLine();
            Console.WriteLine(decipheredText);
        }
    }
}

