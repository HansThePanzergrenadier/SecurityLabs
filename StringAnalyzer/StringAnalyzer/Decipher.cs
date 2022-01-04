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
            foreach (var el in keys)
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

        public static char[] ExchangeChars(char[] text, List<ExchangeRecord> key)
        {
            char[] result = new char[text.Length];
            text.CopyTo(result, 0);
            for (int i = 0; i < key.Count; i++)
            {
                for (int j = 0; j < result.Length; j++)
                {
                    if (key[i].a.Equals(result[j]))
                    {
                        result[j] = key[i].b;
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
                Dictionary<string, double> counted = StringRecord.CountAllPercents(StringRecord.GetLowerString(ExchangeChars(text, candidate)), StringRecord.trigramFreqEngl);
                Dictionary<string, double> diffs = StringRecord.GetDiffs(counted, StringRecord.trigramFreqEngl);
                double newEnglishness = StringRecord.GetCountSum(diffs);
                if (newEnglishness < englishness)
                {
                    englishness = newEnglishness;
                    key = candidate;
                    uselessExchangingCounter = retriesIfUseless;
                }
                else if (newEnglishness == englishness)
                {
                    //uselessExchangingCounter--;
                }
                uselessExchangingCounter--;
            } while (uselessExchangingCounter > 0);

            Console.WriteLine(englishness);
            StringRecord.PrintList(StringRecord.CountAllPercents(StringRecord.GetLowerString(ExchangeChars(text, key)), StringRecord.trigramFreqEngl));
            ExchangeRecord.Show(key);
            return ExchangeChars(text, key);
        }

        /*custom genetic section*/

        public static Dictionary<string, double> CountTextTrigrams(string text)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            int counter = 0;
            for (int i = 0; i <= text.Length - 3; i++)
            {
                string substr = text.Substring(i, 3);

                if (result.ContainsKey(substr))
                {
                    result[substr] += 1;
                }
                else
                {
                    result.Add(substr, 1);
                }

                counter = counter + 1;
            }

            for (int i = 0; i < result.Count; i++)
            {
                string substr = result.ElementAt(i).Key;
                result[substr] = result[substr] / counter;
            }

            return result;
        }

        public static double GetFitness(char[] text, List<ExchangeRecord> key, Dictionary<string, double> ethalonNGramsFreqs)
        {
            char[] deciphered = ExchangeChars(text, key);
            Dictionary<string, double> ethalonReduced = StringRecord.ReduceTrigrams(ethalonNGramsFreqs, deciphered);
            Dictionary<string, double> textTrigrams = CountTextTrigrams(new string(deciphered));
            //List<StringRecord> ethalonReduced = ethalonNGramsFreqs;
            double result = 0;
            foreach (var el in textTrigrams)
            {
                double ethalonTrigramFreq;
                if (ethalonReduced.ContainsKey(el.Key))
                {
                    ethalonTrigramFreq = ethalonReduced[el.Key];
                }
                else
                {
                    ethalonTrigramFreq = 0;
                }
                result += textTrigrams[el.Key] - ethalonTrigramFreq;
            }

            /*
            string lower = StringRecord.GetLowerString(deciphered);
            Dictionary<string, double> counted = StringRecord.CountAllPercents(lower, ethalonReduced);
            Dictionary<string, double> diffs = StringRecord.GetDiffs(counted, ethalonReduced);
            double result = StringRecord.GetCountSum(diffs);
            */
            return result;
        }

        public static Dictionary<List<ExchangeRecord>, double> GetSortedFitnesses(char[] text, List<List<ExchangeRecord>> keys, Dictionary<string, double> ethalonNGramsFreqs)
        {
            //counting
            Dictionary<List<ExchangeRecord>, double> result = new Dictionary<List<ExchangeRecord>, double>();

            foreach (var el in keys)
            {
                result.Add(el, GetFitness(text, el, ethalonNGramsFreqs));
            }

            //sorting
            Dictionary<List<ExchangeRecord>, double> sorted = new Dictionary<List<ExchangeRecord>, double>();
            while (result.Count > 0)
            {
                KeyValuePair<List<ExchangeRecord>, double> minEl = new KeyValuePair<List<ExchangeRecord>, double>(new List<ExchangeRecord>(), double.MaxValue);
                foreach (var el in result)
                {
                    if (el.Value < minEl.Value)
                    {
                        minEl = el;
                    }
                }
                sorted.Add(minEl.Key, minEl.Value);
                result.Remove(minEl.Key);
            }

            return sorted;
        }

        public static List<List<ExchangeRecord>> CutOffRating(Dictionary<List<ExchangeRecord>, double> keys)
        {
            List<List<ExchangeRecord>> result = new List<List<ExchangeRecord>>();

            foreach (var el in keys)
            {
                result.Add(el.Key);
            }

            return result;
        }

        public static List<List<ExchangeRecord>> CrossoverKeys(List<ExchangeRecord> firstParent, List<ExchangeRecord> secondParent, Random rnd)
        {
            List<List<ExchangeRecord>> children = new List<List<ExchangeRecord>>();
            List<ExchangeRecord> firstChild = ExchangeRecord.GetEmptyKey(firstParent.Count);
            List<ExchangeRecord> secondChild = ExchangeRecord.GetEmptyKey(firstParent.Count);

            int endSectionIndex = rnd.Next(1, firstParent.Count + 1);
            int startSectionIndex = rnd.Next(endSectionIndex);
            int sectionLength = endSectionIndex - startSectionIndex;

            for (int i = startSectionIndex; i < endSectionIndex; i++)
            {
                firstChild[i] = secondParent[i];
                secondChild[i] = firstParent[i];
            }

            for (int i = 0; i < firstParent.Count; i++)
            {
                if (i == startSectionIndex)
                {
                    i += sectionLength;
                    if (i >= firstParent.Count)
                    {
                        break;
                    }
                }
                int j = i;
                while (firstChild.Contains(firstParent[j]))
                {
                    j = firstChild.IndexOf(firstParent[j]);
                }
                firstChild[i] = firstParent[j];
                j = i;
                while (secondChild.Contains(secondParent[j]))
                {
                    j = secondChild.IndexOf(secondParent[j]);
                }
                secondChild[i] = secondParent[j];
            }

            children.Add(firstChild);
            children.Add(secondChild);
            return children;
        }

        //fitnesses - is what GetSortedFitnesses returns
        public static int SelectSomeKeyRandomized(Dictionary<List<ExchangeRecord>, double> fitnesses, Random rnd)
        {
            //count total fitness
            double populationFitness = 0;
            //List<(double, List<ExchangeRecord>)> fitnesses = GetUnsortedFitnesses(text, keys, ethalonNGramsFreqs);
            foreach (var el in fitnesses)
            {
                populationFitness += el.Value;
            }

            //select some random keys
            double threshold = populationFitness * rnd.NextDouble();
            double currentTotal = 0;
            int counter = 0;
            foreach (var el in fitnesses)
            {
                currentTotal += el.Value;
                if (currentTotal >= threshold)
                {
                    return counter;
                }
                counter++;
            }

            return 0;
        }

        //fitnesses - is what GetSortedFitnesses returns
        public static List<List<ExchangeRecord>> GenerateNewKeys(Dictionary<List<ExchangeRecord>, double> fitnesses)
        {
            List<List<ExchangeRecord>> sortedKeys = CutOffRating(fitnesses);
            List<List<ExchangeRecord>> result = new List<List<ExchangeRecord>>(fitnesses.Count);
            Random rnd = new Random();
            int oldKeysVolume = sortedKeys.Count / 5;

            for (int i = 0; i < oldKeysVolume; i++)
            {
                result.Add(sortedKeys[i]);
            }

            for (int i = oldKeysVolume; i < sortedKeys.Count; i += 2)
            {
                List<ExchangeRecord> firstParent = sortedKeys[SelectSomeKeyRandomized(fitnesses, rnd)];
                List<ExchangeRecord> secondParent = sortedKeys[SelectSomeKeyRandomized(fitnesses, rnd)];

                result.AddRange(CrossoverKeys(firstParent, secondParent, rnd));
            }

            return result;
        }

        public static string BreakExchangeByGenetic(char[] text, int nIterations, int populationSize, Dictionary<string, double> ethalonNGramsFreqs)
        {
            string result;
            List<List<ExchangeRecord>> keys = new List<List<ExchangeRecord>>();
            for (int i = 0; i < populationSize; i++)
            {
                keys.Add(ExchangeRecord.GenExchangeList(CharRecord.englLiteralsFreq));
            }

            Dictionary<List<ExchangeRecord>, double> keysRatedSorted = GetSortedFitnesses(text, keys, ethalonNGramsFreqs);

            for (int i = 0; i < nIterations; i++)
            {
                keys = GenerateNewKeys(keysRatedSorted);
                keysRatedSorted = GetSortedFitnesses(text, keys, ethalonNGramsFreqs);

            }

            string bestKey = ExchangeRecord.KeyToString(keysRatedSorted.First().Key);
            string deciphered = new string(ExchangeChars(text, keysRatedSorted.First().Key));
            ExchangeRecord.Show(keysRatedSorted.First().Key);

            result = $"Best key: {bestKey} \nText: {deciphered}";

            return result;
        }
    }
}

