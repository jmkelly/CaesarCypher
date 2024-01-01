using Xunit.Abstractions;

namespace CaesarCypher;

public class MarkovTextTest
{
    private readonly ITestOutputHelper output;

    public MarkovTextTest(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Test()
    {
        var text = Text.GetRandomishButRealisticText(100);
        output.WriteLine(text);
        text.Length.ShouldBe(0);

    }

}
