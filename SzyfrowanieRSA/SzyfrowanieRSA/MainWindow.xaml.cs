using System;
using System.Collections.Generic;
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
using System.Numerics;
using System.Collections;

namespace SzyfrowanieRSA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int q, p;
        private int n, phi;
        private int x, d;
        String text;

        private int nwd()
        {
            int ax, bx, t; 
            for(int i= 2; i<phi; i++)
            {
                ax = i;
                bx = phi;
                while(bx != 0)
                {
                    t = bx;
                    bx = ax % bx;
                    ax = t;
                }
                if (ax == 1) return i;
            }
            return 0;
        }

        private int findD()
        {
            int i = 1;
            while(((x * i)- 1)% phi != 0)
            {
                i++;
            }
            return i;
        }

       

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                q = Convert.ToInt32(QTextbox.Text);
                p = Convert.ToInt32(PTextbox.Text);

                n = q * p;
                phi = (q - 1) * (p - 1);
                NTextbox.Text = n.ToString();
                PhiTextbox.Text = phi.ToString();

                x = nwd();
                ETextbox.Text = x.ToString();

                d = findD();
                DTextbox.Text = d.ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(),"Błąd");
            }
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            text = Textbox.Text;
            BigInteger[] textAscii = new BigInteger[text.Length];
            BigInteger[] encryptedAscii = new BigInteger[text.Length];
            String encryptedText = "";
            for (int i = 0; i < text.Length; i++)
            {
                textAscii[i] = new BigInteger(text[i]);
                encryptedAscii[i] =(BigInteger.Pow(textAscii[i], x)% n);

                encryptedText = encryptedText +","+ encryptedAscii[i];
                
            }
            EncryptedTextbox.Text = encryptedText;
            EncryptedTextboxCopy.Text = EncryptedTextbox.Text;
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            String encryptedText = EncryptedTextboxCopy.Text;
            List<BigInteger> encryptedList = new List<BigInteger>();
            String letter = "";
            for (int i = 1; i<encryptedText.Length; i++)
            {
                
                if (i == 0)
                {}
                else if (encryptedText[i] != ',')
                {
                    letter = letter + encryptedText[i];
                }
                else if(encryptedText[i] ==',' && i >0)
                {
                    encryptedList.Add(new BigInteger(Int32.Parse(letter)));
                    letter = "";
                }
            }
            BigInteger[] encryptedAscii = new BigInteger[encryptedList.Count];
            BigInteger[] decryptedText = new BigInteger[encryptedAscii.Length];
            encryptedAscii = encryptedList.ToArray();
            for (int i = 0; i<encryptedAscii.Length; i++)
            {
                decryptedText[i] =(BigInteger.Pow(encryptedAscii[i], d)) % n;
            }



        }

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
