
using System.Security.Cryptography;




public sealed class LFSR : ICryptoTransform
{
    public bool CanReuseTransform => false;

    public bool CanTransformMultipleBlocks => true;

    public int InputBlockSize => 1024;

    public int OutputBlockSize => 1024;

    public void Dispose()
    {
        
    }

    const int sizeMask = 0b_0000000_11111111_11111111_11111111;

    private int val, mask;

    public LFSR(int key)
    {
        val = sizeMask;

        mask = key;
    }

    public LFSR() : this(0b_0000000_10000000_00000000_00001101)
    {
        
    }

    private int newVal(int x)
    {
        x &= mask;

        var cnt = (int)System.Runtime.Intrinsics.X86.Popcnt.PopCount((uint)x);

        return cnt & 1;
    }

    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
    {
        byte[] input = inputBuffer[inputOffset..(inputOffset + inputCount)],
               output = outputBuffer[outputOffset..(outputOffset + inputCount)];

        for (int i = 0; i < inputCount; ++i)
        {
            for (int j = 0; j < 8; ++j)
                val = ((val << 1) & sizeMask) | newVal(val);

            output[i] = (byte)(input[i] ^ val);
        }

        return inputCount;
    }

    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
    {
        byte[] input = inputBuffer[inputOffset..(inputOffset + inputCount)],
               output = new byte[inputCount];

        for (int i = 0; i < inputCount; ++i)
        {
            for (int j = 0; j < 8; ++j)
                val = ((val << 1) & sizeMask) | newVal(val);

            output[i] = (byte)(input[i] ^ val);
        }

        return output;
    }
}