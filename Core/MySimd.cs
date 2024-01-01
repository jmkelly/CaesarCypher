using System.Numerics;

namespace CaesarCypher;

//FIX doesn't work
public static class MySimd
{

    public static string Encrypt(string text, int shift)
    {
        var chars = text.ToCharArray();
        var length = chars.Length;
        var result = new int[length];

        var mask = new int[length];
        var mod = new int[length];
        var input = new int[length];

        for (int i = 0; i < length; i++)
        {
            mask[i] = shift;
            mod[i] = 26;
            input[i] = chars[i];
        }

        int remaining = length & Vector<int>.Count;

        for (int i = 0; i < length - remaining; i += Vector<int>.Count)
        {
            var vInput = new Vector<int>(input, i);
            var vMask = new Vector<int>(mask, i);
            //var vMod = new Vector<int>(mod, i);
            (vInput + vMask).CopyTo(result, i);
        }

        for (int i = length - remaining; i < length; i++)
        {
            result[i] = (char)((chars[i] + shift) % 26);
        }

        for (int i = 0; i < length; i++)
        {
            chars[i] = (char)result[i];
        }

        return new string(chars);
    }
}



