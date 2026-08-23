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
        if (!int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine("Invalid choice");
            return;
        }
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
    // Console.ReadLine() returns null at end of input (piped input, Ctrl+Z), which
    // used to crash on the ToLower() calls below. Treat it as empty instead.
    static string Prompt(string message)
    {
        Console.WriteLine(message);
        return Console.ReadLine() ?? "";
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
        string userInput = Prompt("Enter a string to encode");
        
        ulong[] encodedArray = textNumberConverter.ConvertToNumber(userInput);
        Console.WriteLine("Encoded numbers: " + string.Join(", ", encodedArray));

        string usePreviousKey = Prompt("Use previous secret key ? (y/n)");

        ulong[] encodedSecretKey;
        string saveSecretKey;
        if (usePreviousKey.ToLower() == "y")
        {
            secretKeySelect:
            string keyFileName = Prompt("Enter the name of the secret key:");
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
            string secretKey = Prompt("Enter a Secret Key:");
            encodedSecretKey = textNumberConverter.ConvertToNumber(secretKey);

            saveSecretKey = Prompt("save secret key ? (y/n)");
            
            if(saveSecretKey.ToLower() == "y")
            {
                string keyName = Prompt("Enter a name for the secret key:");
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
        }

        string usePreviousSBox = Prompt("Use Previously generated S-Box ? (y/n)");
        byte[] inverseSBox;  
        byte[] sBox;
        if (usePreviousSBox.ToLower() == "y")
        {
            sBoxSelect:
            string sBoxFileName = Prompt("Enter the name of the S-Box:");
            if (File.Exists($"{sBoxFileName}.txt"))
            {
                string sBoxValues = System.IO.File.ReadAllText($"{sBoxFileName}.txt");
                sBox = sBoxValues.Split(',').Select(byte.Parse).ToArray();
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
            string seed = Prompt("Enter S-Box Generation Seed");
            sBox = sBoxGenerator.GenerateSBox(seed);

            string saveSBox = Prompt("save S-Box ? (y/n)");
            if(saveSBox.ToLower() == "y")
            {
                string sBoxName = Prompt("Enter a name for the S-Box:");
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
        }
        

        byte[] encryptedBytes = symmetricEncryption.Encrypt(encodedArray, encodedSecretKey, sBox);
        ulong[] encryptedNumbers = numberByteChunker.Chunk(encryptedBytes);
        Console.WriteLine("Encrypted numbers: " + string.Join(", ", encryptedNumbers));
        Console.WriteLine("Encrypted bytes: " + string.Join(", ", encryptedBytes));

        Console.WriteLine("Encrypted Message: " + Convert.ToBase64String(encryptedBytes));
    }
    static void SymmetricDecrypt()
    {
        string userInput = Prompt("Enter a string to decode");
        byte[] encryptedBytes = Convert.FromBase64String(userInput);

        string usePreviousKey = Prompt("Use previous secret key ? (y/n)");

        ulong[] encodedSecretKey;
        if (usePreviousKey.ToLower() == "y")
        {
            secretKeySelect:
            string keyFileName = Prompt("Enter the name of the secret key:");
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
            string secretKey = Prompt("Enter the secret key:");
            encodedSecretKey = textNumberConverter.ConvertToNumber(secretKey);
        }
        string usePreviousSBox = Prompt("Use Previously generated S-Box ? (y/n)");
        byte[] inverseSBox;  
        if (usePreviousSBox.ToLower() == "y")
        {
            sBoxSelect:
            string sBoxFileName = Prompt("Enter the name of the S-Box:");
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
            string seed = Prompt("Enter S-Box Generation Seed");
            byte[] sBox = sBoxGenerator.GenerateSBox(seed);
            inverseSBox = sBoxGenerator.GenerateInverseSBox(sBox);
        }

        ulong[] decryptedMessage = symmetricEncryption.Decrypt(encryptedBytes, encodedSecretKey, inverseSBox);
        Console.WriteLine("Decrypted numbers: " + string.Join(", ", decryptedMessage));
        string decryptedString = textNumberConverter.ConvertToText(decryptedMessage);
        Console.WriteLine("Decrypted string: " + decryptedString);

    }
}