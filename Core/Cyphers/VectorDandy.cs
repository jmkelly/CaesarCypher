public sealed class VectorDandy
{
    public static string Encrypt(string input, int shift)
    {
        return string.Create(
            input.Length,
            (input, shift: (ushort)(shift % 26)),
            static (output, state) =>
            {
                var input = state.input.AsSpan();
                int i = 0;
                if (Vector256.IsHardwareAccelerated && input.Length >= Vector256<ushort>.Count)
                {
                    i = applyShiftVector256(output, input, i, state.shift);
                }
                if (Vector128.IsHardwareAccelerated && (input.Length - i) >= Vector128<ushort>.Count)
                {
                    i = applyShiftVector128(output, input, i, state.shift);
                }
                if (Vector64.IsHardwareAccelerated && (input.Length - i) >= Vector64<ushort>.Count)
                {
                    i = applyShiftVector64(output, input, i, state.shift);
                }
                for (; i < input.Length; ++i)
                {
                    output[i] = applyShift(input[i], state.shift);
                }
            }
        );

        static int applyShiftVector256(Span<char> output, ReadOnlySpan<char> input, int i, ushort shiftBy)
        {
            var A = Vector256.Create((ushort)'A'); // 65
            var Z = Vector256.Create((ushort)'Z'); // 90
            var a = Vector256.Create((ushort)'a'); // 97
            var z = Vector256.Create((ushort)'z'); // 122

            var offset = Vector256.Create((ushort)26);

            var vecShift = Vector256.Create(shiftBy);

            ref readonly ushort inputRef = ref Unsafe.As<char, ushort>(ref MemoryMarshal.GetReference(input));
            ref ushort outputRef = ref Unsafe.As<char, ushort>(ref MemoryMarshal.GetReference(output));

            do
            {
                var vec = Vector256.LoadUnsafe(in inputRef, (nuint)i);

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

                wrapped.StoreUnsafe(ref outputRef, (nuint)i);

                i += Vector256<ushort>.Count;
            }
            while (output.Length - i >= Vector256<ushort>.Count);

            return i;
        }

        static int applyShiftVector128(Span<char> output, ReadOnlySpan<char> input, int i, ushort shiftBy)
        {
            var A = Vector128.Create((ushort)'A'); // 65
            var Z = Vector128.Create((ushort)'Z'); // 90
            var a = Vector128.Create((ushort)'a'); // 97
            var z = Vector128.Create((ushort)'z'); // 122

            var offset = Vector128.Create((ushort)26);

            var vecShift = Vector128.Create(shiftBy);

            ref readonly ushort inputRef = ref Unsafe.As<char, ushort>(ref MemoryMarshal.GetReference(input));
            ref ushort outputRef = ref Unsafe.As<char, ushort>(ref MemoryMarshal.GetReference(output));

            do
            {
                var vec = Vector128.LoadUnsafe(in inputRef, (nuint)i);

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

                wrapped.StoreUnsafe(ref outputRef, (nuint)i);

                i += Vector128<ushort>.Count;
            }
            while (output.Length - i >= Vector128<ushort>.Count);


            return i;
        }

        static int applyShiftVector64(Span<char> output, ReadOnlySpan<char> input, int i, ushort shiftBy)
        {
            var A = Vector64.Create((ushort)'A'); // 65
            var Z = Vector64.Create((ushort)'Z'); // 90
            var a = Vector64.Create((ushort)'a'); // 97
            var z = Vector64.Create((ushort)'z'); // 122

            var offset = Vector64.Create((ushort)26);

            var vecShift = Vector64.Create(shiftBy);

            ref readonly ushort inputRef = ref Unsafe.As<char, ushort>(ref MemoryMarshal.GetReference(input));
            ref ushort outputRef = ref Unsafe.As<char, ushort>(ref MemoryMarshal.GetReference(output));

            do
            {
                var vec = Vector64.LoadUnsafe(in inputRef, (nuint)i);

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

                wrapped.StoreUnsafe(ref outputRef, (nuint)i);

                i += Vector64<ushort>.Count;
            }
            while (output.Length - i >= Vector64<ushort>.Count);

            return i;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static char applyShift(char c, ushort shiftBy)
        {
            if (c is >= 'A' and <= 'Z')
            {
                c += (char)shiftBy;
                if (c > 'Z')
                {
                    c -= (char)26;
                }
            }
            else if (c is >= 'a' and <= 'z')
            {
                c += (char)shiftBy;
                if (c > 'z')
                {
                    c -= (char)26;
                }
            }

            return c;
        }
    }
}
