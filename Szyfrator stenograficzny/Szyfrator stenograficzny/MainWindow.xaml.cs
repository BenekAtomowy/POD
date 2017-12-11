using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace Szyfrator_stenograficzny
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public static String[] generateTextByte(String text)
        {
            int[] textInt = new int[text.Length];
            String[] textByte = new String[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                textInt[i] = Convert.ToInt32(text[i]);
                textByte[i] = Convert.ToString(text[i], 2).PadLeft(7,'0');
                
            }
            return textByte;
        }
        public static int[] encrpyt (int[] pixelsTab, String[] textByte)
        {
          
            String[] pixelsByte = new String[pixelsTab.Length];
            int [] pixelsInt = new int[pixelsTab.Length];
            for (int j = 0; j < textByte.Length; j++)
            {

                for (int i = 0; i < pixelsTab.Length; i++)
                {

                    pixelsByte[i] = Convert.ToString(pixelsTab[i], 2).PadLeft(7,'0');
                    pixelsByte[i] = pixelsByte[i].Remove(pixelsByte[i].Length - 1);
                    pixelsByte[i] = pixelsByte[i].Insert(pixelsByte[i].Length - 1, textByte[j][i].ToString());
                    
                    pixelsInt[i] = Convert.ToInt32(pixelsByte[i],2);
                    pixelsInt[i] = pixelsInt[i] ;


                }
            }
            return pixelsInt;
        }


        public MainWindow()
        {
            InitializeComponent();

        }
        String text;
        Bitmap bitmapImage;
        Bitmap encryptedBitmapImage;
        Bitmap encryptedBitmap;
        private void LoadTextFileButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.InitialDirectory = "C:\\Users";
                dlg.DefaultExt = "txt";
                dlg.Filter =
            "Pliki tekstowe (*.txt)|*.txt|All files (*.*)|*.*";
                Nullable<bool> result = dlg.ShowDialog();
                
                
                    // Open the text file using a stream reader.
                    using (StreamReader sr = new StreamReader(dlg.FileName))
                    {
                        // Read the stream to a string, and write the string to the console.
                        text = sr.ReadToEnd();
                        textBox.Text = text;
                    }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),"Błąd!" );
            }
        }

        private void LoadBitmapButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.InitialDirectory = "C:\\Users";
                dlg.DefaultExt = "bmp";
                dlg.Filter =
            "Bitmapa (*.bmp)|*.bmp|All files (*.*)|*.*";
                Nullable<bool> result = dlg.ShowDialog();

                bitmapImage = new Bitmap(dlg.FileName);

                image.Source = new BitmapImage(new Uri(dlg.FileName));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Błąd!");
            }
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                
                String text = textBox.Text;
                if (text.Length > (bitmapImage.Height * bitmapImage.Width)/3)
                {
                    MessageBox.Show("Przekoroczono zakres wielkości tekstu", "Błąd!");
                    return;
                }
                String[] textByte = generateTextByte(text);
                
                
                encryptedBitmap = bitmapImage;

                int licznik = 0;

              
                    for (int y = 1; y < encryptedBitmap.Height; y++)
                    {
                    if (licznik >= textByte.Length) break;
                    else
                    {
                        for (int x = 1; x < encryptedBitmap.Width - 3; x = x + 3)
                        {
                            if (licznik >= textByte.Length) break;
                            else
                            {
                                int[] pixelsTab = new int[7];
                                pixelsTab[0] = bitmapImage.GetPixel(x, y).R;
                                pixelsTab[1] = bitmapImage.GetPixel(x, y).G;
                                pixelsTab[2] = bitmapImage.GetPixel(x, y).B;
                                pixelsTab[3] = bitmapImage.GetPixel(x + 1, y).R;
                                pixelsTab[4] = bitmapImage.GetPixel(x + 1, y).G;
                                pixelsTab[5] = bitmapImage.GetPixel(x + 1, y).B;
                                pixelsTab[6] = bitmapImage.GetPixel(x + 2, y).R;
                              
                                pixelsTab = encrpyt(pixelsTab, textByte);

                                encryptedBitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(255, pixelsTab[0], pixelsTab[1], pixelsTab[2]));
                                encryptedBitmap.SetPixel(x + 1, y, System.Drawing.Color.FromArgb(255, pixelsTab[3], pixelsTab[4], pixelsTab[5]));
                                encryptedBitmap.SetPixel(x + 2, y, System.Drawing.Color.FromArgb(255, pixelsTab[6], bitmapImage.GetPixel(x + 2, y).G, bitmapImage.GetPixel(x + 2, y).B));
                                licznik = licznik + 1;
                            }
                        }
                    }

                    }
                
                Bitmap encryptedBitmapTemp = encryptedBitmap;
                
                encryptedBitmapTemp.Save("C:\\Users\\Public\\Pictures\\temp.bmp");
                EncryptedImage.Source = new BitmapImage(new Uri("C:\\Users\\Public\\Pictures\\temp.bmp"));
                EncryptedImage_Copy.Source = new BitmapImage(new Uri("C:\\Users\\Public\\Pictures\\temp.bmp"));
                TestTextBox.Text = "Zakończono tworzenie";
                
          
             }
             catch(Exception ex)
            {
                 MessageBox.Show(ex.ToString(), "Błąd!");
            }
        }

        public static char decrypt(int [] pixelsTab)
        {
            String charBin="";
            char letter;
            String[] textByte = new String[pixelsTab.Length];

            for (int i = 0; i < pixelsTab.Length; i++)
            {
                pixelsTab[i] = pixelsTab[i] ;
                textByte[i] = Convert.ToString(pixelsTab[i], 2).PadLeft(7, '0');

            }
            charBin =  textByte[0][textByte[0].Length-1].ToString() + textByte[1][textByte[1].Length - 1].ToString() + textByte[2][textByte[2].Length - 1].ToString() + textByte[3][textByte[3].Length - 1].ToString() + textByte[4][textByte[4].Length - 1].ToString() + textByte[5][textByte[5].Length - 1].ToString() + textByte[6][textByte[6].Length - 1].ToString();
            letter = Convert.ToChar(Convert.ToInt32(charBin,2));
            return letter;
        }
        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            encryptedBitmapImage = encryptedBitmap;
            String text = "";
            int licznik = 0;
            String[] textByte = new String[10];
            
            for (int y = 1; y < encryptedBitmapImage.Height; y++)
            {
                if (licznik >= textByte.Length) break;
                else
                {
                    for (int x = 1; x < encryptedBitmapImage.Width - 3; x = x + 3)
                    {
                        if (licznik >= textByte.Length) break;
                        else
                        {
                            int[] pixelsTab = new int[7];
                            
                            pixelsTab[0] = encryptedBitmapImage.GetPixel(x, y).R;
                            pixelsTab[1] = encryptedBitmapImage.GetPixel(x, y).G;
                            pixelsTab[2] = encryptedBitmapImage.GetPixel(x, y).B;
                            pixelsTab[3] = encryptedBitmapImage.GetPixel(x + 1, y).R;
                            pixelsTab[4] = encryptedBitmapImage.GetPixel(x + 1, y).G;
                            pixelsTab[5] = encryptedBitmapImage.GetPixel(x + 1, y).B;
                            pixelsTab[6] = encryptedBitmapImage.GetPixel(x + 2, y).R;
                            
                           text = text + decrypt(pixelsTab);

                            licznik = licznik + 1;

                        }

                    }
                }
            }
            decryptedTextBox.Text = text;


                    }

        private void LoadEncryptedBitmapButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.InitialDirectory = "C:\\Users\\Public";
                dlg.DefaultExt = "bmp";
                dlg.Filter =
            "Bitmapa (*.bmp)|*.bmp|All files (*.*)|*.*";
                Nullable<bool> result = dlg.ShowDialog();

                encryptedBitmapImage = new Bitmap(dlg.FileName);

                EncryptedImage_Copy.Source = new BitmapImage(new Uri(dlg.FileName));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Błąd!");
            }
        }
    }
}
