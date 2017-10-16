using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//ASCII 97-122 a-z  105-i 106-j

namespace Playfair
{   
    

    class Playfair
    {
        char[] alfabet = new char[25] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        char[,] tab = new char[5, 5];

        void findAndChange(char k, int c)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (tab[i, j] == k)
                    {
                       
                    }
                }
            }
        }

        public Playfair(string key)
        {
            int c = 0;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                    tab[i, j] = alfabet[c];
                    c++;

                    }
                }




        }
        public void show()
        {
            Console.WriteLine("|" + tab[0, 0] + "|" + tab[0, 1] + "|" + tab[0, 2] + "|" + tab[0, 3] + "|" + tab[0, 4] + "|");
            Console.WriteLine("|" + tab[1, 0] + "|" + tab[1, 1] + "|" + tab[1, 2] + "|" + tab[1, 3] + "|" + tab[1, 4] + "|");
            Console.WriteLine("|" + tab[2, 0] + "|" + tab[2, 1] + "|" + tab[2, 2] + "|" + tab[2, 3] + "|" + tab[2, 4] + "|");
            Console.WriteLine("|" + tab[3, 0] + "|" + tab[3, 1] + "|" + tab[3, 2] + "|" + tab[3, 3] + "|" + tab[3, 4] + "|");
            Console.WriteLine("|" + tab[4, 0] + "|" + tab[4, 1] + "|" + tab[4, 2] + "|" + tab[4, 3] + "|" + tab[4, 4] + "|");


        }



    }


    class Program
    {
        static void Main(string[] args)
        {
            Playfair szyfr = new Playfair("klucz");
            szyfr.show();
            Console.ReadKey();
        }
    }
}
