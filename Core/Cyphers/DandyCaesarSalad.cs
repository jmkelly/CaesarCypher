namespace CaesarCypher.Cyphers;

public sealed class DandyCaesarSalad
{
    public static string Encrypt(string input, int shift)
    {
        const string salad = "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz";
        return string.Create(input.Length, (input, shift: shift % 26), static (span, args) =>
        {
            for (var i = 0; i < span.Length; i++)
            {
                var c = args.input[i];

                if (c >= 'a')
                {
                    if (c <= 'z')
                    {
                        c = salad[args.shift + c + (52 - 'a')];
                    }
                }
                else if (c >= 'A')
                {
                    if (c <= 'Z')
                    {
                        c = salad[args.shift + c - 'A'];
                    }
                }

                span[i] = c;
            }
        });
    }
}

