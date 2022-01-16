using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Lab5.Data.SecurityTools
{
    public class KeyRing : ILookupProtectorKeyRing
    {
        private readonly IDictionary<string, string> _keyDictionary = new Dictionary<string, string>();

        public KeyRing(IWebHostEnvironment hostingEnvironment)
        {
            string keyRingDirectory = Path.Combine(hostingEnvironment.ContentRootPath, "keyring");
            Directory.CreateDirectory(keyRingDirectory);

            var directoryInfo = new DirectoryInfo(keyRingDirectory);
            if (directoryInfo.GetFiles("*.key").Length == 0)
            {
                SymmetricAlgorithm encryptionAlgorithm = Aes.Create();
                encryptionAlgorithm.KeySize = 256;
                KeyedHashAlgorithm signingAlgorithm = new HMACSHA512();
                encryptionAlgorithm.GenerateKey();

                string keyAsString = Convert.ToBase64String(encryptionAlgorithm.Key);
                string keyId = Guid.NewGuid().ToString();
                string keyFileName = Path.Combine(keyRingDirectory, keyId + ".key");
                using (StreamWriter file = File.CreateText(keyFileName))
                {
                    file.WriteLine(keyAsString);
                }

                _keyDictionary.Add(keyId, keyAsString);

                CurrentKeyId = keyId;
                encryptionAlgorithm.Clear();
                encryptionAlgorithm.Dispose();
                signingAlgorithm.Dispose();
            }
            else
            {
                List<string> filesOrdered = directoryInfo.EnumerateFiles().OrderByDescending(d => d.CreationTime).Select(d => d.Name).ToList();

                foreach (var fileName in filesOrdered)
                {
                    string keyFileName = Path.Combine(keyRingDirectory, fileName);
                    string key = File.ReadAllText(keyFileName);
                    string keyId = Path.GetFileNameWithoutExtension(fileName);
                    _keyDictionary.Add(keyId, key);
                    CurrentKeyId = keyId;
                }
            }
        }

        public string CurrentKeyId
        {
            get; private set;
        }

        public string this[string keyId]
        {
            get
            {
                return _keyDictionary[keyId];
            }
        }

        public IEnumerable<string> GetAllKeyIds()
        {
            return _keyDictionary.Keys;
        }
    }
}
