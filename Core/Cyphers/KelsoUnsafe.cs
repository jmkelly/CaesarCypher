namespace CaesarCypher.Cyphers;

public sealed class KelsoUnsafe
{
    public static string Encrypt(string input, int shift)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }
        string result = string.Create(input.Length, (input), (span, args) =>
        {
            for (var i = 0; i < span.Length; i++)
            {

                char ch = input[i];
                span[i] = ch;
            }
        });

        unsafe
        {
            fixed (char* unsafeInput = result)
            {

                for (var i = 0; i < result.Length; i++)
                {
                    var c = result[i];
                    char b;

                    if (c is >= 'a' and <= 'z')
                    {
                        b = 'a';
                    }
                    else if (c is >= 'A' and <= 'Z')
                    {
                        b = 'A';
                    }
                    else
                    {
                        unsafeInput[i] = c;
                        continue;
                    }

                    c = (char)(c - b + shift);
                    if (c >= 26)
                    {
                        c -= (char)26;
                    }

                    unsafeInput[i] = (char)(c + b);
                }
            }
        }
        return result;
    }
}
