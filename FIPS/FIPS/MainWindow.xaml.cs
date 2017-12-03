using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

namespace FIPS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public static int[] readKey(String FILE_NAME)
        {

            String buffer = "";
            using (FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader r = new StreamReader(fs))
                {
                    while(buffer.Length != 20000)
                    buffer = buffer + (r.Read()-48) ;
                }
            }
            int size = buffer.Length;
            int[] array = new int[size];
            for (int i = 0; i < size; i++)
            {
                array[i] = Convert.ToInt16(buffer[i]) - 48;
            }
            return array;
        }
        public MainWindow()
        {
            InitializeComponent();
        }
        int [] key;
        

        private void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            String keyString = "";
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = "C:\\Users\\Osa\\Source\\Repos\\POD\\Szyfrator Strumieniowy\\Szyfrator Strumieniowy\\bin\\Debug";
            dlg.DefaultExt = "*.txt";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                key = readKey(dlg.FileName);
                    for (int i = 0; i < key.Length; i++)
                {
                    keyString = keyString + key[i];
                }
                MessageBox.Show("    Wczytano klucz");
            }
            else MessageBox.Show("    Błąd!");
            
            KeyTextBox.Text = keyString;
            SerieTestTextBox.Text = "";
            PokerTestTextBox.Text = "";
            OneBitTestTextBox.Text = "";
            LongSerieTestTextBox.Text = "";
        }

        private void OneBitTestButton_Click(object sender, RoutedEventArgs e)
        {
            int wynik = 0;
            for (int i = 0; i<key.Length; i++)
            {
                if (key[i] == 1) wynik++;
            }
            if (wynik < 10275 && wynik > 9725) OneBitTestTextBox.Text = "Pozytywny";
            else OneBitTestTextBox.Text = "Negatywny";
            OneBitTestDetails.Text = wynik.ToString();
        }

        private void SerieTestButton_Click(object sender, RoutedEventArgs e)
        {
            int[] serie = { 0,0,0,0,0,0};
            ArrayList buf = new ArrayList(); 
            for(int i = 0; i< key.Length-1;i++)
            {
                if(key[i] == key[i + 1])
                {
                    buf.Add(key[i]);
                }
                else
                {
                    if (buf.Count == 1) serie[0] = serie[0] + 1;
                    else if (buf.Count == 2) serie[1] = serie[1] + 1;
                    else if (buf.Count == 3) serie[2] = serie[2] + 1;
                    else if (buf.Count == 4) serie[3] = serie[3] + 1;
                    else if (buf.Count == 5) serie[4] = serie[4] + 1;
                    else if (buf.Count >=6) serie[5] = serie[5] + 1;
                    buf.Clear();
                }

            }

            if(serie[0]<=2685 && serie[0] >=2315 &&
                serie[1] <= 1386 && serie[1] >= 1114 &&
                serie[2]<= 723 && serie[2]>= 527 &&
                serie[3] <= 384 && serie[3] >= 240 &&
                serie[4] <= 209 && serie[4] >= 103 &&
                serie[5] <= 209 && serie[5] >= 103)
            {
                SerieTestTextBox.Text = "Pozytywny";
            }
            else SerieTestTextBox.Text = "Negatywny";

            SerieTestDetails.Text = serie[0].ToString() + ", " + serie[1].ToString() + ", " + serie[2].ToString() + ", " + serie[3].ToString() + ", " + serie[4].ToString() + ", " + serie[5].ToString();
        }

        private void LongSerieButton_Click(object sender, RoutedEventArgs e)
        {
            int z = 0, j = 0, maxz = 0, maxj = 0;
            for (int i = 0; i < key.Length - 1; i++)
            {
                if (key[i] == 0 && key[i + 1] == 0)
                {
                    z = z + 1;
                    if (z > maxz)
                        maxz = z;
                }
                else if (key[i] == 0 && key[i + 1] == 1)
                    z = 0;
            }
            for (int i = 0; i < key.Length - 1; i++)
            {
                if (key[i] == 1 && key[i + 1] == 1)
                {
                    j = j + 1;
                    if (j > maxj)
                        maxj = j;
                }
                else if (key[i] == 0 && key[i + 1] == 1)
                    j = 0;
            }
            if (maxj >= 26 || maxz >= 26)
                LongSerieTestTextBox.Text = "Negatywny";
            else LongSerieTestTextBox.Text = "Pozytywny";

            LongSerieDetails.Text = "Liczba jedynek: " + maxj + " Liczba zer: " + maxz; 
        }

        private void PokerTestButton_Click(object sender, RoutedEventArgs e)
        {
            String[] seg = new String[5000];
            int j = 0;

            for ( int i=0; j<20000;i++)
            {
                seg[i] = "";
                seg[i] = seg[i] + key[j] + key[j+1] + key[j+2] + key[j+3];
                j = j+4;
            }
            String[] tab = { "0000","0001","0010","0011", "0100", "0101", "0110", "0111", "1000", "1001", "1010", "1011", "1100", "1101", "1110", "1111" };
            int[] results = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            for (int i = 0; i<seg.Length; i++)
            {
                for (int l = 0; l < tab.Length; l++)
                {
                    if (seg[i] == tab[l]) results[l] = results[l] + 1;
                }
            }
            double x=0, w=0;
            for(int i = 0; i < results.Length; i++)
            {
                w = w + results[i] * results[i];
            }
            x = ((16 * w)/ 5000) - 5000;
            if(x>2.16&&x<46.17)
            PokerTestTextBox.Text = "Pozytywny";
            else PokerTestTextBox.Text = "Negatywny";

            PokerTestDetails.Text = results[0].ToString() + ", "+ results[1].ToString() + ", " + results[2].ToString() + ", " +
                results[3].ToString() + ", " + results[4].ToString() + ", " + results[5].ToString() + ", " + results[6].ToString() + ", " + results[7].ToString() + ", " +
                results[8].ToString() + ", " + results[9].ToString() + ", " + results[10].ToString() + ", " + results[11].ToString() + ", " + results[12].ToString() + ", " +
                results[13].ToString() + ", " + results[14].ToString() + ", " + results[15].ToString() + " X = " + x; 

        }

        private void ExecuteAllTestsButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
