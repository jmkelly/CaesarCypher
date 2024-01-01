using Xunit.Abstractions;

namespace CaesarCypher;


public class FactoryTests
{
    private readonly ITestOutputHelper output;

    public FactoryTests(ITestOutputHelper output)
    {
        this.output = output;
    }
    [Fact]
    public void Test()
    {
        var factory = new CaesorCypherFactory(output);
        var types = factory.GetCypherTypes();
        output.WriteLine($"number of types: {types.Length}");
        types.Length.ShouldNotBe(0);
    }
}
