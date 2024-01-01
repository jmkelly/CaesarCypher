namespace CaesarCypher.Cyphers;

public sealed class BrianOriginal
{
    public static string Encrypt(string input, int shift)
    {
        return string.Create(
            input.Length,
            (input, shift: shift % 26),
            static (span, args) =>
            {
                for (var i = 0; i < span.Length; i++)
                {
                    var c = args.input[i];
                    if (char.IsLetter(c))
                    {
                        span[i] = (char)(c + args.shift);
                        if (char.IsUpper(c))
                        {
                            var newC = span[i];
                            if (newC > 'Z')
                            {
                                span[i] = (char)(newC - 26);
                            }
                        }
                        else
                        {
                            var newC = span[i];
                            if (newC > 'z')
                            {
                                span[i] = (char)(newC - 26);
                            }
                        }
                    }
                    else
                    {
                        span[i] = c;
                    }
                }
            });
    }
}
