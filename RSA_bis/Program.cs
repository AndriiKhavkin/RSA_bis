using System;
using System.IO;

namespace RSA_bis
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //long[] KeysParts = Generate_Keys.GenerateKeys();
            Generate_Keys.GenerateKeys();

            string filePathMessageTxt = ("D:/Users/Andre/RiderProjects/RSA_bis/RSA_bis/message.txt");
            
            string text = null;

            if (File.Exists(filePathMessageTxt))
            {
                // Open the file for reading
                using (StreamReader reader = File.OpenText(filePathMessageTxt))
                {
                    // Read and display each line of the file
                    while (!reader.EndOfStream)
                    {
                        text = reader.ReadLine();
                    }
                }
            }
            else
            {
                Console.WriteLine("File does not exist.");
            }
            
            
            Console.WriteLine("Your Text: " + text);

            string filePathKeys = "D:/Users/Andre/RiderProjects/RSA_bis/RSA_bis/Keys.txt";
            
            string encryptedText = Encrypt.EncryptWithPrivateKey(text, filePathKeys);
            
            Console.WriteLine("Encrypted Text: " + encryptedText);
            

            string decryptedText = Decrypt.DecryptWithPublicKey(encryptedText, filePathKeys);
            Console.WriteLine("Decrypted Text: " + decryptedText);
         
            
            Console.WriteLine("\nPress enter to exit... ");
            Console.ReadLine();
        }

    }
}