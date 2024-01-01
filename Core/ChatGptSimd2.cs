using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace CaesarCypher;

//WRONG!! produces incorrect output
public static class ChatGptSimd2
{

    public static string Encrypt(string plaintext, int key)
    {
        return ProcessText(plaintext, key, true);
    }

    public static string Decrypt(string ciphertext, int key)
    {
        return ProcessText(ciphertext, key, false);
    }

    static string ProcessText(string text, int key, bool encrypt)
    {
        int length = text.Length;
        char[] result = new char[length];

        Vector128<byte> keyVector = Vector128<byte>.Zero.WithElement(0, (byte)key);

        for (int i = 0; i < length; i += 16)
        {
            Vector128<byte> inputVector = LoadVector(text, i);
            Vector128<byte> outputVector = encrypt ? EncryptVector(inputVector, keyVector) : DecryptVector(inputVector, keyVector);
            StoreVector(result, i, outputVector);
        }

        return new string(result);
    }

    static Vector128<byte> LoadVector(string text, int index)
    {
        Vector128<byte> vector = Vector128<byte>.Zero;
        for (int i = 0; i < 16 && (index + i) < text.Length; i++)
        {
            vector = vector.WithElement(i, (byte)text[index + i]);
        }
        return vector;
    }

    static void StoreVector(char[] result, int index, Vector128<byte> vector)
    {
        for (int i = 0; i < 16; i++)
        {
            if (index + i < result.Length)
            {
                result[index + i] = (char)vector.GetElement(i);
            }
        }
    }

    static Vector128<byte> EncryptVector(Vector128<byte> input, Vector128<byte> key)
    {
        // Perform SIMD encryption here
        return Sse2.Add(input, key);
    }

    static Vector128<byte> DecryptVector(Vector128<byte> input, Vector128<byte> key)
    {
        // Perform SIMD decryption here
        return Sse2.Subtract(input, key);
    }
}


