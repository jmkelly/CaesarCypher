using System.Reflection;
using Xunit.Abstractions;

namespace CaesarCypher;

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
                .Where(t => String.Equals(t.Namespace, ns, StringComparison.Ordinal) && t.IsSealed && t.IsClass)
                //.Where(t => String.Equals(t.Namespace, ns, StringComparison.Ordinal))
                .ToArray();

        return cyphers;

    }
}


