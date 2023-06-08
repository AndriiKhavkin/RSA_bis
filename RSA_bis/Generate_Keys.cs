using System;
using System.IO;

namespace RSA_bis
{
    public class Generate_Keys
    {
        static Random random = new Random();
        
        public static /*long[]*/ void GenerateKeys()
        {

            long[] KeysParts = new long[5];
            const int min = 4095; // Minimum value for prime number generation
            const int max = 8191; // Maximum value for prime number generation
            
            Console.WriteLine($"Min: {min}");
            Console.WriteLine($"Max: {max}");
            
            int p = GenerateRandomPrime(min, max);
            int q = GenerateRandomPrime(min, max);
            
            Console.WriteLine($"Random P: {p}");
            Console.WriteLine($"Random Q: {q}");

            int n = (p * q);
            Console.WriteLine($"Random n {n}");
            int[] PQ = Fermat_Factor.FermatFactor(n);
            bool satisfyConditionsFermat = false;
            bool satisfyConditionsPollard = false;
            
            while (!satisfyConditionsFermat)
            {
                if (PQ[0] == p && PQ[1] == q)
                {
                    Console.WriteLine("Fermat Factor revealed that p and q do not satisfy the conditions. Trying to generate new values...");
                    // Generate new values for p and q 
                    p = GenerateRandomPrime(min, max);
                    q = GenerateRandomPrime(min, max);
                    
                    Console.WriteLine($"Random P: {p}");
                    Console.WriteLine($"Random Q: {q}");
                    n = (p * q);
                    
                    Console.WriteLine($"Random n {n}");
                    Array.Clear(PQ, 0, PQ.Length);
                    PQ = Fermat_Factor.FermatFactor(n);
                    
                }else
                {
                    Console.WriteLine("Fermat Factor revealed that p and q satisfy the conditions.");
                    while (!satisfyConditionsPollard)
                    {
                        if (p == Pollard_Factor.PollardFactor(n) || q == Pollard_Factor.PollardFactor(n))
                        {
                            Console.WriteLine("Pollard Factor revealed that p and q do not satisfy the conditions. Trying to generate new values...");
                            // Generate new values for p and q 
                            p = GenerateRandomPrime(min, max);
                            q = GenerateRandomPrime(min, max);
            
                            Console.WriteLine($"Random P: {p}");
                            Console.WriteLine($"Random Q: {q}");
                            n = (p * q);
                            Console.WriteLine($"Random n {n}");
                        }
                        else
                        {
                            Console.WriteLine("Pollard Factor revealed that p and q satisfy the conditions.");
                            satisfyConditionsPollard = true;
                        }
                    }
                    satisfyConditionsFermat = true;
                }
            }
            Console.WriteLine("All factors revealed that p and q satisfy the conditions.");
            
            int phi = EulerPhi(p, q);
            Console.WriteLine($"Your phi is: {phi}");

            int publicExponent = 65537;
            //int publicExponent = GeneratePublicExponent(phi);
            Console.WriteLine("Generated  public exponent (e): " + publicExponent);

            long privateExponent = CalculatePrivateExponent(publicExponent, phi);
            Console.WriteLine("Calculated private exponent (d): " + privateExponent);
            
            Console.WriteLine("Your  public key : {" + publicExponent + ", " + n + "}");

            Console.WriteLine("Your  private key : {" + privateExponent + ", " + n + "}");
            KeysParts[0] = p;
            KeysParts[1] = q;
            KeysParts[2] = n;
            KeysParts[3] = publicExponent;
            KeysParts[4] = privateExponent;
            
            string filePath = "D:/Users/Andre/RiderProjects/RSA_bis/RSA_bis/Keys.txt";

            // Create a new text file and open it for writing
            using (StreamWriter writer = File.CreateText(filePath))
            {
                // Write some text to the file
                writer.Write(p);
                writer.Write(' ');
                writer.Write(q);
                writer.Write(' ');
                writer.Write(n);
                writer.Write(' ');
                writer.Write(publicExponent);
                writer.Write(' ');
                writer.Write(privateExponent);
            }
            
            //return KeysParts;
        }

        static int GenerateRandomPrime(int min, int max)
        {
            int randomValue = random.Next(min, max);
            while (!IsPrime(randomValue))
            {
                randomValue = random.Next(min, max);
            }
            return randomValue;
        }
        
        static bool IsPrime(int number)
        {
            if (number < 2)
                return false;
            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0)
                    return false;
            }
            return true;
        }
        
        static int EulerPhi(int p, int q)
        {
            int phi = (p - 1) * (q - 1);
            return phi;
        }
        
        static int GeneratePublicExponent(int phi)
        {
            Random random = new Random();
            int publicExponent;

            do
            {
                publicExponent = random.Next(3, phi); // Generate a random value between 3 and phi (exclusive)
            } while (Pollard_Factor.GCD(publicExponent, phi) != 1); // Ensure the public exponent is coprime with phi

            return publicExponent;
        }
        
        static long CalculatePrivateExponent(long publicExponent, long phi)
        {
            long d = ExtendedEuclidean(publicExponent, phi);
            while (d < 0)
            {
                d += phi;
            }
            return d;
        }

        static long ExtendedEuclidean(long a, long b)
        {
            long x = 0, y = 1, lastX = 1, lastY = 0;

            while (b != 0)
            {
                long quotient = a / b;

                long temp = a;
                a = b;
                b = temp % b;

                temp = x;
                x = lastX - quotient * x;
                lastX = temp;

                temp = y;
                y = lastY - quotient * y;
                lastY = temp;
            }

            return lastX;
        }
    }
}