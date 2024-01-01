namespace CaesarCypher.Cyphers;
using System.Text;

public sealed class StringBuilderVersion
{
    public static string Encrypt(string text, int shift)
    {
        char[] alphabet = new char[26] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        const int numberOfLetters = 26;
        int length = text.Length;
        StringBuilder sb = new StringBuilder();
        int currentPosition = 0;
        int newPosition = 0;
        foreach (char s in text)
        {
            switch (s)
            {
                case var letter when !char.IsLetter(letter):
                    sb.Append(letter);
                    break;
                case var letter when char.IsUpper(letter):
                    currentPosition = Array.IndexOf(alphabet, char.ToLower(letter));
                    newPosition = (currentPosition + shift) % numberOfLetters;
                    sb.Append(char.ToUpper(alphabet[newPosition]));
                    break;
                default:
                    currentPosition = Array.IndexOf(alphabet, s);
                    newPosition = (currentPosition + shift) % numberOfLetters;
                    sb.Append(alphabet[newPosition]);
                    break;
            }
        }
        return sb.ToString();
    }
}
