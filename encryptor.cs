using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WpfApp1final
{

    public class Encryptor
    {

        private const string StaticSalt = "S3cureS@lt!";
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("EiNvS7nKQAqB6G7aJxCjFmOsQJnN59bN");

        public static string EncryptPassword(string password)
        {
            byte[] saltBytes = Encoding.UTF8.GetBytes(StaticSalt);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            using (RijndaelManaged aes = new RijndaelManaged())
            {
                aes.Key = Key;
                aes.Padding = PaddingMode.PKCS7;

                // Set the salt as the initialization vector (IV)
                aes.IV = saltBytes;

                // Create an encryptor to perform the stream transform
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                // Create the streams used for encryption
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        // Write all data to the crypto stream and flush it
                        csEncrypt.Write(passwordBytes, 0, passwordBytes.Length);
                        csEncrypt.FlushFinalBlock();

                        // Convert the encrypted bytes to a string
                        byte[] encryptedBytes = msEncrypt.ToArray();
                        string encryptedPassword = Convert.ToBase64String(encryptedBytes);

                        return encryptedPassword;
                    }
                }
            }
        }
    }
}
    

