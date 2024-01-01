using System.Runtime.CompilerServices;

namespace CaesarCypher.Cyphers;

public sealed class DandyAfterBed
{
    public static string Encrypt(string given, int shift)
    {
        if (shift == 0)
        {
            return given;
        }

        return string.Create(given.Length, (given, shift: (char)(shift % 26)), static (span, args) =>
        {
            for (var i = 0; i < span.Length; i++)
            {
                span[i] = shiftify(args.given[i], args.shift);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static char shiftify(char c, char shift)
            {
                if (c is >= 'a' and <= 'z')
                {
                    c += shift;
                    if (c > 'z')
                    {
                        c -= (char)26;
                    }
                }
                else if (c is >= 'A' and <= 'Z')
                {
                    c += shift;
                    if (c > 'Z')
                    {
                        c -= (char)26;
                    }
                }

                return c;
            }
        });
    }

}

