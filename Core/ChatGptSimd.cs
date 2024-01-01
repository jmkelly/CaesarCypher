using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace CaesarCypher;

//WRONG!! produces wrong output
public static class ChatGptSimd
{

    public static string Encrypt(string text, int shift)
    {
        return ProcessText(text, shift, true);
    }

    public static string Decrypt(string text, int shift)
    {
        return ProcessText(text, shift, false);
    }

    static string ProcessText(string text, int shift, bool encrypt)
    {
        char[] result = new char[text.Length];

        Vector128<byte> shiftVector = Vector128<byte>.Zero.WithElement(0, (byte)shift);
        Vector128<byte> mask = GetMask(encrypt);

        for (int i = 0; i < text.Length; i += 16)
        {
            Vector128<byte> input = LoadVector(text, i);
            Vector128<byte> shifted = Avx2.Add(input, shiftVector);

            Vector128<byte> resultVector = Avx2.And(shifted, mask);
            resultVector = Avx2.Or(resultVector, Avx2.AndNot(mask, input));

            StoreVector(result, i, resultVector);
        }

        return new string(result);
    }

    static Vector128<byte> GetMask(bool encrypt)
    {
        if (encrypt)
        {
            return Vector128<byte>.AllBitsSet;
        }

        return Vector128<byte>.Zero.WithElement(0, (byte)('A' - 1))
                                   .WithElement(4, (byte)('Z' + 1))
                                   .WithElement(8, (byte)('a' - 1))
                                   .WithElement(12, (byte)('z' + 1));
    }

    static Vector128<byte> LoadVector(string text, int index)
    {
        Vector128<byte> vector = Vector128<byte>.Zero;

        for (int i = 0; i < 16 && index + i < text.Length; i++)
        {
            vector = vector.WithElement(i, (byte)text[index + i]);
        }

        return vector;
    }

    static void StoreVector(char[] result, int index, Vector128<byte> vector)
    {
        for (int i = 0; i < 16 && index + i < result.Length; i++)
        {
            result[index + i] = (char)vector.GetElement(i);
        }
    }

}

