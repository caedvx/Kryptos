using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter a string to encode");
        string userInput = Console.ReadLine();
        TextNumberConverter textNumberConverter = new TextNumberConverter();
        ulong[] encodedArray = textNumberConverter.ConvertToNumber(userInput);
        Console.WriteLine("Encoded numbers: " + string.Join(", ", encodedArray));
        Console.WriteLine("Enter a Secret Key:");
        string secretKey = Console.ReadLine();
        ulong[] encodedSecretKey = textNumberConverter.ConvertToNumber(secretKey);
        SymmetricEncryption symmetricEncryption = new SymmetricEncryption();
        ulong[] encryptedMessage = symmetricEncryption.Encrypt(encodedArray, encodedSecretKey);
        string decodedString = textNumberConverter.ConvertToText(encodedArray);
        Console.WriteLine("Decoded string: " + decodedString);
    }  
}