using System;
using System.IO;
using System.Text;
namespace RSA_bis
{
    public class Encrypt
    {
        public static string EncryptWithPrivateKey(string message, string privateKeyFilePath)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] encryptedBytes;

            // Load the private key from file
            string[] numbers = { };
            
            long n, privateExponentD;
            string line;
            
            if (File.Exists(privateKeyFilePath))
            {
                using (StreamReader reader = File.OpenText(privateKeyFilePath))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        numbers = line.Split(' ');
                    }
                }
            }
            n = int.Parse(numbers[2]);
            privateExponentD = int.Parse(numbers[4]);
            
            encryptedBytes = EncryptWithPrivateKey(messageBytes, privateExponentD, n);

            // Convert the encrypted bytes to Base64 string for easy storage
            string encryptedMessage = Convert.ToBase64String(encryptedBytes);

            return encryptedMessage;
        }

        static byte[] EncryptWithPrivateKey(byte[] message, long d, long modulus)
        {
            long messageInt = ToPositiveBigInteger(message);
            long encryptedInt = ModPow(messageInt, d, modulus);
            byte[] encryptedBytes = BitConverter.GetBytes(encryptedInt);

            // Ensure correct endianness
            if (BitConverter.IsLittleEndian)
                Array.Reverse(encryptedBytes);

            return encryptedBytes;
        }
        
        static long ToPositiveBigInteger(byte[] bytes)
        {
            byte[] positiveBytes = new byte[bytes.Length + 1];
            Array.Copy(bytes, 0, positiveBytes, 1, bytes.Length);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(positiveBytes);

            return BitConverter.ToInt64(positiveBytes, 0);
        }
        
        public static long ModPow(long value, long exponent, long modulus)
        {
            long result = 1;

            while (exponent > 0)
            {
                if ((exponent & 1) == 1)
                    result = (result * value) % modulus;

                value = (value * value) % modulus;
                exponent >>= 1;
            }

            return result;
        }
    }

    
}