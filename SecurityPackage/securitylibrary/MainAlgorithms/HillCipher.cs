using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher : ICryptographicTechnique<List<int>, List<int>>
    {
        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            List<int> plain2 = new List<int>();
            List<int> res = new List<int>();
            //int a=0, z=3, c=1, d=4;
            //int a=0, z=3, c=2, d = 5;
            int a = 0, z = 1, c = 4, d = 5;

            if (plainText.Count > 4)
            {
                int det = (plainText[a] * plainText[d]) - (plainText[z] * plainText[c]);
                Console.WriteLine(det);
                det = det % 26;
                while (det < 0) det += 26;
                Console.WriteLine(det);
                int b = 0;
                while ((det == 0) || (det % 2 == 0))
                {
                    if (d == 11) throw new InvalidAnlysisException();
                    det = (plainText[(++a) % plainText.Count] * plainText[(++d) % plainText.Count]) - (plainText[(++z) % plainText.Count] * plainText[(++c) % plainText.Count]);
                    while (det < 0) det += 26;
                    det = det % 26;

                }

                for (int x = 1; x < 26; x++)
                {
                    if ((det * x) % 26 == 1)
                    {
                        b = x;
                        break;
                    }
                }
                if (b == 0)
                {
                    throw new NotImplementedException();
                }

                plain2.Add((b * plainText[d]) % 26);
                int val = (b * plainText[z] * -1) % 26;
                while (val < 0) val += 26;
                plain2.Add(val);
                val = (b * plainText[c] * -1) % 26;
                while (val < 0) val += 26;
                plain2.Add(val);
                plain2.Add((b * plainText[a]) % 26);
            }
            else
            {
                int det = (plainText[0] * plainText[3]) - (plainText[1] * plainText[2]);
                Console.WriteLine(det);
                det = det % 26;
                if (det < 0) det += 26;
                Console.WriteLine(det);
                int b = 0;
                if ((det != 0) && (det % 2 == 1))
                {
                    for (int x = 1; x < 26; x++)
                    {
                        if ((det * x) % 26 == 1)
                        {
                            b = x;
                            break;
                        }
                    }
                    if (b == 0)
                    {
                        throw new InvalidAnlysisException();
                    }

                    plain2.Add((b * plainText[d]) % 26);
                    int val = (b * plainText[z] * -1) % 26;
                    while (val < 0) val += 26;
                    plain2.Add(val);
                    val = (b * plainText[c] * -1) % 26;
                    while (val < 0) val += 26;
                    plain2.Add(val);
                    plain2.Add((b * plainText[a]) % 26);
                }
                else
                {
                    throw new InvalidAnlysisException();

                }
            }

            //res.Add(((cipherText[a] * plain2[0]) + (cipherText[z] * plain2[2]))%26);
            //res.Add(((cipherText[a] * plain2[1]) + (cipherText[z] * plain2[3]))%26);
            //res.Add(((cipherText[c] * plain2[0]) + (cipherText[d] * plain2[2]))%26);
            //res.Add(((cipherText[c] * plain2[1]) + (cipherText[d] * plain2[3]))%26);

            res.Add(((cipherText[a] * plain2[0]) + (cipherText[c] * plain2[1])) % 26);
            res.Add(((cipherText[a] * plain2[2]) + (cipherText[c] * plain2[3])) % 26);
            res.Add(((cipherText[z] * plain2[0]) + (cipherText[d] * plain2[1])) % 26);
            res.Add(((cipherText[z] * plain2[2]) + (cipherText[d] * plain2[3])) % 26);
            return res;
        }


        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            int div = (int)Math.Sqrt(key.Count);
            List<int> res = new List<int>();
            List<int> key2 = new List<int>();

            if (div == 2)
            {
                int det = (key[0] * key[3]) - (key[1] * key[2]);
                Console.WriteLine(det);
                det = det % 26;
                if (det < 0) det += 26;
                Console.WriteLine(det);
                int b = 0;
                if ((det != 0) && (det % 2 == 1))
                {
                    for (int x = 1; x < 26; x++)
                    {
                        if ((det * x) % 26 == 1)
                        {
                            b = x;
                            break;
                        }
                    }
                    if (b == 0)
                    {
                        throw new NotImplementedException();
                    }

                    key2.Add((b * key[3]) % 26);
                    key2.Add((b * key[1] * -1) % 26);
                    key2.Add((b * key[2] * -1) % 26);
                    key2.Add((b * key[0]) % 26);

                    for (int n = 0; n < cipherText.Count;)
                    {
                        for (int i = 0; i < key2.Count;)
                        {
                            int r = 0;
                            for (int j = 0; j < div; j++)
                            {
                                r += key2[i + j] * cipherText[n + j];

                            }
                            while (r < 0) r += 26;
                            res.Add(r % 26);
                            i += div;
                        }
                        n += div;
                    }
                    return res;
                }
            }
            else//matrix 3*3
            {
                int det = (key[0] * (key[4] * key[8] - key[5] * key[7]))
                    - (key[1] * (key[3] * key[8] - key[5] * key[6]))
                    + (key[2] * (key[3] * key[7] - key[4] * key[6]));
                Console.WriteLine(det);
                det = det % 26;
                while (det < 0) det += 26;
                Console.WriteLine(det);
                int b = 0;
                if ((det != 0) && (det % 2 == 1))
                {
                    for (int x = 1; x < 26; x++)
                    {
                        if ((det * x) % 26 == 1)
                        {
                            b = x;
                            break;
                        }
                    }
                    if (b == 0)
                    {
                        throw new NotImplementedException();
                    }
                }
                //0
                int key_val = b * (key[4] * key[8] - key[5] * key[7]);
                while (key_val < 0) key_val += 26;
                key2.Add(key_val % 26);
                //3
                key_val = -1 * b * (key[1] * key[8] - key[2] * key[7]);
                while (key_val < 0) key_val += 26;
                key2.Add(key_val % 26);
                //6
                key_val = b * (key[1] * key[5] - key[2] * key[4]);
                while (key_val < 0) key_val += 26;
                key2.Add(key_val % 26);
                //1
                key_val = -1 * b * (key[3] * key[8] - key[5] * key[6]);
                while (key_val < 0) key_val += 26;
                key2.Add(key_val % 26);
                //4
                key_val = b * (key[0] * key[8] - key[2] * key[6]);
                while (key_val < 0) key_val += 26;
                key2.Add(key_val % 26);
                //7
                key_val = -1 * b * (key[0] * key[5] - key[2] * key[3]);
                while (key_val < 0) key_val += 26;
                key2.Add(key_val % 26);
                //2
                key_val = b * (key[3] * key[7] - key[4] * key[6]);
                while (key_val < 0) key_val += 26;
                key2.Add(key_val % 26);
                //5
                key_val = -1 * b * (key[0] * key[7] - key[1] * key[6]);
                while (key_val < 0) key_val += 26;
                key2.Add(key_val % 26);
                //8
                key_val = b * (key[0] * key[4] - key[1] * key[3]);
                while (key_val < 0) key_val += 26;
                key2.Add(key_val % 26);

                for (int n = 0; n < cipherText.Count;)
                {
                    for (int i = 0; i < key2.Count;)
                    {
                        int r = 0;
                        for (int j = 0; j < div; j++)
                        {
                            r += key2[i + j] * cipherText[n + j];

                        }
                        while (r < 0) r += 26;
                        res.Add(r % 26);
                        i += div;
                    }
                    n += div;
                }
                return res;
            }

            throw new NotImplementedException();
        }


        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            List<int> res = new List<int>();

            int div = (int)Math.Sqrt(key.Count);

            for (int n = 0; n < plainText.Count;)
            {
                for (int i = 0; i < key.Count;)
                {
                    int r = 0;
                    for (int j = 0; j < div; j++)
                    {
                        r += key[i + j] * plainText[n + j];

                    }
                    res.Add(r % 26);
                    i += div;
                }
                n += div;
            }
            return res;
            //throw new NotImplementedException();
        }


        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            List<int> res = new List<int>();
            List<int> plain2 = new List<int>();

            int det = (plainText[0] * (plainText[4] * plainText[8] - plainText[5] * plainText[7]))
                    - (plainText[1] * (plainText[3] * plainText[8] - plainText[5] * plainText[6]))
                    + (plainText[2] * (plainText[3] * plainText[7] - plainText[4] * plainText[6]));
            Console.WriteLine(det);
            det = det % 26;
            while (det < 0) det += 26;
            Console.WriteLine(det);

            int b = 0;
            if ((det != 0) && (det % 2 == 1))
            {
                for (int x = 1; x < 26; x++)
                {
                    if ((det * x) % 26 == 1)
                    {
                        b = x;
                        break;
                    }
                }
                if (b == 0)
                {
                    throw new NotImplementedException();
                }
            }
            //0
            int plain_val = b * (plainText[4] * plainText[8] - plainText[5] * plainText[7]);
            while (plain_val < 0) plain_val += 26;
            plain2.Add(plain_val % 26);
            //3
            plain_val = -1 * b * (plainText[1] * plainText[8] - plainText[2] * plainText[7]);
            while (plain_val < 0) plain_val += 26;
            plain2.Add(plain_val % 26);
            //6
            plain_val = b * (plainText[1] * plainText[5] - plainText[2] * plainText[4]);
            while (plain_val < 0) plain_val += 26;
            plain2.Add(plain_val % 26);
            //1
            plain_val = -1 * b * (plainText[3] * plainText[8] - plainText[5] * plainText[6]);
            while (plain_val < 0) plain_val += 26;
            plain2.Add(plain_val % 26);
            //4
            plain_val = b * (plainText[0] * plainText[8] - plainText[2] * plainText[6]);
            while (plain_val < 0) plain_val += 26;
            plain2.Add(plain_val % 26);
            //7
            plain_val = -1 * b * (plainText[0] * plainText[5] - plainText[2] * plainText[3]);
            while (plain_val < 0) plain_val += 26;
            plain2.Add(plain_val % 26);
            //2
            plain_val = b * (plainText[3] * plainText[7] - plainText[4] * plainText[6]);
            while (plain_val < 0) plain_val += 26;
            plain2.Add(plain_val % 26);
            //5
            plain_val = -1 * b * (plainText[0] * plainText[7] - plainText[1] * plainText[6]);
            while (plain_val < 0) plain_val += 26;
            plain2.Add(plain_val % 26);
            //8
            plain_val = b * (plainText[0] * plainText[4] - plainText[1] * plainText[3]);
            while (plain_val < 0) plain_val += 26;
            plain2.Add(plain_val % 26);

            int result1 = cipherText[0] * plain2[0] + cipherText[3] * plain2[1] + cipherText[6] * plain2[2];
            res.Add(result1 % 26);
            result1 = cipherText[0] * plain2[3] + cipherText[3] * plain2[4] + cipherText[6] * plain2[5];
            res.Add(result1 % 26);
            result1 = cipherText[0] * plain2[6] + cipherText[3] * plain2[7] + cipherText[6] * plain2[8];
            res.Add(result1 % 26);
            result1 = cipherText[1] * plain2[0] + cipherText[4] * plain2[1] + cipherText[7] * plain2[2];
            res.Add(result1 % 26);
            result1 = cipherText[1] * plain2[3] + cipherText[4] * plain2[4] + cipherText[7] * plain2[5];
            res.Add(result1 % 26);
            result1 = cipherText[1] * plain2[6] + cipherText[4] * plain2[7] + cipherText[7] * plain2[8];
            res.Add(result1 % 26);
            result1 = cipherText[2] * plain2[0] + cipherText[5] * plain2[1] + cipherText[8] * plain2[2];
            res.Add(result1 % 26);
            result1 = cipherText[2] * plain2[3] + cipherText[5] * plain2[4] + cipherText[8] * plain2[5];
            res.Add(result1 % 26);
            result1 = cipherText[2] * plain2[6] + cipherText[5] * plain2[7] + cipherText[8] * plain2[8];
            res.Add(result1 % 26);
            return res;

            throw new NotImplementedException();
        }

    }
}
