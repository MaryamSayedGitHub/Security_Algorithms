using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        private char[,] matrix; // 5x5 
        private Dictionary<char, (int, int)> Table;
        private void GenerateMatrix(string key)
        {
            key = key.ToUpper().Replace("J", "I"); // (J -> I)
            string alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
            HashSet<char> usedChars = new HashSet<char>();
            List<char> unique = new List<char>();

            // Construct unique sequence using key and remaining alphabet
            foreach (char c in key + alphabet)
            {
                if (!usedChars.Contains(c)) // Avoid duplicates
                {
                    usedChars.Add(c);
                    unique.Add(c);
                }
            }

            matrix = new char[5, 5];
            Table = new Dictionary<char, (int, int)>();

            int counter = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    matrix[i, j] = unique[counter]; // Fill matrix with unique 
                    Table[matrix[i, j]] = (i, j);
                    counter++;
                }
            }
        }
        /// Splits the input text into pairs following Playfair rules.
        /// - Converts to uppercase and replaces 'J' with 'I'.
        /// - Removes non-letter characters.
        /// - Inserts 'X' between repeating letters and at the end if needed.
        private List<string> GeneratePairs(string input)
        {
            input = input.ToUpper().Replace("J", "I"); // Normalize text
            List<string> pairs = new List<string>();
            StringBuilder cleanText = new StringBuilder();

            // Remove non-letter characters
            foreach (char c in input)
            {
                if (char.IsLetter(c))
                    cleanText.Append(c);
            }

            // Generate letter pairs while handling duplicate letters
            for (int i = 0; i < cleanText.Length; i++)
            {
                char first = cleanText[i];
                char second = (i + 1 < cleanText.Length) ? cleanText[i + 1] : 'X';

                if (first == second)
                {
                    pairs.Add(first.ToString() + 'X');
                }
                else
                {
                    pairs.Add(first.ToString() + second);
                    i++; // Skip next letter , it's already paired
                }
            }

            // Ensure the last pair has two letters 
            if (pairs[pairs.Count - 1].Length == 1)
                pairs[pairs.Count - 1] += 'X';

            return pairs;
        }

        /// - If both letters are in the same row, shift right (encryption) or left (decryption).
        /// - If both letters are in the same column, shift down (encryption) or up (decryption).
        /// - Otherwise, swap column positions to form a rectangle.

        private string pairsRules(List<string> digraphs, bool encrypt)
        {
            StringBuilder output = new StringBuilder();
            int shift = encrypt ? 1 : -1; // Shift right for encryption, left for decryption

            foreach (var pair in digraphs)
            {
                var (row1, col1) = Table[pair[0]];
                var (row2, col2) = Table[pair[1]];
                if (row1 == row2) // Same row: Shift right (encryption) or left (decryption)
                {
                    output.Append(matrix[row1, (col1 + shift + 5) % 5]);
                    output.Append(matrix[row2, (col2 + shift + 5) % 5]);
                }
                else if (col1 == col2) // Same column: Shift down (encryption) or up (decryption)
                {
                    output.Append(matrix[(row1 + shift + 5) % 5, col1]);
                    output.Append(matrix[(row2 + shift + 5) % 5, col2]);
                }
                else
                {
                    output.Append(matrix[row1, col2]);
                    output.Append(matrix[row2, col1]);
                }
            }

            return output.ToString();
        }
        public string Encrypt(string plainText, string key)
        {

            GenerateMatrix(key);
            var digraphs = GeneratePairs(plainText); // Prepare text into letter pairs
            return pairsRules(digraphs, true); // Encrypt 
        }

        public string Decrypt(string cipherText, string key)
        {
            // throw new NotImplementedException();

            GenerateMatrix(key);

            //  (remove non-letters and replace 'J' with 'I')
            StringBuilder cleanText = new StringBuilder();
            foreach (char c in cipherText.ToUpper())
            {
                if (char.IsLetter(c))
                    cleanText.Append(c == 'J' ? 'I' : c);
            }

            // Generate (letter pairs)
            List<string> pairs = new List<string>();
            for (int i = 0; i < cleanText.Length; i += 2)
            {
                pairs.Add(cleanText[i].ToString() + (i + 1 < cleanText.Length ? cleanText[i + 1] : 'X'));
            }

            string decrypted = pairsRules(pairs, false); // Decrypt the pairs

            // Remove padding 'X' characters (used for encryption adjustments)
            StringBuilder result = new StringBuilder(decrypted);

            // Remove 'X' if it was inserted between duplicate letters
            for (int i = 1; i < result.Length - 1; i += 2)
            {
                if (result[i] == 'X' && result[i - 1] == result[i + 1])
                {
                    result.Remove(i, 1);
                    i--; // Adjust index after removal
                }
            }

            // Remove trailing 'X' if present
            if (result.Length > 0 && result[result.Length - 1] == 'X')
            {
                result.Length--;
            }

            return result.ToString();
        }
    }

}
