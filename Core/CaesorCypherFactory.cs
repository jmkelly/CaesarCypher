using System.Reflection;
using Xunit.Abstractions;

namespace CaesarCypher;

//Note: I know this is terrible....but it does appear to work as long as you have a sealed class with a static Encrypt method that takes a string and an int :)
public class CaesorCypherFactory
{
    public CaesorCypherFactory(ITestOutputHelper output)
    {
        this.output = output;
    }
    //get all the cyphers within the CaesarCypher.Cyphers namespace
    const string ns = "CaesarCypher.Cyphers";
    private readonly ITestOutputHelper output;

    public Type[] GetCypherTypes()
    {
        //use reflection to get all the cyphers in a namespace
        var assembly = Assembly.GetExecutingAssembly();
        output.WriteLine($"assembly name {assembly.GetName()}");
        var cyphers = assembly.GetTypes()
                .Where(t => String.Equals(t.Namespace, ns, StringComparison.Ordinal) && t.IsSealed && t.IsClass && t.GetMethod("Encrypt") != null)
                .ToArray();

        return cyphers;

    }
}


