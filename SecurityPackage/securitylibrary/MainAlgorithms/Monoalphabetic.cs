   using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        static char[] alphabetic = new char[26] {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
                'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'};

        public string Analyse(string plainText, string cipherText)
        {
            //throw new NotImplementedException();
            string temp = new string(alphabetic).ToUpper();
            List<Tuple<char, char>> plain_cipher_mapping = new List<Tuple<char, char>>();
            var result = new StringBuilder(); bool isFound = false;

            // mapping
            for (int k = 0; k < plainText.Length; k++)
            {
                isFound = plain_cipher_mapping.Exists(t => t.Item1 == char.ToUpper(plainText[k]));
                if (!isFound)
                {
                    plain_cipher_mapping.Add(new Tuple<char, char>(char.ToUpper(plainText[k]), char.ToUpper(cipherText[k])));
                }
            }

            char i = 'A';
            while (i <= 'Z')
            {
                var pair = plain_cipher_mapping.Find(t => t.Item1 == i);
                if (pair == null)
                {
                    for (int j = 0; j < temp.Length; j++)
                    {
                        isFound = plain_cipher_mapping.Exists(t => t.Item2 == temp[j]);
                        if (!isFound)
                        {
                            result.Append(temp[j]);
                            plain_cipher_mapping.Add(new Tuple<char, char>(i, temp[j]));
                            break;
                        }
                    }
                }
                else
                {
                    result.Append(pair.Item2);
                }
                i++;
            }
            return result.ToString().ToLower();
        }

        public string Decrypt(string cipherText, string key)
        {
            string decrypted = MainConcept(cipherText, key);
            return decrypted;
        }

        public string Encrypt(string plainText, string key)
        {
            //throw new NotImplementedException();
            //char[] alphabetic = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            var result = new StringBuilder(); int index;
            var alphabetic = new char[26] {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
                'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'};

            foreach (char character in plainText.ToUpper())
            {
                bool isLetter = char.IsLetter(character);
                if (isLetter)
                {
                    for (int i = 0; i < alphabetic.Length; i++)
                        if (alphabetic[i] == character)
                        {
                            result.Append(key[i]); 
                            break; // stop if found 
                        }
                }
                else
                {
                    result.Append(character);
                }
            } 

            return result.ToString();
        }
        public string MainConcept(string conceptText, string key)
        {
            var result = new StringBuilder();
            bool isLetter = false;
            foreach (char character in conceptText.ToUpper())
            {
                isLetter = char.IsLetter(character);
                if (isLetter)
                    for (int i = 0; i < key.Length; i++)
                    {
                        char key_ch = char.ToUpper(key[i]);
                        if (key_ch == character)
                        {
                            result.Append(alphabetic[i]);
                            break;
                        }
                    }
                else
                    result.Append(character);
            }

            return result.ToString();
        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        public string AnalyseUsingCharFrequency(string cipher)
        {
            throw new NotImplementedException();
        }
    }
}
