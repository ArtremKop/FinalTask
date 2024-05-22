using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WpfApp1final
{
    public class BFattack
    {
        private const string Charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";  
        private const int MaxPasswordLength = 2;  

        public static string? PerformBruteForceAttack(string encryptedPassword, int maxThreads)
        {
            var tasks = new List<Task<string?>>();

            for (int i = 0; i < maxThreads; i++)
            {
                int threadIndex = i;
                tasks.Add(Task.Run(() => BruteForce(encryptedPassword, threadIndex, maxThreads)));
            }

            Task.WaitAll(tasks.ToArray());

            foreach (var task in tasks)
            {
                if (!string.IsNullOrEmpty(task.Result))
                {
                    return task.Result;
                }
            }
            return null;
        }

        private static string? BruteForce(string encryptedPassword, int threadIndex, int maxThreads)
        {
            int startIndex = threadIndex;
            char[] password = new char[MaxPasswordLength];

            while (true)
            {
                if (BruteForceRecursive(encryptedPassword, password, 0, startIndex, maxThreads))
                {
                    return new string(password);
                }

                startIndex += maxThreads;
                if (startIndex >= Charset.Length)
                {
                    break;
                }
            }

            return null;
        }

        private static bool BruteForceRecursive(string encryptedPassword, char[] password, int position, int startIndex, int maxThreads)
        {
            if (position == MaxPasswordLength)
            {
                string candidate = new string(password);
                string encryptedCandidate = Encryptor.EncryptPassword(candidate);
                Debug.WriteLine($"Trying password: {candidate} -> {encryptedCandidate}");
                if (encryptedCandidate == encryptedPassword)
                {
                    return true;
                }
                return false;
            }

            for (int i = startIndex; i < Charset.Length; i += maxThreads)
            {
                password[position] = Charset[i];
                if (BruteForceRecursive(encryptedPassword, password, position + 1, startIndex, maxThreads))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
