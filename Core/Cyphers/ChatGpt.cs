namespace CaesarCypher.Cyphers;

public sealed class ChatGpt
{
    public static string Encrypt(string text, int shift)
    {
        char[] charArray = text.ToCharArray();

        for (int i = 0; i < charArray.Length; i++)
        {
            if (char.IsLetter(charArray[i]))
            {
                char baseLetter = char.IsUpper(charArray[i]) ? 'A' : 'a';
                charArray[i] = (char)(((charArray[i] - baseLetter + shift) % 26) + baseLetter);
            }
        }

        return new string(charArray);
    }
}
