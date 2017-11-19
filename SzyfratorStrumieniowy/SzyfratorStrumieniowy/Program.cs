using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzyfratorStrumieniowy
{
    
    class Program
    {
        static String FILE_NAME = "GeneratedKey.txt";
        static int size;
        public static int[] readKey()
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
                array[i] = Convert.ToInt16(buffer[i]) -48;
            }
            return array;
        }

        static void printKey(int [] key)
        {
            for (int i = 0; i < size; i++)
            {
                Console.Write(key[i]);
            }
        }

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
            for (int i=0; i<a.Length; i++)
            {
                if (a[i] == b[i]) wynik = wynik + "0";
                else  if(a[i] != b[i])wynik = wynik + "1";
            }
            return wynik;
        }
        public static String[] genertateKeyBinTab(int [] key)
            {
            String[] keyBin = new String[size / 6 + 1];
            int l = 0;
            
            for(int i = 0; i<keyBin.Length; i++)
            {
                keyBin[i] = "";
            }
            for(int i = 0; i<key.Length; i++)
            {
                keyBin[l] = keyBin[l] + key[i];
                if (keyBin[l].Length == 6) l ++;

            }
            return keyBin; 
            }
        public static String[] generateAlphabetBin(char [] alphabet)
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
        public static String[] encrypt(String[] textByte, String [] keyBin)
        {
            String[] encryptedTextBin = new String[textByte.Length];
            for (int i = 0; i < encryptedTextBin.Length; i++)
            {
                encryptedTextBin[i] = xor(textByte[i], keyBin[i]);
            }
            return encryptedTextBin;

        }
        public static String findLetters(String[] encryptedTextBin, String[] alphabetBin, char [] alphabet)
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
        


        static void Main(string[] args)
        {
            int[] key = new int[size];
            key = readKey();
            String[] keyBin = genertateKeyBinTab(key);

            char[] alphabet = { 'A', 'a', 'B', 'b', 'C', 'c', 'D', 'd', 'E', 'e', 'F', 'f', 'G', 'g', 'H', 'h', 'I', 'i', 'J', 'j', 'K', 'k', 'L',
                                'l', 'M', 'm', 'N', 'n', 'O', 'o', 'P', 'p', 'Q', 'q', 'R', 'r', 'S', 's', 'T', 't', 'U', 'u', 'V', 'v', 'W', 'w',
                                'X', 'x', 'Y', 'y', 'Z', 'z', ' ', ',', '.', '?', '!', '(', ')', ':', '+', '-', '/', '*' };
            String[] alphabetBin = generateAlphabetBin(alphabet);

            String text = "Jebac tego chuja Bartoszka i Jankowska bolszewicka kurwe";
            String[] textByte = generateTextByte(text);
            String[] encryptedTextBin = encrypt(textByte, keyBin);
            String encryptedText = findLetters(encryptedTextBin, alphabetBin, alphabet);
            Console.WriteLine(encryptedText);



            String [] decryptedTextByte = generateTextByte(encryptedText);
            String[] decryptedTextBin = encrypt(decryptedTextByte, keyBin);
            String decryptedText = findLetters(decryptedTextBin, alphabetBin, alphabet);

            Console.Write(decryptedText);
            Console.ReadKey();
        }
    }
}
