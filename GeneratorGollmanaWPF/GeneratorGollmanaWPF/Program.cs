using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeneratorGollmana
{
    class LFSR
    {
        private int[] rej;
        private int o1, o2, o3;
        Random rnd = new Random();
        public LFSR(int m)
        {
            rej = new int[m];
            for (int i= 0; i < m; i++) 
            {
                Thread.Sleep(2);
                rej[i] = rnd.Next(0, 2);
                
            }
            for (int i = 0; i < m; i++)
            {
                Console.Write(rej[i]);
            }
            o1 = rnd.Next(0, m-1);
            o2 = rnd.Next(0, m-1);
            o3 = rnd.Next(0, m-1);
            while (o2 == o1) { o2 = rnd.Next(0, m-1); }
            while (o3 == o1||o3 == o2) { o3 = rnd.Next(0, m-1); }
            Console.Write(" " +o1 + " "+ o2+ " "+ o3+"\n");
        }
        public int[] get_rejestr()
        {
            return rej;
        }
        public static int xor(int a, int b)
        {
            if ((a == 1 && b == 1) || (a == 0 && b == 0))
            {
                return 0;
            }
            else if ((a == 1 && b == 0) || (a == 0 && b == 1))
            {
                return 1;
            }
            else return -1;
        }
        public int geto1() { return o1; }
        public int geto2() { return o2; }
        public int geto3() { return o3; }
        private void check(int[] buf, int[]rej)
        {

            for (int i = 1; i < rej.Length; i++)
                if (rej[0] == buf[buf.Length - 1] && rej[i] == buf[i - 1]) { Console.WriteLine("TRUE"); }
                else Console.WriteLine("FALSE");
        }
        public int shift(int flag)
        {
                int[] buf = new int[rej.Length];
                int wyj;
                wyj = rej[rej.Length - 1];
                for (int i = 0; i < rej.Length; i++)
                {
                    buf[i] = rej[i];
                }

                buf[o1] = xor(buf[o1], wyj);
                buf[o2] = xor(buf[o2], wyj);
                buf[o3] = xor(buf[o3], wyj);
                for (int i = 1; i < rej.Length; i++)
                {
                    rej[i] = buf[i - 1];

                }

                rej[0] = wyj;


               //for (int i = 0; i < rej.Length; i++) Console.Write(rej[i]);
              // Console.Write("\n");
               return wyj;
           



        }

    }

    
    class Program
    {
        public static int and(int a, int b)
        {
            if ((a == 1 && b == 0) || (a == 0 && b == 0) || (a == 0 && b == 1))
            {
                return 0;
            }
            else if ((a == 1 && b == 1) )
            {
                return 1;
            }
            else return -1;
        }
        public static int xor(int a, int b)
        {
            if ((a == 1 && b == 1) || (a == 0 && b == 0))
            {
                return 0;
            }
            else if ((a == 1 && b == 0) || (a == 0 && b == 1))
            {
                return 1;
            }
            else return -1;
        }

        public static LFSR[] generateLfsr(int n, int m)
        {
            LFSR[] reg = new LFSR[n];
            for (int i = 0; i<n; i++)
            {
                reg[i] = new LFSR(m);
            }
            return reg;
        }
        public static int cascade(LFSR[] registers)
        {

            //Console.WriteLine("Zaczęto shifta");
            int lfsr1output = registers[0].shift(1);
            int xor1 = xor(1, lfsr1output);
            int and1 = and(xor1, registers[0].shift(1));
            for (int i = 1; i < registers.Length; i++)
            {
                xor1 = xor(xor1, registers[i].shift(and1));
                and1 = and(xor1, registers[i].shift(i));
    
            }
            return registers[registers.Length-1].shift(and1);
        }

       
    }
}
