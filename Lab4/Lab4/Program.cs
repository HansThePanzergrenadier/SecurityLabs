using Konscious.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerator
{
    class Program
    {
        private const int recordsCount = 100;
        static async Task Main(string[] args)
        {
            var passwordGenerator = new PasswordGenerator();
            var records = await passwordGenerator.GenerateRecordsAsync(recordsCount);
            PrintResult(records);
            TakeToFile(await Task.Run(() => HashSha1(records)), "WeakPasswords");
            TakeToFile(await Task.Run(() => HashArgon2i(records)), "StrongPasswords");
            Console.WriteLine("Done.");
            Console.ReadKey();
        }

        private static void TakeToFile(List<string> strs, string fileName)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string finalPath = Path.Combine(basePath, fileName + ".csv");
            TextWriter writer = new StreamWriter(finalPath);

            foreach (var r in strs)
            {
                writer.WriteLine(r);
            }
        }

        

        private static byte[] CreateSalt()
        {
            var buffer = new byte[16];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buffer);
            return buffer;
        }

        private static byte[] StartHashPassword(string password, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));

            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 8; 
            argon2.Iterations = 4;
            argon2.MemorySize = 1024 * 1024; 

            return argon2.GetBytes(16);
        }

        private static List<string> HashSha1(List<string> recs)
        {
            var result = new List<string>();

            foreach (var record in recs)
            {
                var hash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(record));
                result.Add(string.Concat(hash.Select(b => b.ToString("x2"))));
            }

            return result;
        }

        private static List<string> HashArgon2i(List<string> records)
        {
            var result = new List<string>();

            foreach (var record in records)
            {
                var stopwatch = Stopwatch.StartNew();

                Console.WriteLine($"Password to be hashed: '{ record }'.");

                var salt = CreateSalt();
                Console.WriteLine($"Salt: '{ Convert.ToBase64String(salt) }'.");

                var hash = StartHashPassword(record, salt);
                Console.WriteLine($"Resulting hash: '{ Convert.ToBase64String(hash) }'.");

                stopwatch.Stop();
                //Console.WriteLine($"Time elapsed: { stopwatch.ElapsedMilliseconds / 1024.0 } s");

                result.Add($"Hash: {Convert.ToBase64String(hash)}, salt: {Convert.ToBase64String(salt)}");
            }

            return result;
        }

        static void PrintResult(IList<string> collection)
        {
            foreach (var item in collection)
            {
                Console.WriteLine(item);
            }
        }

        private static bool CheckHash(string password, byte[] salt, byte[] hash)
        {
            var newHash = StartHashPassword(password, salt);
            return hash.SequenceEqual(newHash);
        }

        

        

        
    }
}
