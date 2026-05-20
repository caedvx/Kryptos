using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;

class TextNumberConverter
{
    public ulong[] ConvertToNumber(string text)
    {
        int n = text.Length;
        char[] textArray = text.ToCharArray();
        ulong[] numberArray = Array.ConvertAll(textArray, c => (ulong)c);
        ulong[] result = new ulong[(n + 7) / 8]; // Calculate the size of the result array
        for (int i = 0; i < n; i++)
        {
            if(i%8 == 0)
            {
                result[i/8] = numberArray[i];
            }
            else
            {
                result[i/8] += numberArray[i]*(ulong)Math.Pow(256, i%8);
            }
        }

        return result;
    }
    public string ConvertToText(ulong[] numbers)
    {
        int n = numbers.Length;
        char[] textArray = new char[n * 8];
        for(int i = 0; i < n * 8; i++)
        {
          if (i%8 == 0)
            {
                textArray[i] = (char)(numbers[i/8] % 256);
            }
            else
            {
                textArray[i] += (char)((numbers[i/8] / (ulong)Math.Pow(256, i%8)) % 256);
            }
        }
        return new string(textArray);
    }
}