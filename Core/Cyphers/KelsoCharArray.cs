namespace CaesarCypher.Cyphers;

public sealed class KelsoCharArray
{
    public static string Encrypt(string text, int shift)
    {
        char[] alphabet = new char[26] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        const int numberOfLetters = 26;
        var chars = text.ToCharArray();
        int length = chars.Length;
        var newChars = new char[length];
        int currentPosition = 0;
        int newPosition = 0;
        for (int i = 0; i < length; i++)
        {
            bool isLetter = char.IsLetter(chars[i]);

            if (!isLetter)
            {
                newChars[i] = chars[i];
                continue;
            }

            if (isLetter && char.IsUpper(chars[i]))
            {
                currentPosition = Array.IndexOf(alphabet, char.ToLower(chars[i]));
                newPosition = (currentPosition + shift) % numberOfLetters;
                newChars[i] = char.ToUpper(alphabet[newPosition]);
                continue;
            }
            else
            {

                currentPosition = Array.IndexOf(alphabet, chars[i]);
                newPosition = (currentPosition + shift) % numberOfLetters;
                newChars[i] = alphabet[newPosition];
            }
        }
        return new string(newChars);
    }
}
