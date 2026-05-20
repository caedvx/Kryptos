using System.Collections.Specialized;
using System.Runtime.InteropServices;

class SymmetricEncryption
{
    NumberByteChunker numberByteChunker = new NumberByteChunker();
    SBoxGenerator sBoxGenerator = new SBoxGenerator();

    public byte[] Encrypt(ulong[] message, ulong[] secretKey, byte[] sBox)
    {
        int n = message.Length;
        ulong[] encryptedMessage = new ulong[n];
        for(int i = 0; i < n; i++)
        {
            encryptedMessage[i] = message[i] ^ secretKey[i % secretKey.Length];
        }

        byte[] encryptedBytes = numberByteChunker.DeChunk(encryptedMessage);
        sBoxGenerator.GenerateInverseSBox(sBox);
        byte[] resultBytes = new byte[encryptedBytes.Length];
        for (int i = 0; i < encryptedBytes.Length; i++)
        {
            resultBytes[i] = sBox[encryptedBytes[i]];
        }
        return resultBytes;
    }

    public ulong[] Decrypt(byte[] encryptedMessage, ulong[] secretKey, byte[] inverseSBox)
    {
        int n = encryptedMessage.Length;
        byte[] sBoxDecryptedBytes = new byte[encryptedMessage.Length];
        for (int i = 0; i < encryptedMessage.Length; i++)
        {
            sBoxDecryptedBytes[i] = inverseSBox[encryptedMessage[i]];
        }

        ulong[] decryptedMessage = new ulong[n];
        for(int i = 0; i < n; i++)
        {
            decryptedMessage[i] = sBoxDecryptedBytes[i] ^ secretKey[i % secretKey.Length];
        }
        return decryptedMessage;
    }
}   