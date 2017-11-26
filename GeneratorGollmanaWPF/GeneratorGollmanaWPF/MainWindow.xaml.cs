using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GeneratorGollmanaWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    class LFSR
    {
        private int[] rej;
        private int o1, o2, o3;
        Random rnd = new Random();
        public LFSR(String text)
        {
            rej = new int[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                rej[i] = Convert.ToInt16(text[i]-48);
            }
            o1 = rnd.Next(0, text.Length - 1);
            o2 = rnd.Next(0, text.Length - 1);
            o3 = rnd.Next(0, text.Length - 1);
            while (o2 == o1) { o2 = rnd.Next(0, text.Length - 1); }
            while (o3 == o1 || o3 == o2) { o3 = rnd.Next(0, text.Length - 1); }
        }
        public LFSR(int m)
        {
            rej = new int[m];
            for (int i = 0; i < m; i++)
            {
                Thread.Sleep(2);
                rej[i] = rnd.Next(0, 2);

            }
            for (int i = 0; i < m; i++)
            {
                Console.Write(rej[i]);
            }
            o1 = rnd.Next(0, m - 1);
            o2 = rnd.Next(0, m - 1);
            o3 = rnd.Next(0, m - 1);
            while (o2 == o1) { o2 = rnd.Next(0, m - 1); }
            while (o3 == o1 || o3 == o2) { o3 = rnd.Next(0, m - 1); }
            Console.Write(" " + o1 + " " + o2 + " " + o3 + "\n");
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
        private void check(int[] buf, int[] rej)
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

    public partial class MainWindow : Window
    {
        public static int and(int a, int b)
        {
            if ((a == 1 && b == 0) || (a == 0 && b == 0) || (a == 0 && b == 1))
            {
                return 0;
            }
            else if ((a == 1 && b == 1))
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
        static LFSR[] generateLfsr(int n, int m)
        {
            LFSR[] reg = new LFSR[n];
            for (int i = 0; i < n; i++)
            {
                reg[i] = new LFSR(m);
            }
            return reg;
        }


        public MainWindow()
        {
            InitializeComponent();
        }
        LFSR[] registers;
        String key = "";

        private void GenerateLFSRbutton_Click(object sender, RoutedEventArgs e)
        {
            
            if (ntextbox.Text.Count() == 0  || mtextbox.Text.Count() == 0 || Convert.ToInt16(mtextbox.Text)<=3) MessageBox.Show("Wypełnij pola\nMinimalna długość rejestru to 4");
            else
            {
                registers = generateLfsr(Convert.ToInt16(ntextbox.Text), Convert.ToInt16(mtextbox.Text));
                MessageBox.Show("Utworzono rejestry LFSR");

                String rejestry_do_wyswietlenia = "";
                for (int i = 0; i < registers.Length; i++)
                {
                    rejestry_do_wyswietlenia = rejestry_do_wyswietlenia + "LFSR-" + (i+1) +" : ";
                    for (int j = 0; j < registers[i].get_rejestr().Length; j++)
                        rejestry_do_wyswietlenia = rejestry_do_wyswietlenia + registers[i].get_rejestr()[j];
                    rejestry_do_wyswietlenia = rejestry_do_wyswietlenia +" Odczepy: "+ (registers[i].geto1()+1) +", " + (registers[i].geto2()+1) + ", " + (registers[i].geto3()+1) + ", " + "\n" ;

                }
                PrintLFSRTextBox.Text = rejestry_do_wyswietlenia;
            }
        }

        private void PrintLFSRTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
       
        static int cascade(LFSR[] registers)
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
            return registers[registers.Length - 1].shift(and1);
        }
  

        private void GenerateKeyButton_Click(object sender, RoutedEventArgs e)
        {
            key = "";
            if (Convert.ToInt64(KeyLengthTextBox.Text) > 1000000 || Convert.ToInt64(KeyLengthTextBox.Text)<1||KeyLengthTextBox.Text==null) MessageBox.Show("Przekroczony zakres. Podaj liczbę z zakresu 1-1 000 000");
            else if (KeyLengthTextBox.Text.Count() == 0) { MessageBox.Show("Podaj ilość znaków"); }
            else if (registers == null) MessageBox.Show("Najpierw wygeneruj rejestry");
            else
            {

                for (int i = 0; i < Convert.ToInt64(KeyLengthTextBox.Text); i++)
                {
                    key = key + cascade(registers);
                }
                PrintKeyTextBox.Text = key;

            }
        }

        private void SaveKeyButton_Click(object sender, RoutedEventArgs e)
        {
            String path = @"GeneratedKey.txt";
            System.IO.File.WriteAllText(path, key);
            MessageBox.Show("Zapisano klucz do pliku w aktualnym katalogu");
        }

        private void CreditsButton_Click(object sender, RoutedEventArgs e)
        {
            
            String about = "Algorytm generatora kaskadowego polega na pobieraniu najmłodszego bitu z rejestru przesuwnego LFSR. Rejestr przy każdym użyciu przesuwa elementy w środku. Do tego stosuje opercacje XOR na 3 wybranych losowo elementach. Najmłodszy bit staje się wejściem bramki XOR, z której wynik kierowany jest do kolejnej bramki XOR, oraz bramki AND. Wynik z bramki AND wchodzi do kolejnego rejestru LFSR, natomiast wynik z rejestru LFSR kierowany jest na drugie wejśćie drugiej bramki XOR. Dalej schemat się powtarza aż do ostatniego rejestru LFSR, z którego wynik jest otrzymaną losową liczbą\n\n"+
                "Program jest ograniczony do 1000000 możliwych znaków, oraz posiada podstawowe zabezpieczenia przed wpisaniem błędnych danych. Generowanie 1000000 znaków trwa bardzo długo, przez co nie zaleca się używania tak wielkiej liczby. Dodatkowo zostało zaimplementowane generowanie ciągu znaków za pomocą systemowego generatora Random\n"+
                "Plik zapisywany jest w katalogu programu\n\n"+"Autor: Emilian Ossowski";
            MessageBox.Show(about);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt64(KeyLengthTextBox.Text) > 1000010 || Convert.ToInt64(KeyLengthTextBox.Text) < 1 || KeyLengthTextBox.Text == "") MessageBox.Show("Przekroczony zakres. Podaj liczbę z zakresu 1-1 000 000");
            else if (KeyLengthTextBox.Text.Count() == 0) { MessageBox.Show("Podaj ilość znaków"); }
            else if (registers == null) MessageBox.Show("Najpierw wygeneruj rejestry!");
            else
            {
                Random rnd = new Random();
                for (int i = 0; i < Convert.ToInt64(KeyLengthTextBox.Text); i++)
                {
                    key = key + rnd.Next(0, 2);
                }
               // PrintKeyTextBox.Text = key;
            }
        }

        private void AddLFSRButton_Click(object sender, RoutedEventArgs e)
        {
            List<LFSR> lista = null;
            if (registers == null)
            {
                lista = new List<LFSR>();
                lista.Add(new LFSR(AddLFSRTextBox.Text));
            }
            else
            {
                lista = registers.ToList();

                lista.Add(new LFSR(AddLFSRTextBox.Text));
            }

            registers = lista.ToArray();
            
            String rejestry_do_wyswietlenia = "";
            for (int i = 0; i < registers.Length; i++)
            {
                rejestry_do_wyswietlenia = rejestry_do_wyswietlenia + "LFSR-" + (i + 1) + " : ";
                for (int j = 0; j < registers[i].get_rejestr().Length; j++)
                    rejestry_do_wyswietlenia = rejestry_do_wyswietlenia + registers[i].get_rejestr()[j];
                rejestry_do_wyswietlenia = rejestry_do_wyswietlenia + " Odczepy: " + (registers[i].geto1() + 1) + ", " + (registers[i].geto2() + 1) + ", " + (registers[i].geto3() + 1) + ", " + "\n";

            }
            PrintLFSRTextBox.Text = rejestry_do_wyswietlenia;
        }

        private void ResetLFSRButton_Click(object sender, RoutedEventArgs e)
        {
            registers = null;
            String rejestry_do_wyswietlenia = "";
            
            PrintLFSRTextBox.Text = rejestry_do_wyswietlenia;
        }
    }
}
