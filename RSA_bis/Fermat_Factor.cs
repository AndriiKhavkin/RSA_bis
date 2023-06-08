using System;
namespace RSA_bis
{
    public class Fermat_Factor
    {
        public static int[] FermatFactor(int n)
        {
            int a, b;
            //якщо число парне, повертаємо результат
            if ((n % (int)2UL) == 0)
            {
                return new[] { (int)2UL, n / (int)2UL };
            }
            a = (int)(Math.Ceiling(Math.Sqrt(n)));
            if (a * a == n)
            {
                return new[] { a, a };
            }
            while (true)
            {
                int tmp = a * a - n;
                b = (int)(Math.Sqrt(tmp));
                if (b * b == tmp)
                {
                    break;
                }
                a++;
            }
            return new[] { a - b, a + b };
        }
    }
}