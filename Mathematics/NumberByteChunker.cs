class NumberByteChunker
{
    public byte[] DeChunk(ulong[] inputNumber)
    {
        byte[] byteArray = new byte[inputNumber.Length * 8];
        for (int i = 0; i < inputNumber.Length * 8; i++)
        {
            if (i%8 == 0)
            {
                byteArray[i] = (byte)(inputNumber[i/8] % 256);
            }
            else
            {
                byteArray[i] += (byte)((inputNumber[i/8] / (ulong)Math.Pow(256, i%8)) % 256);
            }
        }
        return byteArray;
    }
    public ulong[] Chunk(byte[] inputBytes)
    {
        int n = inputBytes.Length;
        ulong[] result = new ulong[(n + 7) / 8]; // Calculate the size of the result array
        for (int i = 0; i < n; i++)
        {
            if(i%8 == 0)
            {
                result[i/8] = inputBytes[i];
            }
            else
            {
                result[i/8] += inputBytes[i]*(ulong)Math.Pow(256, i%8);
            }
        }
        return result;
    }
}