using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Xml.Serialization;

class Program
{
    static TextNumberConverter textNumberConverter = new TextNumberConverter();
    static SymmetricEncryption symmetricEncryption = new SymmetricEncryption();
    static SBoxGenerator sBoxGenerator = new SBoxGenerator();
    static NumberByteChunker numberByteChunker = new NumberByteChunker();
    static void Main()
    {
        Console.WriteLine($"choose a Cryptography Method:\n 1. Symmetric Encryption \n 2. Symmetric Decryption\n 3. Asymmetric Encryption \n 4. Asymmetric Decryption");
        int choice = int.Parse(Console.ReadLine());
        switch (choice)
        {
            case 1:
                SymmetricEncrypt();
                break;
            case 2:
                SymmetricDecrypt();
                break;
            case 3:
                AsymmetricEncrypt();
                break;
            // case 4:
            //     AsymmetricDecrypt();
            //     break;
            default:
                Console.WriteLine("Invalid choice");
                break;
        }
    }
    static void AsymmetricEncrypt()
    {
        Console.WriteLine("Not yet implemented");
        Main();
        // ulong[] encryptedMessage = symmetricEncryption.Encrypt(encodedArray, encodedSecretKey, sBox);
        // string decodedString = textNumberConverter.ConvertToText(encodedArray);
        // Console.WriteLSine("Decoded string: " + decodedString);
    }  
    static void SymmetricEncrypt()
    {
        Console.WriteLine("Enter a string to encode");
        string userInput = Console.ReadLine();
        
        ulong[] encodedArray = textNumberConverter.ConvertToNumber(userInput);
        Console.WriteLine("Encoded numbers: " + string.Join(", ", encodedArray));

        Console.WriteLine("Enter a Secret Key:");
        string secretKey = Console.ReadLine();
        ulong[] encodedSecretKey = textNumberConverter.ConvertToNumber(secretKey);

        Console.WriteLine("Enter S-Box Generation Seed");
        string seed = Console.ReadLine();
        byte[] sBox = sBoxGenerator.GenerateSBox(seed);

        byte[] encryptedBytes = symmetricEncryption.Encrypt(encodedArray, encodedSecretKey, sBox);
        ulong[] encryptedNumbers = numberByteChunker.Chunk(encryptedBytes);
        Console.WriteLine("Encrypted numbers: " + string.Join(", ", encryptedNumbers));
        Console.WriteLine("Encrypted bytes: " + string.Join(", ", encryptedBytes));

        Console.WriteLine("Encrypted Message: " + Convert.ToBase64String(encryptedBytes));
    }
    static void SymmetricDecrypt()
    {
        Console.WriteLine("Enter a string to decode");
        string userInput = Console.ReadLine();
    }
}