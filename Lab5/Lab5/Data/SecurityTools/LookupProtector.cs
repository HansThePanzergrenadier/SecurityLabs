using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Lab5.Data.SecurityTools
{
    public class LookupProtector : ILookupProtector
    {
        private readonly ILookupProtectorKeyRing _keyRing;

        public LookupProtector(ILookupProtectorKeyRing keyRing)
        {
            _keyRing = keyRing;
        }

        public string Protect(string keyId, string data)
        {
            SymmetricAlgorithm encryptingAlgorithm = Aes.Create();
            KeyedHashAlgorithm signingAlgorithm = new HMACSHA512();
            encryptingAlgorithm.KeySize = 256;
            int keyDerivationIterationCount = 5000;
            byte[] masterKey = MasterKey(keyId);

            // Convert the string to bytes, because encryption works on bytes, not strings.
            byte[] plainText = Encoding.UTF8.GetBytes(data);
            byte[] cipherTextAndIV;

            // Derive a key for encryption from the master key
            encryptingAlgorithm.Key = DerivedEncryptionKey(masterKey, encryptingAlgorithm, keyDerivationIterationCount);

            // As we need this to be deterministic, we need to force an IV that is derived from the plain text.
            encryptingAlgorithm.IV = DerivedInitializationVector(masterKey, data, encryptingAlgorithm, keyDerivationIterationCount);

            // And encrypt
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, encryptingAlgorithm.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(plainText);
                cs.FlushFinalBlock();
                byte[] encryptedData = ms.ToArray();

                cipherTextAndIV = CombineByteArrays(encryptingAlgorithm.IV, encryptedData);
            }

            // Now get a signature for the data so we can detect tampering in situ.
            byte[] signature = SignData(cipherTextAndIV, masterKey, encryptingAlgorithm, signingAlgorithm, keyDerivationIterationCount);

            // Add the signature to the cipher text.
            byte[] signedData = CombineByteArrays(signature, cipherTextAndIV);

            // Add our algorithm identifier to the combined signature and cipher text.
            byte[] algorithmIdentifier = BitConverter.GetBytes(1);
            byte[] output = CombineByteArrays(algorithmIdentifier, signedData);

            // Clean everything up.
            encryptingAlgorithm.Clear();
            signingAlgorithm.Clear();
            encryptingAlgorithm.Dispose();
            signingAlgorithm.Dispose();

            Array.Clear(plainText, 0, plainText.Length);

            // Return the results as a string.
            return Convert.ToBase64String(output);
        }

        public string Unprotect(string keyId, string data)
        {
            byte[] masterKey = MasterKey(keyId);
            byte[] plainText;
            byte[] payload = Convert.FromBase64String(data);

            SymmetricAlgorithm encryptingAlgorithm = Aes.Create();
            KeyedHashAlgorithm signingAlgorithm = new HMACSHA512();
            encryptingAlgorithm.KeySize = 256;
            int keyDerivationIterationCount = 5000;

            byte[] signature = new byte[signingAlgorithm.HashSize / 8];
            Buffer.BlockCopy(payload, 4, signature, 0, signingAlgorithm.HashSize / 8);
            int dataLength = payload.Length - 4 - signature.Length;
            byte[] cipherTextAndIV = new byte[dataLength];
            Buffer.BlockCopy(payload, 4 + signature.Length, cipherTextAndIV, 0, dataLength);

            byte[] computedSignature = SignData(cipherTextAndIV, masterKey, encryptingAlgorithm, signingAlgorithm, keyDerivationIterationCount);
            if (!ByteArraysEqual(computedSignature, signature))
            {
                throw new CryptographicException(@"Invalid Signature.");
            }
            signingAlgorithm.Clear();
            signingAlgorithm.Dispose();

            int ivLength = encryptingAlgorithm.BlockSize / 8;
            byte[] initializationVector = new byte[ivLength];
            byte[] cipherText = new byte[cipherTextAndIV.Length - ivLength];
            Buffer.BlockCopy(cipherTextAndIV, 0, initializationVector, 0, ivLength);
            Buffer.BlockCopy(cipherTextAndIV, ivLength, cipherText, 0, cipherTextAndIV.Length - ivLength);
            encryptingAlgorithm.Key = DerivedEncryptionKey(masterKey, encryptingAlgorithm, keyDerivationIterationCount);
            encryptingAlgorithm.IV = initializationVector;
            using (var ms = new MemoryStream())
            using (var cs = new CryptoStream(ms, encryptingAlgorithm.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(cipherText);
                cs.FlushFinalBlock();
                plainText = ms.ToArray();
            }
            encryptingAlgorithm.Clear();
            encryptingAlgorithm.Dispose();

            return Encoding.UTF8.GetString(plainText);
        }

        public byte[] MasterKey(string keyId)
        {
            return Convert.FromBase64String(_keyRing[keyId]);
        }

        private byte[] SignData(byte[] cipherText, byte[] masterKey, SymmetricAlgorithm symmetricAlgorithm, KeyedHashAlgorithm hashAlgorithm, int keyDerivationIterationCount)
        {
            hashAlgorithm.Key = DerivedSigningKey(masterKey, symmetricAlgorithm, keyDerivationIterationCount);

            byte[] signature = hashAlgorithm.ComputeHash(cipherText);
            hashAlgorithm.Clear();

            return signature;
        }

        private byte[] DerivedInitializationVector(byte[] key, string plainText, SymmetricAlgorithm algorithm, int keyDerivationIterationCount)
        {
            return KeyDerivation.Pbkdf2(plainText, key, KeyDerivationPrf.HMACSHA512, keyDerivationIterationCount, algorithm.BlockSize / 8);
        }

        private byte[] DerivedSigningKey(byte[] key, SymmetricAlgorithm algorithm, int keyDerivationIterationCount)
        {
            return KeyDerivation.Pbkdf2(@"IdentityLookupDataSigning", key, KeyDerivationPrf.HMACSHA512, keyDerivationIterationCount, algorithm.KeySize / 8);
        }

        private byte[] DerivedEncryptionKey(byte[] key, SymmetricAlgorithm algorithm, int keyDerivationIterationCount)
        {
            return KeyDerivation.Pbkdf2(@"IdentityLookupEncryption", key, KeyDerivationPrf.HMACSHA512, keyDerivationIterationCount, algorithm.KeySize / 8);
        }

        private byte[] CombineByteArrays(byte[] left, byte[] right)
        {
            byte[] output = new byte[left.Length + right.Length];

            Buffer.BlockCopy(left, 0, output, 0, left.Length);
            Buffer.BlockCopy(right, 0, output, left.Length, right.Length);

            return output;
        }

        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }

            bool areSame = true;
            for (int i = 0; i < a.Length; i++)
            {
                areSame = areSame & (a[i] == b[i]);
            }
            return areSame;
        }
    }
}
