namespace CaesarCypher.Cyphers;

public sealed class DandyCaesarSalad8
{
    public static string Encrypt(string input, int shift)
    {
        return string.Create(input.Length, (input, shift: shift % 26), static (span, args) =>
        {
            var salad = "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxy"u8;
            for (var i = 0; i < span.Length; i++)
            {
                var c = args.input[i];

                if (c >= 'a')
                {
                    if (c <= 'z')
                    {
                        c = (char)salad[args.shift + c + (51 - 'a')];
                    }
                }
                else if (c >= 'A')
                {
                    if (c <= 'Z')
                    {
                        c = (char)salad[args.shift + c - 'A'];
                    }
                }

                span[i] = c;
            }
        });
    }
}

