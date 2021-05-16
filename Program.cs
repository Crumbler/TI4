using System;
using System.IO;
using System.Security.Cryptography;
using System.Linq;

namespace TI3
{
    public class Program
    {
        private static int getKey(int[] factors)
        {
            int key = 0;

            for (int i = 0; i < factors.Length; ++i)
                key |= 1 << (factors[i] - 1);

            return key;
        }

        private static void Encrypt()
        {
            Console.WriteLine("Enter the key");

            string keyLine = Console.ReadLine();

            int[] factors = keyLine.Split(' ').Select(e => int.Parse(e)).ToArray();

            int key = getKey(factors);

            Console.WriteLine("Enter the file name:");

            string fileName = Console.ReadLine();

            Console.WriteLine("Enter the new file name:");

            string newFileName = Console.ReadLine();

            using var oldStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            var lfsr = new LFSR(key);

            using var crStream = new CryptoStream(oldStream, lfsr, CryptoStreamMode.Read);
 
            using var newStream = new FileStream(newFileName, FileMode.OpenOrCreate, FileAccess.Write);

            crStream.CopyTo(newStream);
        }

        public static void Main()
        {
            // Encryption and decryption are identical
            // 24 4 3 1
            while (true)
            {
                Encrypt();

                Console.WriteLine();
            }
        }
    }
}
