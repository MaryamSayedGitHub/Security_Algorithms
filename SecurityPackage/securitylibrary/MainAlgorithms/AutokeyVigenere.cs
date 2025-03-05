using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class AutokeyVigenere : ICryptographicTechnique<string, string>
    {
        // Analyses the plaintext and ciphertext to determine the key
        public string Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToUpper();
            cipherText = cipherText.ToUpper();

            char[] keyChars = new char[plainText.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                // Calculate  key = (cipher - plain) mod 26
                int keyIndex = (cipherText[i] - plainText[i] + 26) % 26;
                keyChars[i] = (char)(keyIndex + 'A');
            }


            string fullKey = new string(keyChars);

            for (int keyLength = 1; keyLength < plainText.Length; keyLength++)
            {
                bool isAutokey = true;

                for (int i = 0; i < plainText.Length - keyLength; i++)
                {
                    if (keyChars[keyLength + i] != plainText[i])
                    {
                        isAutokey = false;
                        break;
                    }
                }

                if (isAutokey)
                {
                    return fullKey.Substring(0, keyLength);
                }
            }

            return fullKey; // If no pattern is found, return the entire calculated key
        }

        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToUpper();
            key = key.ToUpper();

            StringBuilder plainText = new StringBuilder();

            // Decrypt the first characters using the provided key
            for (int i = 0; i < Math.Min(key.Length, cipherText.Length); i++)
            {
                // Calculate  plain = (cipher - key) mod 26
                int plainIndex = (cipherText[i] - key[i] + 26) % 26;
                char plainChar = (char)(plainIndex + 'A');
                plainText.Append(plainChar);
            }

            // For the remaining characters, use the previously decrypted plaintext as the key
            for (int i = key.Length; i < cipherText.Length; i++)
            {
                int keyChar = plainText[i - key.Length];

                int plainIndex = (cipherText[i] - keyChar + 26) % 26;
                char plainChar = (char)(plainIndex + 'A');
                plainText.Append(plainChar);
            }

            return plainText.ToString();
        }
        public string Encrypt(string plainText, string key)
        {
            plainText = plainText.ToUpper();
            key = key.ToUpper();

            StringBuilder cipherText = new StringBuilder();
            StringBuilder fullKey = new StringBuilder(key);

            for (int i = 0; i < plainText.Length; i++)
            {
                if (i >= key.Length)
                {
                    fullKey.Append(plainText[i - key.Length]);
                }
            }

            for (int i = 0; i < plainText.Length; i++)
            {
                int cipherIndex = (plainText[i] - 'A' + fullKey[i] - 'A') % 26; //converted into a numeric 0 - 25
                char cipherChar = (char)(cipherIndex + 'A'); //converted back into a character
                cipherText.Append(cipherChar);
            }
            return cipherText.ToString();
        }
    }
}