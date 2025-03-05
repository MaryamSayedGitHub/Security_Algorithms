using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            string res = null;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < cipherText.Length; i++)
            {
                char cipher = char.ToUpper(cipherText[i]);
                char plain = char.ToUpper(plainText[i]);

                // Get numeric  (0-25)
                int cNum = cipher - 'A';
                int pNum = plain - 'A';
                int kNum = (cNum - pNum + 26) % 26;
                sb.Append((char)('a' + kNum));
            }
            res = sb.ToString();
            res = FindRepeating(sb.ToString());
            return res;

        }
        static string FindRepeating(string key)
        {
            int length = key.Length;
            for (int i = 1; i <= length / 2; i++)
            {
                string pattern = key.Substring(0, i);
                bool isRepeating = true;

                for (int j = 0; j < length; j++)
                {
                    if (key[j] != pattern[j % i])
                    {
                        isRepeating = false;
                        break;
                    }
                }

                if (isRepeating)
                    return pattern;
            }
            return key;
        }

        public string Decrypt(string cipherText, string key)
        {
            string res = null;
            while (key.Length < cipherText.Length)
            {
                key = string.Concat(key, key);
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < cipherText.Length; i++)
            {
                char cipher = char.ToUpper(cipherText[i]);
                char keyChar = char.ToUpper(key[i]);

                // Get numeric (0-25)
                int cNum = cipher - 'A';
                int keyNum = keyChar - 'A';
                int pNum = (cNum - keyNum + 26) % 26;
                sb.Append((char)('a' + pNum));
            }



            res = sb.ToString();
            return res;
        }

        public string Encrypt(string plainText, string key)
        {
            string res = null;
            while (key.Length < plainText.Length)
            {
                key = string.Concat(key, key);
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < plainText.Length; i++)
            {
                char plainChar = char.ToUpper(plainText[i]);
                char keyChar = char.ToUpper(key[i]);

                // Get numeric (0-25)
                int plainNum = plainChar - 'A';
                int keyNum = keyChar - 'A';
                int cipherNum = (plainNum + keyNum) % 26;
                sb.Append((char)('A' + cipherNum));
            }
            res = sb.ToString();
            return res;
        }
    }
}