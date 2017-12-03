using System;
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

namespace Szyfrator_Strumieniowy_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static public String character(int Letter)
        {
            String binChar;
            switch (Convert.ToChar(Letter))
            {
                case ' ':
                    return binChar = "000000";
                case ',':
                    return binChar = "011011";
                case '.':
                    return binChar = "011100";
                case '?':
                    return binChar = "011101";
                case '!':
                    return binChar = "011110";
                case '(':
                    return binChar = "011111";
                case ')':
                    return binChar = "100000";
                case ':':
                    return binChar = "111011";
                case '+':
                    return binChar = "111100";
                case '-':
                    return binChar = "111101";
                case '*':
                    return binChar = "111110";
                case '/':
                    return binChar = "111111";
                default:
                    binChar = Convert.ToString(Letter, 2);
                    binChar = binChar.Remove(0, 1);
                    return binChar;
            }
        }
        public static String xor(String a, String b)
        {
            String wynik = "";
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == b[i]) wynik = wynik + "0";
                else if (a[i] != b[i]) wynik = wynik + "1";
            }
            return wynik;
        }
        public static String[] generateAlphabetBin(char[] alphabet)
        {
            String[] alphabetBin = new String[alphabet.Length];
            for (int i = 0; i < alphabet.Length; i++)
            {
                alphabetBin[i] = character(Convert.ToInt32(alphabet[i]));
                //Console.Write(alphabetBin[i] + " ");
            }
            return alphabetBin;
        }
        public static String[] generateTextByte(String text)
        {
            int[] textInt = new int[text.Length];
            String[] textByte = new String[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                textInt[i] = Convert.ToInt32(text[i]);
                textByte[i] = character(text[i]);
            }
            return textByte;
        }
        public static String[] encrypt(String[] textByte, String[] keyBin)
        {
            String[] encryptedTextBin = new String[textByte.Length];
            for (int i = 0; i < encryptedTextBin.Length; i++)
            {
                encryptedTextBin[i] = xor(textByte[i], keyBin[i]);
            }
            return encryptedTextBin;

        }
        public static String findLetters(String[] encryptedTextBin, String[] alphabetBin, char[] alphabet)
        {

            String encryptedText = "";
            for (int i = 0; i < encryptedTextBin.Length; i++)
            {
                for (int j = 0; j < alphabetBin.Length; j++)
                {
                    if (encryptedTextBin[i] == alphabetBin[j])
                    {
                        encryptedText = encryptedText + alphabet[j];
                    }
                }
            }
            return encryptedText;
        }
        public static int[] readKey(String FILE_NAME)
        {

            String buffer = "";
            using (FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader r = new StreamReader(fs))
                {

                    buffer = r.ReadToEnd(); ;
                }
            }
            size = buffer.Length;
            int[] array = new int[size];
            for (int i = 0; i < size; i++)
            {
                array[i] = Convert.ToInt16(buffer[i]) - 48;
            }
            return array;
        }
        public static String[] genertateKeyBinTab(int[] key)
        {
            String[] keyBin = new String[size / 6 + 1];
            int l = 0;

            for (int i = 0; i < keyBin.Length; i++)
            {
                keyBin[i] = "";
            }
            for (int i = 0; i < key.Length; i++)
            {
                keyBin[l] = keyBin[l] + key[i];
                if (keyBin[l].Length == 6) l++;

            }
            return keyBin;
        }
        public MainWindow()
        {
            InitializeComponent();
        }
        static int size;
        int[] key;
        String text;
        String encryptedText;
        char[] alphabet = { 'A', 'a', 'B', 'b', 'C', 'c', 'D', 'd', 'E', 'e', 'F', 'f', 'G', 'g', 'H', 'h', 'I', 'i', 'J', 'j', 'K', 'k', 'L',
                                'l', 'M', 'm', 'N', 'n', 'O', 'o', 'P', 'p', 'Q', 'q', 'R', 'r', 'S', 's', 'T', 't', 'U', 'u', 'V', 'v', 'W', 'w',
                                'X', 'x', 'Y', 'y', 'Z', 'z', ' ', ',', '.', '?', '!', '(', ')', ':', '+', '-', '/', '*' };
        String[] alphabetBin;
        String[] keyBin;
        String decryptedText;
        String[] encryptedTextBin;


        private void readKeyButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = "C:\\Users\\Osa\\Source\\Repos\\POD\\Szyfrator Strumieniowy\\Szyfrator Strumieniowy\\bin\\Debug";
            dlg.DefaultExt = "*.txt";
            Nullable<bool> result = dlg.ShowDialog();
            if(result == true)
            {
                key = readKey(dlg.FileName);
                
                String keyString="";
                for (int i= 0; i <10000; i++)
                {
                    keyString = keyString + key[i];
                }
                MessageBox.Show("    Wczytano klucz");
                PrintKeyTextBox.Text = keyString;
                alphabetBin = generateAlphabetBin(alphabet);
                keyBin = genertateKeyBinTab(key);
            }
            else MessageBox.Show("    Błąd!");



        }

        private void LoadTextButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = "C:\\Users";
            dlg.DefaultExt = "*.txt";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(dlg.FileName))
                {
                    // Read the stream to a string, and write the string to the console.
                    text = sr.ReadToEnd();
                    TextBox.Text = text;
                }
               
            }
            else MessageBox.Show("Błąd!");
        }

        private void EncryptTextButton_Click(object sender, RoutedEventArgs e)
        {
           
                String[] textByte = generateTextByte(TextBox.Text);
                encryptedTextBin = encrypt(textByte, keyBin);
                encryptedText = findLetters(encryptedTextBin, alphabetBin, alphabet);
                EncryptedTextBox.Text = encryptedText;
                EncryptedTextBox_Copy.Text = encryptedText;
            
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            String[] decryptedTextByte = generateTextByte(EncryptedTextBox_Copy.Text);
            String[] decryptedTextBin = encrypt(decryptedTextByte, keyBin);
            decryptedText = findLetters(decryptedTextBin, alphabetBin, alphabet);
            DecryptedTextBox.Text = decryptedText;
        }

        private void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = "C:\\Users";
            dlg.DefaultExt = "*.txt";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(dlg.FileName))
                {
                    // Read the stream to a string, and write the string to the console.
                     
                    EncryptedTextBox_Copy.Text = sr.ReadToEnd(); 
                }

            }
            else MessageBox.Show("Błąd!");
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
            
            System.IO.File.WriteAllText(@"C:\Users\Public\Documents\WriteText.txt", encryptedText);
        }
    }
}
