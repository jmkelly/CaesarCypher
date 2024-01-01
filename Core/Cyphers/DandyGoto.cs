namespace CaesarCypher.Cyphers;

public sealed class DandyGoto
{
    public static string Encrypt(string text, int shift)
    {
        return string.Create(text.Length, (text, shift: (char)(shift % 26)), static (span, args) =>
        {
            for (var i = 0; i < span.Length; i++)
            {
                int c = args.text[i];

                if (c is >= 'A' and <= 'z')
                {
                    if (c >= 'a')
                    {
                        if (args.shift + c > 'z')
                        {
                            goto wrapShift;
                        }

                        goto shift;
                    }
                    else if (c <= 'Z')
                    {
                        if (args.shift + c > 'Z')
                        {
                            goto wrapShift;
                        }

                        goto shift;
                    }
                }

                goto assign;

            wrapShift:
                c -= 26;

            shift:
                c += args.shift;

            assign:
                span[i] = unchecked((char)c);
            }
        });
    }
}
