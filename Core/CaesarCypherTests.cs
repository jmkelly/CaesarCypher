using Xunit.Abstractions;

namespace CaesarCypher;

public class CaesarCypherTests
{
    private readonly ITestOutputHelper output;

    public CaesarCypherTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Theory]
    [InlineData("abc", 1, "bcd", "abc")]
    [InlineData("abc", 2, "cde", "abc")]
    [InlineData("aBc", 1, "bCd", "aBc")]
    [InlineData("xyz", 1, "yza", "xyz")]
    [InlineData("xyz1", 1, "yza1", "xyz1")]
    [InlineData("x3yz1", 1, "y3za1", "x3yz1")]
    public void CaesorCypher_WhenGivenText_ShouldShiftCorrectly(string text, int shift, string result, string ori)
    {
        //create the encrypter via the factory
        var factory = new CaesorCypherFactory(output);
        string methodName = "Encrypt";
        foreach (var type in factory.GetCypherTypes())
        {
            output.WriteLine($"creating {type.Name}");
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod(methodName);
            if (method == null)
            {
                throw new Exception($"{methodName} static method does not exist on type {type.Name}");
            }

            //TODO params

            object[] parameters = { text, shift };
            string encrypted = (string)method.Invoke(instance, parameters);
            encrypted.ShouldBe(result);
            //make sure input isn't mutated 
            text.ShouldBe(ori);
        }
    }
}

