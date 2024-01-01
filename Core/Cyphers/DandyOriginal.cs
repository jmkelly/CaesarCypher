namespace CaesarCypher.Cyphers;

public sealed class DandyOriginal
{
    public static string Encrypt(string given, int shift)
    {
        return string.Create(given.Length, (given, shift), (span, args) =>
        {
            for (var i = 0; i < span.Length; i++)
            {
                var c = args.given[i];
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
                    span[i] = c;
                    continue;
                }

                c = (char)(c - b + args.shift);
                if (c >= 26)
                {
                    c -= (char)26;
                }

                span[i] = (char)(c + b);
            }
        });
    }
}
