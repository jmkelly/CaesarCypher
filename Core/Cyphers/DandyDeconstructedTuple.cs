namespace CaesarCypher.Cyphers;

public sealed class DandyDeconstructedTuple
{
    public static string Encrypt(string given, int shift)
    {
        if (shift == 0)
        {
            return given;
        }

        return string.Create(given.Length, (given, shift: shift % 26), static (span, args) =>
        {
            var (given, shift) = args;
            for (var i = 0; i < span.Length; i++)
            {
                char c = given[i];

                if (c is >= 'a' and <= 'z')
                {
                    c = (char)(shift + c);
                    if (c > 'z')
                    {
                        c -= (char)26;
                    }
                }
                else if (c is >= 'A' and <= 'Z')
                {
                    c = (char)(shift + c);
                    if (c > 'Z')
                    {
                        c -= (char)26;
                    }
                }

                span[i] = c;
            }
        });
    }
}


