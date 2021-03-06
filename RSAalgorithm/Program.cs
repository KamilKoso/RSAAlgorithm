﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RSAalgorithm
{
    class Program
    {
        private static void Main(string[] args)
        {
            RSA rsa = new RSA();

            Console.WriteLine(rsa.ReturnPrivateKey());
            Console.WriteLine(rsa.ReturnPublicKey());

            Console.WriteLine("Podaj twoją wiadomość do zakodowania");
            
          List<int> test =  rsa.EncodeMessage(Console.ReadLine());
            Console.WriteLine("---------------CODED MESSAGE-----------------");
            foreach(int i in test)
                Console.Write(i+", ");
            Console.WriteLine();
            Console.WriteLine("-------------CODED MESSAGE END-------------");
            Console.WriteLine();
            Console.WriteLine("Zdekodowana Wiadomość:");
            Console.WriteLine(rsa.DecodeMessage(test));
            Console.ReadKey();
        }
    }
}
