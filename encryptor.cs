using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WpfApp1final
{
    public class Encryptor
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("EiNvS7nKQAqB6G7aJxCjFmOsQJnN59bN");

        public static string EncryptPassword(string password)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.GenerateIV();
                byte[] iv = aes.IV;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(aes.Key, iv), CryptoStreamMode.Write))
                    {
                        cs.Write(passwordBytes, 0, passwordBytes.Length);
                        cs.FlushFinalBlock();
                    }

                    byte[] encrypted = ms.ToArray();
                    byte[] result = new byte[iv.Length + encrypted.Length];
                    Array.Copy(iv, 0, result, 0, iv.Length);
                    Array.Copy(encrypted, 0, result, iv.Length, encrypted.Length);

                    return Convert.ToBase64String(result);
                }
            }
        }

        public static string DecryptPassword(string encryptedPassword)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedPassword);
            byte[] iv = new byte[16];
            byte[] cipherText = new byte[encryptedBytes.Length - iv.Length];

            Array.Copy(encryptedBytes, 0, iv, 0, iv.Length);
            Array.Copy(encryptedBytes, iv.Length, cipherText, 0, cipherText.Length);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = iv;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(aes.Key, aes.IV), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherText, 0, cipherText.Length);
                        cs.FlushFinalBlock();
                    }

                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }
    }
}
