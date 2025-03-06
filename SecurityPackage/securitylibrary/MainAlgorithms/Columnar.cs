using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public List<int> Analyse(string plainText, string cipherText)
        {
            List<int> keyList = new List<int>();
            plainText = plainText.ToUpper();
            cipherText = cipherText.ToUpper();
            int counter, count = 0;
            int rows = 0, cols = 0;
            bool equals = false;

            for (int key = 1; key < plainText.Length; key++)
            {
                List<List<char>> matrix = new List<List<char>>();
                keyList.Add(key);
                int noOfColumns = keyList.Count;
                int noOfRows = (plainText.Length + noOfColumns - 1) / noOfColumns;

                count = 0; // reset
                for (int r = 0; r < noOfRows; r++)
                {
                    List<char> each_row = new List<char>();
                    for (int c = 0; c < noOfColumns; c++)
                    {
                        each_row.Add(count < plainText.Length ? plainText[count++] : '\0');
                    }
                    matrix.Add(each_row);
                }

                rows = matrix.Count;
                cols = matrix[0].Count;

                for (int c = 0; c < cols; c++)
                {
                    int rows_num = 0;
                    for (int r = 0; r < rows; r++)
                    {
                        if (matrix[r][c] == cipherText[rows_num])
                            rows_num++;
                    }

                    if (rows_num == rows)
                    {
                        equals = true; break;
                    }
                }

                if (equals) break;
            }

            if ((rows * cols) > cipherText.Length)
            {
                return keyList;
            }

            List<List<char>> matrix2 = new List<List<char>>();
            count = 0;
            for (int r = 0; r < rows; r++)
            {
                List<char> each_row = new List<char>();
                for (int c = 0; c < cols; c++)
                {
                    each_row.Add(count < plainText.Length ? plainText[count++] : '\0');
                }
                matrix2.Add(each_row);
            }

            for (int c = 0; c < cols; c++)
            {
                equals = true;
                int iterator = 0;
                counter = 0;
                while (equals)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        counter = matrix2[r][c] == cipherText[iterator] ? counter + 1 : 0;
                        iterator++;
                        if (counter == rows)
                        {
                            equals = false; break;
                        }
                    }
                }
                keyList[c] = ((iterator + 1) / rows);
            }
            return keyList.ToList();
        }

        public string Decrypt(string cipherText, List<int> key)
        {
            List<List<char>> matrix = new List<List<char>>();
            StringBuilder plainText = new StringBuilder();

            int numOfColumns = key.Max();
            int numOfRows = (cipherText.Length + numOfColumns - 1) / numOfColumns;
            int length = 0;

            for (int row = 0; row < numOfRows; row++)
            {
                var new_row = new List<char>(new char[numOfColumns]);
                matrix.Add(new_row);
            }

            for (int col = 1; col <= numOfColumns; col++)
            {
                int index = key.IndexOf(col);
                for (int row = 0; row < numOfRows; row++)
                {
                    if (length < cipherText.Length)
                    {
                        matrix[row][index] = cipherText[length++];
                    }
                }
            }

            foreach (var row in matrix)
            {
                plainText.Append(new string(row.ToArray()));
            }

            return plainText.ToString().TrimEnd('x'); 
        }

        public string Encrypt(string plainText, List<int> key)
        {
            List<List<char>> matrix = new List<List<char>>();
            var cipherText = new StringBuilder();
            int noOfColumns = key.Max();
            int noOfRows = (plainText.Length + noOfColumns - 1) / noOfColumns;
            int i = 0;

            for (int row = 0; row < noOfRows; row++)
            {
                List<char> each_row = new List<char>();
                for (int col = 0; col < noOfColumns; col++)
                {
                    each_row.Add(i < plainText.Length ? plainText[i++] : 'x');
                }
                matrix.Add(each_row);
            }

            for (int c = 1; c <= key.Count(); c++)
            {
                i = key.IndexOf(c);
                for (int r = 0; r < noOfRows; r++)
                {
                    cipherText.Append(matrix[r][i]);
                }
            }

            return cipherText.ToString();
        }
    }
}
