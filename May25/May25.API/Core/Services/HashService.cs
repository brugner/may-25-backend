﻿using May25.API.Core.Contracts.Services;
using System;
using System.Security.Cryptography;

namespace May25.API.Core.Services
{
    public class HashService : IHashService
    {
        private const int SaltByteSize = 24;
        private const int HashByteSize = 20; // to match the size of the PBKDF2-HMAC-SHA-1 hash 
        private const int Pbkdf2Iterations = 1000;
        private const int IterationIndex = 0;
        private const int SaltIndex = 1;
        private const int Pbkdf2Index = 2;

        public string HashPassword(string password)
        {
            var cryptoProvider = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SaltByteSize];

            cryptoProvider.GetBytes(salt);

            var hash = GetPbkdf2Bytes(password, salt, Pbkdf2Iterations, HashByteSize);

            return Pbkdf2Iterations + ":" + Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
        }

        public bool ValidatePassword(string password, string passwordHash)
        {
            char[] delimiter = { ':' };
            var split = passwordHash.Split(delimiter);
            var iterations = int.Parse(split[IterationIndex]);
            var salt = Convert.FromBase64String(split[SaltIndex]);
            var hash = Convert.FromBase64String(split[Pbkdf2Index]);
            var testHash = GetPbkdf2Bytes(password, salt, iterations, hash.Length);

            return SlowEquals(hash, testHash);
        }

        private bool SlowEquals(byte[] a, byte[] b)
        {
            var diff = (uint)a.Length ^ (uint)b.Length;

            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }

            return diff == 0;
        }

        private byte[] GetPbkdf2Bytes(string password, byte[] salt, int iterations, int outputBytes)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt)
            {
                IterationCount = iterations
            };

            return pbkdf2.GetBytes(outputBytes);
        }
    }
}
