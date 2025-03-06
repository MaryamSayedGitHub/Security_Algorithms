using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)

        {
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            cipherText = cipherText.Replace("x", "");

            int n = 2;

            while (true)
            {
                string encrypted = Encrypt(plainText, n);
                if (string.Equals(encrypted, cipherText)) break;
                else
                    n++;
            }

            return n;
        }

        public string Decrypt(string cipherText, int n)
        {
            int c = Convert.ToInt32(Math.Ceiling((double)cipherText.Length / (double)n));

            char[,] matrix = new char[n, c];

            int counter = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < c; j++)
                {

                    if (counter < cipherText.Length)
                    {

                        matrix[i, j] = cipherText[counter++];
                    }

                    else break;

                }
            }

            string decryption = null;

            for (int i = 0; i < c; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (matrix[j, i] != '\0')
                        decryption += matrix[j, i];
                }
            }

            return decryption;
        }

        public string Encrypt(string plainText, int n)
        {
            plainText = plainText.Trim();
            int c = Convert.ToInt32(Math.Ceiling((double)plainText.Length / (double)n));

            Console.WriteLine(c);

            char[,] matrix = new char[n, c];
            Console.WriteLine(matrix.Length);
            int counter = 0;


            for (int i = 0; i < c; i++)
            {
                for (int j = 0; j < n; j++)
                {

                    if (counter < plainText.Length)
                    {

                        matrix[j, i] = plainText[counter++];
                    }


                }
            }
            string encryption = null;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    if (matrix[i, j] != '\0')
                        encryption += matrix[i, j];
                }
            }

            return encryption.Trim();
        }
        

    }
}
