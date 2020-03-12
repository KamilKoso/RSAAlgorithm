using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Numerics;

namespace RSAalgorithm
{
   public class RSA
    {
        int P { get; set; }
        int Q { get; set; }
        int Z { get; set; }
        int E { get; set; }
        int D { get; set; }
        int N { get; set; }
        public int RandomPrimeMaxValue { get; set; } = 500;

        public RSA()
        {
            //RequestP();
            //RequestQ();
            SetValues();
        }

        


        public string ReturnPublicKey()
        {
           return ($"Twój klucz publiczny to [{N},{E}]" );
        }

       public string ReturnPrivateKey()
        {
            return ($"Twój klucz prywatny to [{N},{D}]");
        }
        

        public void SetValues()
        {
            P = GeneratePrime();
            Thread.Sleep(1000); //W przeciwnym wypadku wygeneruje 2 te same liczby pierwsze dla p i Q
            Q = GeneratePrime();
            Z = CalculateZ();
            E = CalculateE();
            D = CalculateD();
            N = CalculateN();
            
        }

        public void RequestP()
        {

            //Pobieranie P
            Console.WriteLine("Proszę podać liczbę pierwszą P");
            bool isParsingSuccesfull = Int32.TryParse(Console.ReadLine(), out int pBeforeCheck);
            if (!isParsingSuccesfull)
            {
                Console.WriteLine("Nie udało się skonwerować podanych znaków na liczbę. Czy podajesz tylko dodatnie liczby ?");
                RequestP();
            }
            if (IsPrime(pBeforeCheck))
                P = pBeforeCheck;
            else
            {
             Console.WriteLine("Podana liczba nie jest liczbą pierwszą. Proszę podać liczbę pierwszą");
                RequestP();
            }
        }

        public void RequestQ()   
        {

            //Pobieranie Q
            Console.WriteLine("Proszę podać liczbę pierwszą Q");
            bool isParsingSuccesfull = Int32.TryParse(Console.ReadLine(), out int qBeforeCheck);
                if (!isParsingSuccesfull)
                {
                Console.WriteLine("Nie udało się skonwerować podanych znaków na liczbę. Czy podajesz tylko liczby ?");
                RequestQ();
                }
            if (IsPrime(qBeforeCheck))
                Q = qBeforeCheck;
            else
            {
                Console.WriteLine("Podana liczba nie jest liczbą pierwszą. Proszę podać liczbę pierwszą\n");
                RequestQ();
            }
        }

        

        int GeneratePrime()
        {
            Random random = new Random();
            int randomNum = random.Next(RandomPrimeMaxValue);
            while (true)
            {
                randomNum += 2;
                if (randomNum % 2 == 0)
                    randomNum += 1;
                if (IsPrime(randomNum))
                {
                    return randomNum;
                   
                }
            }
        }

        int CalculateN(){ return P * Q; }

        int CalculateE()
        {
            int[] primes = GeneratePrimes(Z).ToArray();


            for (int i = primes.Length; i >= 2; i--)
                if (BiggestCommonDivider(i, Z) == 1)    //Jezeli wspolnym dzielnikiem I z Z jest 1 to znaczy ze I jest wzglednie pierwsze z Z
                    return i;
            return -1; //Nie ma takich liczb
        }

         int CalculateZ()
        {
            return ((P - 1) * (Q - 1));
        }

        int CalculateD()
        {
            for (int i = 1; i < Int32.MaxValue; i++)
            {

                if (i * E % Z == 1 && i!=E)
                    return i;
            }
            return -1; //Nie ma takiej liczby
        }

        

        List<int> GeneratePrimes(int limit)
        {
            List<int> primes = new List<int>
            {
                2,
                3
            };

            if (limit % 2 == 0)
                limit++;

            for (int i = 5; i <= limit; i += 2)
                if (IsPrime(i))
                    primes.Add(i);

            return primes;
        }

        static int BiggestCommonDivider(int a, int b)   //Algorytm Euklidesa do znajdowania największego wspólnego dzielnika
        {
            int c;
            while(b!=0)
            {
                c = a % b;
                a = b;
                b = c;
            }
            return a;
        }

         static bool IsPrime(int check)
        {
            if (check <= 1) return false;
            if (check == 2) return true;
            if (check % 2 == 0) return false;

            /* Sprawdzamy tylko liczby z przedziału <check, pierwiastekZCheck> dlatego
             że pierwiastek z check jest medianą liczb które dzielą liczbę check
             więc jeżeli istnieją trzy liczby dzielące check wystarczy sprawdzić tylko lewą stronę przediału

            Przykład
            check = 24
            sqrt(check) = ~4.9
            Dzielniki 24 to {1,2,3,4, |4.9| , 6,8,12,24}
            Można zauważyć że dzielników po lewej stronie przedziału i prawej stronie przedziału względem pierwiastka z check jest tyle samo.
             */

            int boundary = (int)Math.Floor(Math.Sqrt(check));

            for(int i = 3;i<=boundary;i+=2) //i+=2 ponieważ nie ma sensu sprawdzania liczb parzystych, ponieważ te zawsze będą dzielić się przez 2
                if (check % i == 0) return false;
            
            return true;
        }

        public List<int> EncodeMessage(string message)
        {
            List<int> codedLetters = new List<int>();
            for (int i = 0; i < message.Length; i++)
            {
                int letterinASCII = message[i];
                BigInteger calculationBigInt = BigInteger.Pow(letterinASCII, E) % N;
                codedLetters.Add((int)calculationBigInt);
            }
            return codedLetters;
        }

        public string DecodeMessage(List<int> codedList)
        {
            string message = "";
            foreach (var i in codedList)
            {

                char decodedChar = (char)(BigInteger.Pow(i, D) % N);
                message += decodedChar.ToString();
            }
            return message;
        }
    }
}
