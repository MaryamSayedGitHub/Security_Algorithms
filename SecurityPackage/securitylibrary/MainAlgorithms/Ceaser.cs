using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        
        public string Encrypt(string plainText, int key)
        {
            
            string cipherText = String.Empty;
            foreach (var item in plainText.Select(c => new { Letter = c, AlphabetIndex = char.ToUpper(c) - 'A' }))
            {
                int cipherText_char_index = (item.AlphabetIndex + key) % 26; 
                char cipherText_char = (char)('A' + cipherText_char_index); 
                cipherText += cipherText_char;

                Console.WriteLine($"Letter: {item.Letter}, Alphabet Index: {item.AlphabetIndex}, New Index: {cipherText_char_index}, New Letter: {cipherText_char}");
            }

            Console.WriteLine($"Cipher Text: {cipherText}");
            return cipherText;

        }

        public string Decrypt(string cipherText, int key)
        {
            string plainText = String.Empty;

            foreach (var item in cipherText.Select(c => new { Letter = c, AlphabetIndex = char.ToUpper(c) - 'A' }))
            {
                int plainText_char_index = (item.AlphabetIndex - key + 26) % 26;
                char plainText_char = (char)('A' + plainText_char_index);

                plainText += plainText_char;  

                Console.WriteLine($"Letter: {item.Letter}, Alphabet Index: {item.AlphabetIndex}, New Index: {plainText_char_index}, New Letter: {plainText_char}");
            }

            Console.WriteLine($"Decrypted Text: {plainText}");
            return plainText;
        }

        public int Analyse(string plainText, string cipherText)
        {
            int key = (char.ToUpper(cipherText[0]) - char.ToUpper(plainText[0]) + 26) % 26;

            Console.WriteLine($"The Key is: {key}");
            return key;
        }
    }
}
