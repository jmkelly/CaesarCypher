using System.Runtime.Intrinsics;

namespace CaesarCypher.Cyphers;

public sealed class BrianFirstVector
{
    public static string Encrypt(string text, int shift)
    {
        return string.Create(
            text.Length,
            (text, shift: shift % 26),
            static (span, args) =>
            {
                var vecAA = Vector128.Create((int)'A');
                var vecZZ = Vector128.Create((int)'Z');
                var vecA = Vector128.Create((int)'a');
                var vecZ = Vector128.Create((int)'z');
                var vecShift = Vector128.Create(args.shift);
                var vec26 = Vector128.Create(26);

                for (var i = 0; i < span.Length; i++)
                {
                    var vc = Vector128.Create((int)args.text[i]);

                    if (Vector128.GreaterThanOrEqualAny(vc, vecAA) && Vector128.LessThanOrEqualAny(vc, vecZZ))
                    {
                        vc = Vector128.Add(vc, vecShift);

                        if (Vector128.GreaterThanAny(vc, vecZZ))
                        {
                            vc = Vector128.Subtract(vc, vec26);
                        }
                    }
                    else if (Vector128.GreaterThanOrEqualAny(vc, vecA) && Vector128.LessThanOrEqualAny(vc, vecZ))
                    {
                        vc = Vector128.Add(vc, vecShift);

                        if (Vector128.GreaterThanAny(vc, vecZ))
                        {
                            vc = Vector128.Subtract(vc, vec26);
                        }
                    }

                    span[i] = (char)vc.GetElement(0);
                }
            });
    }
}
