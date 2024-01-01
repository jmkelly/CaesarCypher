using System.Reflection;
using CaesarCypher.Cyphers;
using Xunit.Abstractions;

namespace CaesarCypher;

public class CaesarCypherTests
{

    [Theory]
    [InlineData("abc", 1, "bcd")]
    [InlineData("abc", 2, "cde")]
    [InlineData("aBc", 1, "bCd")]
    [InlineData("xyz", 1, "yza")]
    [InlineData("xyz1", 1, "yza1")]
    [InlineData("x3yz1", 1, "y3za1")]
    public void CaesorCypher_WhenGivenText_ShouldShiftCorrectly(string text, int shift, string result)
    {
        var encrypted = KelsoCharArray.Encrypt(text, shift);
        encrypted.ShouldBe(result);
    }

    [Theory]
    [InlineData("abc", 1, "bcd")]
    [InlineData("abc", 2, "cde")]
    [InlineData("aBc", 1, "bCd")]
    [InlineData("xyz", 1, "yza")]
    [InlineData("xyz1", 1, "yza1")]
    [InlineData("x3yz1", 1, "y3za1")]
    public void CaesorCypherBrianVectore_WhenGivenText_ShouldShiftCorrectly(string text, int shift, string result)
    {
        var encrypted = BrianFirstVector.Encrypt(text, shift);
        encrypted.ShouldBe(result);
    }
    [Theory]
    [InlineData("abc", 1, "bcd", "abc")]
    [InlineData("abc", 2, "cde", "abc")]
    [InlineData("aBc", 1, "bCd", "aBc")]
    [InlineData("xyz", 1, "yza", "xyz")]
    [InlineData("xyz1", 1, "yza1", "xyz1")]
    [InlineData("x3yz1", 1, "y3za1", "x3yz1")]
    public void CaesorCypherWithSpan_WhenGivenText_ShouldShiftCorrectly(string text, int shift, string result, string ori)
    {
        var encrypted = KelsoUnsafe.Encrypt(text, shift);
        encrypted.ShouldBe(result);
        //make sure i haven't stuffed my original text
        text.ShouldBe(ori);

    }

    [Theory]
    [InlineData("abc", 1, "bcd", "abc")]
    [InlineData("abc", 2, "cde", "abc")]
    [InlineData("aBc", 1, "bCd", "aBc")]
    [InlineData("xyz", 1, "yza", "xyz")]
    [InlineData("xyz1", 1, "yza1", "xyz1")]
    [InlineData("x3yz1", 1, "y3za1", "x3yz1")]
    public void CaesorCypherDandy_WhenGivenText_ShouldShiftCorrectly(string text, int shift, string result, string ori)
    {
        var encrypted = DandyGoto.Encrypt(text, shift);
        encrypted.ShouldBe(result);
        //make sure i haven't stuffed my original text
        text.ShouldBe(ori);

    }

    [Theory]
    [InlineData("abc", 1, "bcd", "abc")]
    [InlineData("abc", 2, "cde", "abc")]
    [InlineData("aBc", 1, "bCd", "aBc")]
    [InlineData("xyz", 1, "yza", "xyz")]
    [InlineData("xyz1", 1, "yza1", "xyz1")]
    [InlineData("x3yz1", 1, "y3za1", "x3yz1")]
    public void CaesorCypherBrianOriginal_WhenGivenText_ShouldShiftCorrectly(string text, int shift, string result, string ori)
    {
        var encrypted = BrianOriginal.Encrypt(text, shift);
        encrypted.ShouldBe(result);
        //make sure i haven't stuffed my original text
        text.ShouldBe(ori);

    }

    [Theory]
    [InlineData("abc", 1, "bcd", "abc")]
    [InlineData("abc", 2, "cde", "abc")]
    [InlineData("aBc", 1, "bCd", "aBc")]
    [InlineData("xyz", 1, "yza", "xyz")]
    [InlineData("xyz1", 1, "yza1", "xyz1")]
    [InlineData("x3yz1", 1, "y3za1", "x3yz1")]
    public void CaesorCypherDandyOriginal_WhenGivenText_ShouldShiftCorrectly(string text, int shift, string result, string ori)
    {
        var encrypted = DandyOriginal.Encrypt(text, shift);
        encrypted.ShouldBe(result);
        //make sure i haven't stuffed my original text
        text.ShouldBe(ori);

    }

    [Theory]
    [InlineData("abc", 1, "bcd", "abc")]
    [InlineData("abc", 2, "cde", "abc")]
    [InlineData("aBc", 1, "bCd", "aBc")]
    [InlineData("xyz", 1, "yza", "xyz")]
    [InlineData("xyz1", 1, "yza1", "xyz1")]
    [InlineData("x3yz1", 1, "y3za1", "x3yz1")]
    public void CaesorCypherDandyGoto_WhenGivenText_ShouldShiftCorrectly(string text, int shift, string result, string ori)
    {
        var encrypted = DandyGoto.Encrypt(text, shift);
        encrypted.ShouldBe(result);
        //make sure i haven't stuffed my original text
        text.ShouldBe(ori);

    }

    public CaesarCypherTests(ITestOutputHelper output)
    {
        this.output = output;
    }
    private readonly ITestOutputHelper output;

    //[Theory]
    //[InlineData("abc", 1, "bcd", "abc")]
    [Fact]
    public void ATest()
    {
        //create the encrypter via the factory
        var factory = new CaesorCypherFactory(output);
        string methodName = "Encrypt";
        foreach (var type in factory.GetCypherTypes())
        {
            output.WriteLine($"creating {type.Name}");
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod(methodName, BindingFlags.Static);
            if (method == null)
            {
                throw new Exception($"{methodName} does not exist on type {type.Name}");
            }

            //TODO params
            string text = "abc";
            int shift = 1;

            object[] parameters = { text, shift };
            string result = (string)method.Invoke(instance, parameters);
            result.ShouldBe("bcd");

        }


    }


}

