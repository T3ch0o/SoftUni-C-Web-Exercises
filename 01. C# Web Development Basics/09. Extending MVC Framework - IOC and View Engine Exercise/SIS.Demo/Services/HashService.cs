namespace SIS.Demo.Services
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    using SIS.Demo.Services.Interfaces;

    internal class HashService : IHashService
    {
        public string Hash(string stringToHash)
        {
            stringToHash = stringToHash + "C#SaltHereBoys";

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
                string hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                return hash;
            }
        }
    }
}