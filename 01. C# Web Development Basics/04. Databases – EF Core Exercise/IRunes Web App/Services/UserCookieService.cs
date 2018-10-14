namespace CakeWebApp.Services
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    using CakeWebApp.Services.Interfaces;

    internal class UserCookieService : IUserCookieService
    {
        private const string EncryptKey = "E546C8DF278CD5931069B522E695D4F2";

        private string EncryptString(string text)
        {
            byte[] key = Encoding.UTF8.GetBytes(EncryptKey);

            using (Aes aesAlg = Aes.Create())
            {
                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                                swEncrypt.Write(text);
                        }

                        byte[] iv = aesAlg.IV;

                        byte[] decryptedContent = msEncrypt.ToArray();

                        byte[] result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        private string DecryptString(string cipherText)
        {
            byte[] fullCipher = Convert.FromBase64String(cipherText);

            byte[] iv = new byte[16];
            byte[] cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            byte[] key = Encoding.UTF8.GetBytes(EncryptKey);

            using (Aes aesAlg = Aes.Create())
            {
                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (MemoryStream msDecrypt = new MemoryStream(cipher))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt)) result = srDecrypt.ReadToEnd();
                        }
                    }

                    return result;
                }
            }
        }

        public string GetUserCookie(string username)
        {
            string cookieContent = EncryptString(username);

            return cookieContent;
        }

        public string GetUserData(string cookieContent)
        {
            string username = DecryptString(cookieContent);

            return username;
        }
    }
}