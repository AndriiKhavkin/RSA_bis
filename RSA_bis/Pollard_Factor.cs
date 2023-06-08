using System;

namespace RSA_bis
{
    public class Pollard_Factor
    {
        public static long PollardFactor(long n)
        {
            long x = 2;
            long y = 2;
            long d = 1;

            while (d == 1)
            {
                x = F(x, n);
                y = F(F(y, n), n);
                d = GCD((int)Math.Abs(x - y), (int)n);
            }

            return d;
        }

        public static long F(long x, long n)
        {
            return (x * x + 1) % n;
        }

        public static int GCD(int a, int b)
        {
            while (b != 0)
            {
                int remainder = a % b;
                a = b;
                b = remainder;
            }
            return a;
        }
    }
}