using System;
using System.IO;
using System.Text;
namespace RSA_bis
{
    public class Decrypt
    {
        public static string DecryptWithPublicKey(string encryptedMessage, string publicKeyFilePath)
        {
            
            string[] numbers = { };
            long n, publicExponentE;
            string line;
            
            if (File.Exists(publicKeyFilePath))
            {

                using (StreamReader reader = File.OpenText(publicKeyFilePath))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        numbers = line.Split(' ');
                    }
                }
            }
            n = int.Parse(numbers[2]);
            publicExponentE = int.Parse(numbers[3]);
            byte[] encryptedBytes = Convert.FromBase64String(encryptedMessage);
            
            byte[] decryptedBytes = DecryptWithPublicKey(encryptedBytes, publicExponentE, n);

            // Convert the decrypted bytes back to string
            string decryptedMessage = Encoding.UTF8.GetString(decryptedBytes);

            return decryptedMessage;
        }

        static byte[] DecryptWithPublicKey(byte[] encryptedBytes, long exponent, long modulus)
        {
            // Ensure correct endianness
            if (BitConverter.IsLittleEndian)
                Array.Reverse(encryptedBytes);

            long encryptedInt = BitConverter.ToInt64(encryptedBytes, 0);
            long decryptedInt = Encrypt.ModPow(encryptedInt, exponent, modulus);
            byte[] decryptedBytes = BitConverter.GetBytes(decryptedInt);

            // Ensure correct endianness
            if (BitConverter.IsLittleEndian)
                Array.Reverse(decryptedBytes);

            return decryptedBytes;
        }

    }
}