using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WpfApp1final
{
    public class BFattack
    {
        public static string PerformBruteForceAttack(string encryptedPassword, int maxThreads)
        {
            const string charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const int passwordLength = 6;

            List<Task<string>> tasks = new List<Task<string>>();

            // Divide the workload among multiple threads
            for (int i = 0; i < maxThreads; i++)
            {
                int start = (charset.Length / maxThreads) * i;
                int end = (i == maxThreads - 1) ? charset.Length : (charset.Length / maxThreads) * (i + 1);

                tasks.Add(Task.Run(() => BruteForce(encryptedPassword, charset, passwordLength, start, end)));
            }

            // Wait for all tasks to complete
            Task.WaitAll(tasks.ToArray());

            // Retrieve the result from the completed tasks
            foreach (var task in tasks)
            {
                if (task.IsCompletedSuccessfully)
                {
                    return task.Result;
                }
            }

            return null; // No result found
        }

        // Helper method for multithreaded brute force
        private static string BruteForce(string encryptedPassword, string charset, int passwordLength, int start, int end)
        {
            char[] password = new char[passwordLength];

            // Start brute force attack
            for (int i = start; i < end; i++)
            {
                // Generate a candidate password
                for (int j = 0; j < passwordLength; j++)
                {
                    password[j] = charset[i]; // Use characters from the charset
                }

                // Convert the candidate password to a string
                string candidate = new string(password);

                // Check if the encrypted candidate password matches the given encrypted password
                if (Encryptor.EncryptPassword(candidate) == encryptedPassword)
                {
                    return candidate; // Return the original password
                }
            }

            return null; // No result found
        }
    }
}