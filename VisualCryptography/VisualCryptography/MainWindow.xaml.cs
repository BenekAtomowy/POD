using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
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

namespace VisualCryptography
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        Bitmap bitmapImage;
        Bitmap part1, part2, output;
        System.Drawing.Color Black = System.Drawing.Color.FromArgb(0, 0, 0);
        System.Drawing.Color White = System.Drawing.Color.FromArgb(255, 255, 255);

        private void LoadPartOneButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.InitialDirectory = "C:\\Users";
                dlg.DefaultExt = "bmp";
                dlg.Filter =
            "Bitmapa (*.bmp)|*.bmp|All files (*.*)|*.*";
                Nullable<bool> result = dlg.ShowDialog();

                part1 = new Bitmap(dlg.FileName);

                PartOneBox.Source = new BitmapImage(new Uri(dlg.FileName));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Błąd!");
            }

        }

        private void LoadPartTwoButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.InitialDirectory = "C:\\Users";
                dlg.DefaultExt = "bmp";
                dlg.Filter =
            "Bitmapa (*.bmp)|*.bmp|All files (*.*)|*.*";
                Nullable<bool> result = dlg.ShowDialog();

                part2 = new Bitmap(dlg.FileName);

                PartTwoBox.Source = new BitmapImage(new Uri(dlg.FileName));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Błąd!");
            }
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            output = new Bitmap(2 * bitmapImage.Width, bitmapImage.Height);

            for (int i = 0; i < output.Width; i++)
            {
                for (int j = 0; j < output.Height; j++)
                {
                    System.Drawing.Color pixelPart1 = part1.GetPixel(i, j);
                    System.Drawing.Color pixelPart2 = part2.GetPixel(i, j);
                    if (pixelPart1 == White && pixelPart2 == White) output.SetPixel(i, j, White);
                    if (pixelPart1 == Black && pixelPart2 == Black) output.SetPixel(i, j, Black);
                    if (pixelPart1 == White && pixelPart2 == Black) output.SetPixel(i, j, Black);
                    if (pixelPart1 == Black && pixelPart2 == White) output.SetPixel(i, j, Black);
                }
            }
            output.Save(@"C:\Users\Public\output.bmp");
            OutputImage.Source = new BitmapImage(new Uri(@"C:\Users\Public\output.bmp"));
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            part1 = new Bitmap(2 * bitmapImage.Width, bitmapImage.Height);
            part2 = new Bitmap(2 * bitmapImage.Width, bitmapImage.Height);
         
           

            Random r = new Random();

            for (int j = 0; j < bitmapImage.Height; j++)
            {
                for (int i = 0; i<bitmapImage.Width; i++)
                {
                    int value = r.Next(0, 2);
                    if (bitmapImage.GetPixel(i,j) == White)
                    {
                        if (value == 1)
                        {
                            part1.SetPixel(2 * i, j, Black);
                            part1.SetPixel(2 * i + 1, j, White);
                            part2.SetPixel(2 * i, j, Black);
                            part2.SetPixel(2 * i + 1, j,White);
                        }
                        else
                        {
                            part1.SetPixel(2 * i, j, White);
                            part1.SetPixel(2 * i + 1, j, Black);
                            part2.SetPixel(2 * i, j, White);
                            part2.SetPixel(2 * i + 1, j, Black);
                        }

                    }
                    if (bitmapImage.GetPixel(i,j) == Black)
                    {
                        if (value == 1)
                        {
                            part1.SetPixel(2 * i, j, Black);
                            part1.SetPixel(2 * i + 1, j, White);
                            part2.SetPixel(2 * i, j, White);
                            part2.SetPixel(2 * i + 1, j, Black);
                        }
                        else
                        {
                            part1.SetPixel(2 * i, j, White);
                            part1.SetPixel(2 * i + 1, j, Black);
                            part2.SetPixel(2 * i, j, Black);
                            part2.SetPixel(2 * i + 1, j, White);
                        }
                    }



                }

            }
          
            part1.Save(@"C:\Users\Public\Part1.bmp");
            PartOneBox.Source = new BitmapImage(new Uri(@"C:\Users\Public\Part1.bmp"));
            part2.Save(@"C:\Users\Public\Part2.bmp");
            PartTwoBox.Source = new BitmapImage(new Uri(@"C:\Users\Public\Part2.bmp"));
        }

    

    private void LoadImageButton_Click(object sender, RoutedEventArgs e)
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

                ImageBox.Source = new BitmapImage(new Uri(dlg.FileName));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Błąd!");
            }
        }
    }
}
