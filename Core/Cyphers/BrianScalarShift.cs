using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CaesarCypher.Cyphers
{
	public sealed class BrianScalarShift
	{
		public static string Encrypt(string str, int shift)
		{
			return string.Create(
				str.Length,
				(
					input: str,
					shift: (ushort)(shift % 26)
				),
				static (buffer, state) =>
				{
					nuint bufferLength = (nuint)buffer.Length;

					ref ushort input = ref Unsafe.As<char, ushort>(ref MemoryMarshal.GetReference(state.input.AsSpan()));
					ref ushort output = ref Unsafe.As<char, ushort>(ref MemoryMarshal.GetReference(buffer));

					ScalarShift(ref output, ref input, bufferLength, state.shift);
				}
			);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)] private static void ScalarShift(ref ushort output, ref ushort input, nuint bufferLength, ushort shiftBy)
		{
			ref var end = ref Unsafe.Add(ref input, bufferLength);

			while (true)
			{
				var c = input;

				if (c >= 'A' && c <= 'Z')
				{
					c += shiftBy;

					if (c > 'Z')
					{
						c -= 26;
					}
				}
				else if (c >= 'a' && c <= 'z')
				{
					c += shiftBy;

					if (c > 'z')
					{
						c -= 26;
					}
				}

				output = c;

				if (Unsafe.IsAddressGreaterThan(ref input, ref end))
				{
					break;
				}

				input = ref Unsafe.Add(ref input, 1);
				output = ref Unsafe.Add(ref output, 1);
			}
		}
	}
}
