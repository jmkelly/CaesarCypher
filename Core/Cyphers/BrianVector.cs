
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;


public sealed class BrianVector
{
    public unsafe static string Encrypt(string text, nint shift)
    {
        var strSpan = text.AsSpan();

        fixed (char* ptr = &MemoryMarshal.GetReference(strSpan))
        {
            return string.Create(
                strSpan.Length,
                (ptr: (IntPtr)ptr, shift: (short)(shift % 26)),
                static (buffer, state) =>
                {
                    fixed (char* ptr = &MemoryMarshal.GetReference(buffer))
                    {
                        var input = (short*)state.ptr.ToPointer();
                        var output = (short*)ptr;

                        nint i = 0;
                        nint n = buffer.Length;

                        if (Vector256.IsHardwareAccelerated && n >= Vector256<ushort>.Count)
                        {
                            do
                            {
                                Vector256Shift(ref output, ref input, i, state.shift);

                                i += Vector256<ushort>.Count;
                            }
                            while (i <= n - Vector256<ushort>.Count);
                        }

                        if (Vector128.IsHardwareAccelerated && n >= Vector128<ushort>.Count)
                        {
                            do
                            {
                                Vector128Shift(ref output, ref input, i, state.shift);

                                i += Vector128<ushort>.Count;
                            }
                            while (i <= n - Vector128<ushort>.Count);
                        }

                        if (Vector64.IsHardwareAccelerated && n >= Vector64<ushort>.Count)
                        {
                            do
                            {
                                Vector64Shift(ref output, ref input, i, state.shift);

                                i += Vector64<ushort>.Count;
                            }
                            while (i <= n - Vector64<ushort>.Count);
                        }

                        for (; i < n; ++i)
                        {
                            ShiftSingleCharacter(ref output, ref input, i, state.shift);
                        }
                    }
                }
            );
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe static void Vector256Shift(ref short* output, ref short* input, nint i, short shiftBy)
    {
        var A = Vector256.Create((short)'A'); // 65
        var Z = Vector256.Create((short)'Z'); // 90
        var a = Vector256.Create((short)'a'); // 97
        var z = Vector256.Create((short)'z'); // 122

        var offset = Vector256.Create((short)26);

        var vecShift = Vector256.Create(shiftBy);
        var vec = Vector256.Load(input + i);

        var inUpperRange = Vector256.BitwiseAnd(
            Vector256.GreaterThanOrEqual(vec, A),
            Vector256.LessThanOrEqual(vec, Z));
        var inLowerRange = Vector256.BitwiseAnd(
            Vector256.GreaterThanOrEqual(vec, a),
            Vector256.LessThanOrEqual(vec, z));
        var inRange = Vector256.BitwiseOr(
            inUpperRange,
            inLowerRange);

        var incremented = Vector256.ConditionalSelect(
            inRange,
            Vector256.Add(vec, vecShift),
            vec);

        var overflowUpper = Vector256.BitwiseAnd(
            inUpperRange,
            Vector256.GreaterThan(incremented, Z));
        var overflowLower = Vector256.GreaterThan(incremented, z);
        var overflow = Vector256.BitwiseOr(
            overflowUpper,
            overflowLower);

        var wrapped = Vector256.Subtract(
            incremented,
            Vector256.BitwiseAnd(overflow, offset));

        wrapped.Store(output + i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe static void Vector128Shift(ref short* output, ref short* input, nint i, short shiftBy)
    {
        var A = Vector128.Create((short)'A'); // 65
        var Z = Vector128.Create((short)'Z'); // 90
        var a = Vector128.Create((short)'a'); // 97
        var z = Vector128.Create((short)'z'); // 122

        var offset = Vector128.Create((short)26);

        var vecShift = Vector128.Create(shiftBy);
        var vec = Vector128.Load(input + i);

        var inUpperRange = Vector128.BitwiseAnd(
            Vector128.GreaterThanOrEqual(vec, A),
            Vector128.LessThanOrEqual(vec, Z));
        var inLowerRange = Vector128.BitwiseAnd(
            Vector128.GreaterThanOrEqual(vec, a),
            Vector128.LessThanOrEqual(vec, z));
        var inRange = Vector128.BitwiseOr(
            inUpperRange,
            inLowerRange);

        var incremented = Vector128.ConditionalSelect(
            inRange,
            Vector128.Add(vec, vecShift),
            vec);

        var overflowUpper = Vector128.BitwiseAnd(
            inUpperRange,
            Vector128.GreaterThan(incremented, Z));
        var overflowLower = Vector128.GreaterThan(incremented, z);
        var overflow = Vector128.BitwiseOr(
            overflowUpper,
            overflowLower);

        var wrapped = Vector128.Subtract(
            incremented,
            Vector128.BitwiseAnd(overflow, offset));

        wrapped.Store(output + i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe static void Vector64Shift(ref short* output, ref short* input, nint i, short shiftBy)
    {
        var A = Vector64.Create((short)'A'); // 65
        var Z = Vector64.Create((short)'Z'); // 90
        var a = Vector64.Create((short)'a'); // 97
        var z = Vector64.Create((short)'z'); // 122

        var offset = Vector64.Create((short)26);

        var vecShift = Vector64.Create(shiftBy);
        var vec = Vector64.Load(input + i);

        var inUpperRange = Vector64.BitwiseAnd(
            Vector64.GreaterThanOrEqual(vec, A),
            Vector64.LessThanOrEqual(vec, Z));
        var inLowerRange = Vector64.BitwiseAnd(
            Vector64.GreaterThanOrEqual(vec, a),
            Vector64.LessThanOrEqual(vec, z));
        var inRange = Vector64.BitwiseOr(
            inUpperRange,
            inLowerRange);

        var incremented = Vector64.ConditionalSelect(
            inRange,
            Vector64.Add(vec, vecShift),
            vec);

        var overflowUpper = Vector64.BitwiseAnd(
            inUpperRange,
            Vector64.GreaterThan(incremented, Z));
        var overflowLower = Vector64.GreaterThan(incremented, z);
        var overflow = Vector64.BitwiseOr(
            overflowUpper,
            overflowLower);

        var wrapped = Vector64.Subtract(
            incremented,
            Vector64.BitwiseAnd(overflow, offset));

        wrapped.Store(output + i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe static void ShiftSingleCharacter(ref short* output, ref short* input, nint i, short shiftBy)
    {
        if (input[i] >= 'A' && input[i] <= 'Z')
        {
            output[i] = (short)(input[i] + shiftBy);
            if (output[i] > 'Z')
            {
                output[i] -= 26;
            }
        }
        else if (input[i] >= 'a' && input[i] <= 'z')
        {
            output[i] = (short)(input[i] + shiftBy);
            if (output[i] > 'z')
            {
                output[i] -= 26;
            }
        }
        else
        {
            output[i] = input[i];
        }
    }
}
