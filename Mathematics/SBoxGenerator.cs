class SBoxGenerator
{
    TextNumberConverter textNumberConverter = new TextNumberConverter();
    NumberByteChunker numberByteChunker = new NumberByteChunker();

    public byte[] GenerateSBox(string userSeed)
    {
        ulong[] numberSeed = textNumberConverter.ConvertToNumber(userSeed);
        byte[] byteSeed = numberByteChunker.DeChunk(numberSeed);
        
        int seed = 0;
        foreach (byte b in byteSeed)
        {
            seed = seed * 73 + b;
        }

        Random prng = new Random(seed);
        byte[] sBox = new byte[256];

        for (int i = 0; i < 256; i++)
        {
            sBox[i] = (byte)i;
        }

        for (int i = 255; i > 0; i--)
        {
            int j = prng.Next(i + 1);
            byte tempByte = sBox[i];
            sBox[i] = sBox[j];
            sBox[j] = tempByte;
        }

        return sBox;
    }
    public byte[] GenerateInverseSBox(byte[] sBox)
    {
        byte[] inverseSBox = new byte[256];
        for (int i = 0; i < 256; i++)
        {
            inverseSBox[sBox[i]] = (byte)i;
        }
        return inverseSBox;
    }

}