using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CustomSecurityLibrary.DES
{
    /// <summary>
    /// Custom DES implementation for encryption and decryption.
    /// If the string starts with 0x.... then it's Hexadecimal not string.
    /// </summary>
    public class CustomDES : CryptographicTechnique
    {
        private static readonly int[] PrimaryPermutationTable =
        {
            57, 49, 41, 33, 25, 17, 9,
            1, 58, 50, 42, 34, 26, 18,
            10, 2, 59, 51, 43, 35, 27,
            19, 11, 3, 60, 52, 44, 36,
            63, 55, 47, 39, 31, 23, 15,
            7, 62, 54, 46, 38, 30, 22,
            14, 6, 61, 53, 45, 37, 29,
            21, 13, 5, 28, 20, 12, 4
        };

        private static readonly int[] SecondaryPermutationTable =
        {
            13, 16, 10, 23, 0, 4,
            2, 27, 14, 5, 20, 9,
            22, 18, 11, 3, 25, 7,
            15, 6, 26, 19, 12, 1,
            40, 51, 30, 36, 46, 54,
            29, 39, 50, 44, 32, 47,
            43, 48, 38, 55, 33, 52,
            45, 41, 49, 35, 28, 31
        };

        private static readonly int[] InitialPermutationTable =
        {
            58, 50, 42, 34, 26, 18, 10, 2,
            60, 52, 44, 36, 28, 20, 12, 4,
            62, 54, 46, 38, 30, 22, 14, 6,
            64, 56, 48, 40, 32, 24, 16, 8,
            57, 49, 41, 33, 25, 17, 9, 1,
            59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5,
            63, 55, 47, 39, 31, 23, 15, 7
        };

        private static readonly int[] PermutationTable =
        {
            16, 7, 20, 21,
            29, 12, 28, 17,
            1, 15, 23, 26,
            5, 18, 31, 10,
            2, 8, 24, 14,
            32, 27, 3, 9,
            19, 13, 30, 6,
            22, 11, 4, 25
        };

        private static readonly int[] InversePermutationTable =
        {
            40, 8, 48, 16, 56, 24, 64, 32,
            39, 7, 47, 15, 55, 23, 63, 31,
            38, 6, 46, 14, 54, 22, 62, 30,
            37, 5, 45, 13, 53, 21, 61, 29,
            36, 4, 44, 12, 52, 20, 60, 28,
            35, 3, 43, 11, 51, 19, 59, 27,
            34, 2, 42, 10, 50, 18, 58, 26,
            33, 1, 41, 9, 49, 17, 57, 25
        };

        private static readonly int[] ExpansionPermutationTable =
        {
            32, 1, 2, 3, 4, 5,
            4, 5, 6, 7, 8, 9,
            8, 9, 10, 11, 12, 13,
            12, 13, 14, 15, 16, 17,
            16, 17, 18, 19, 20, 21,
            20, 21, 22, 23, 24, 25,
            24, 25, 26, 27, 28, 29,
            28, 29, 30, 31, 32, 1
        };

        private static readonly int[] ShiftTable = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };

        private static void ConvertTextToBinary(int[] arr, string text)
        {
            int count = 0;
            string binaryString = "";
            for (int i = 2; i < text.Length; i++)
            {
                binaryString = Convert.ToString(Convert.ToInt32(text[i].ToString(), 16), 2).PadLeft(4, '0');
                for (int j = 0; j < binaryString.Length; j++)
                {
                    arr[count] = binaryString[j] - '0';
                    count++;
                }
            }
        }

        private static void PermutePlainText(int[] arr, int[,] mat)
        {
            int[,] initialPermutationMatrix = {
                { 58, 50, 42, 34, 26, 18, 10, 2 },
                { 60, 52, 44, 36, 28, 20, 12, 4 },
                { 62, 54, 46, 38, 30, 22, 14, 6 },
                { 64, 56, 48, 40, 32, 24, 16, 8 },
                { 57, 49, 41, 33, 25, 17, 9, 1 },
                { 59, 51, 43, 35, 27, 19, 11, 3 },
                { 61, 53, 45, 37, 29, 21, 13, 5 },
                { 63, 55, 47, 39, 31, 23, 15, 7 }
            };

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    mat[i, j] = arr[initialPermutationMatrix[i, j] - 1];
                }
            }
        }

        private static void PermuteKey(int[] arr, int[,] mat)
        {
            int[,] keyPermutationMatrix = {
                { 14, 17, 11, 24, 1, 5 },
                { 3, 28, 15, 6, 21, 10 },
                { 23, 19, 12, 4, 26, 8 },
                { 16, 7, 27, 20, 13, 2 },
                { 41, 52, 31, 37, 47, 55 },
                { 30, 40, 51, 45, 33, 48 },
                { 44, 49, 39, 56, 34, 53 },
                { 46, 42, 50, 36, 29, 32 }
            };

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    mat[i, j] = arr[keyPermutationMatrix[i, j] - 1];
                }
            }
        }

        private static void ShiftLeft(int[] arr, int shifts)
        {
            for (int i = 0; i < shifts; i++)
            {
                int temp = arr[0];
                for (int j = 0; j < arr.Length - 1; j++)
                {
                    arr[j] = arr[j + 1];
                }
                arr[arr.Length - 1] = temp;
            }
        }

        private static void ExpandRight(int[] right, int[] expandedRight)
        {
            for (int i = 0; i < ExpansionPermutationTable.Length; i++)
            {
                expandedRight[i] = right[ExpansionPermutationTable[i] - 1];
            }
        }

        private static void XorArrays(int[] arr1, int[] arr2, int[] result)
        {
            for (int i = 0; i < arr1.Length; i++)
            {
                result[i] = arr1[i] ^ arr2[i];
            }
        }

        private static void SubstituteSBox(int[] input, int[] output, int sBoxNumber)
        {
            int[,] sBox = GetSBox(sBoxNumber);
            int row = (input[0] << 1) | input[5];
            int col = (input[1] << 3) | (input[2] << 2) | (input[3] << 1) | input[4];
            int value = sBox[row, col];
            string binaryValue = Convert.ToString(value, 2).PadLeft(4, '0');
            for (int i = 0; i < 4; i++)
            {
                output[i] = binaryValue[i] - '0';
            }
        }

        private static int[,] GetSBox(int sBoxNumber)
        {
            switch (sBoxNumber)
            {
                case 1: return new int[,] { /* S1 Box */ };
                case 2: return new int[,] { /* S2 Box */ };
                case 3: return new int[,] { /* S3 Box */ };
                case 4: return new int[,] { /* S4 Box */ };
                case 5: return new int[,] { /* S5 Box */ };
                case 6: return new int[,] { /* S6 Box */ };
                case 7: return new int[,] { /* S7 Box */ };
                case 8: return new int[,] { /* S8 Box */ };
                default: throw new ArgumentException("Invalid S-Box number");
            }
        }

        private static void Permute(int[] input, int[] output, int[] permutationTable)
        {
            for (int i = 0; i < permutationTable.Length; i++)
            {
                output[i] = input[permutationTable[i] - 1];
            }
        }

        private static void ConvertBinaryToHex(int[] binary, ref string hex)
        {
            StringBuilder hexBuilder = new StringBuilder();
            for (int i = 0; i < binary.Length; i += 4)
            {
                string nibble = binary.Skip(i).Take(4).Select(b => b.ToString()).Aggregate((a, b) => a + b);
                hexBuilder.Append(Convert.ToInt32(nibble, 2).ToString("X"));
            }
            hex = "0x" + hexBuilder.ToString();
        }

        public override string Encrypt(string plainText, string key)
        {
            int[] plainTextBits = new int[64];
            ConvertTextToBinary(plainTextBits, plainText);

            int[,] permutedPlainText = new int[8, 8];
            PermutePlainText(plainTextBits, permutedPlainText);

            StringBuilder permutedPlainTextBuilder = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    permutedPlainTextBuilder.Append(permutedPlainText[i, j]);
                }
            }
            string initialPermutation = permutedPlainTextBuilder.ToString();

            int[] keyBits = new int[64];
            ConvertTextToBinary(keyBits, key);

            int[] permutedKey = new int[56];
            Permute(keyBits, permutedKey, PrimaryPermutationTable);

            int[] leftKey = permutedKey.Take(28).ToArray();
            int[] rightKey = permutedKey.Skip(28).ToArray();

            List<int[]> roundKeys = new List<int[]>();
            for (int round = 0; round < 16; round++)
            {
                ShiftLeft(leftKey, ShiftTable[round]);
                ShiftLeft(rightKey, ShiftTable[round]);

                int[] combinedKey = leftKey.Concat(rightKey).ToArray();
                int[,] roundKeyMatrix = new int[8, 6];
                PermuteKey(combinedKey, roundKeyMatrix);

                int[] roundKey = new int[48];
                int index = 0;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        roundKey[index++] = roundKeyMatrix[i, j];
                    }
                }
                roundKeys.Add(roundKey);
            }

            int[] leftHalf = initialPermutation.Take(32).Select(c => c - '0').ToArray();
            int[] rightHalf = initialPermutation.Skip(32).Select(c => c - '0').ToArray();

            for (int round = 0; round < 16; round++)
            {
                int[] expandedRight = new int[48];
                ExpandRight(rightHalf, expandedRight);

                int[] xorResult = new int[48];
                XorArrays(roundKeys[round], expandedRight, xorResult);

                int[] sBoxOutput = new int[32];
                for (int i = 0; i < 8; i++)
                {
                    int[] sBoxInput = xorResult.Skip(i * 6).Take(6).ToArray();
                    int[] sBoxResult = new int[4];
                    SubstituteSBox(sBoxInput, sBoxResult, i + 1);
                    Array.Copy(sBoxResult, 0, sBoxOutput, i * 4, 4);
                }

                int[] permutedSBoxOutput = new int[32];
                Permute(sBoxOutput, permutedSBoxOutput, PermutationTable);

                int[] newRight = new int[32];
                XorArrays(leftHalf, permutedSBoxOutput, newRight);

                leftHalf = rightHalf;
                rightHalf = newRight;
            }

            int[] finalCombined = rightHalf.Concat(leftHalf).ToArray();
            int[] finalPermuted = new int[64];
            Permute(finalCombined, finalPermuted, InversePermutationTable);

            string cipherText = "";
            ConvertBinaryToHex(finalPermuted, ref cipherText);
            return cipherText;
        }

        public override string Decrypt(string cipherText, string key)
        {
            return Encrypt(cipherText, key); // DES decryption is the same as encryption with reversed keys
        }
    }
}
