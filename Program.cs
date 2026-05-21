using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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

        Console.WriteLine("save secret key ? (y/n)");
        string saveSecretKey = Console.ReadLine();
        if(saveSecretKey.ToLower() == "y")
        {
            Console.WriteLine("Enter a name for the secret key:");
            string keyName = Console.ReadLine();
            if(!string.IsNullOrWhiteSpace(keyName))
            {
                File.WriteAllText($"{keyName}.txt", string.Join(", ", encodedSecretKey));
                Console.WriteLine($"Secret key saved as {keyName}.txt");
            }
            else
            {
                Console.WriteLine("Invalid key name provided.");
            }
        }

        Console.WriteLine("Enter S-Box Generation Seed");
        string seed = Console.ReadLine();
        byte[] sBox = sBoxGenerator.GenerateSBox(seed);

        Console.WriteLine("save S-Box ? (y/n)");
        string saveSBox = Console.ReadLine();
        if(saveSBox.ToLower() == "y")
        {
            Console.WriteLine("Enter a name for the S-Box:");
            string sBoxName = Console.ReadLine();
            if(!string.IsNullOrWhiteSpace(sBoxName))
            {
                File.WriteAllText($"{sBoxName}.txt", string.Join(", ", sBox));
                Console.WriteLine($"S-Box saved as {sBoxName}.txt");
            }
            else
            {
                Console.WriteLine("Invalid S-Box name provided.");
            }
        }

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
        byte[] encryptedBytes = Convert.FromBase64String(userInput);

        Console.WriteLine("Use previous secret key ? (y/n)");
        string usePreviousKey = Console.ReadLine();

        ulong[] encodedSecretKey;
        if (usePreviousKey.ToLower() == "y")
        {
            secretKeySelect:
            Console.WriteLine("Enter the name of the secret key:");
            string keyFileName = Console.ReadLine();
            if (File.Exists($"{keyFileName}.txt"))
            {
                string keyValues = System.IO.File.ReadAllText($"{keyFileName}.txt");
                encodedSecretKey = textNumberConverter.ConvertToNumber(keyValues);
                
            }
            else
            {
                Console.WriteLine("File not found.");
                goto secretKeySelect;
            }
        }
        else
        {
            Console.WriteLine("Enter the secret key:");
            string secretKey = Console.ReadLine();
            encodedSecretKey = textNumberConverter.ConvertToNumber(secretKey);
        }
        Console.WriteLine("Use Previously generated S-Box ? (y/n)");
        string usePreviousSBox = Console.ReadLine(); 
        byte[] inverseSBox;  
        if (usePreviousSBox.ToLower() == "y")
        {
            sBoxSelect:
            Console.WriteLine("Enter the name of the S-Box:");
            string sBoxFileName = Console.ReadLine();
            if (File.Exists($"{sBoxFileName}.txt"))
            {
                string sBoxValues = System.IO.File.ReadAllText($"{sBoxFileName}.txt");
                byte[] sBox = sBoxValues.Split(',').Select(byte.Parse).ToArray();
                inverseSBox = sBoxGenerator.GenerateInverseSBox(sBox);
            }
            else
            {
                Console.WriteLine("File not found.");
                goto sBoxSelect;
            }
        }
        else
        {
            Console.WriteLine("Enter S-Box Generation Seed");
            string seed = Console.ReadLine();
            byte[] sBox = sBoxGenerator.GenerateSBox(seed);
            inverseSBox = sBoxGenerator.GenerateInverseSBox(sBox);
        }

        ulong[] decryptedMessage = symmetricEncryption.Decrypt(encryptedBytes, encodedSecretKey, inverseSBox);
        Console.WriteLine("Decrypted numbers: " + string.Join(", ", decryptedMessage));
        string decryptedString = textNumberConverter.ConvertToText(decryptedMessage);
        Console.WriteLine("Decrypted string: " + decryptedString);

    }
}