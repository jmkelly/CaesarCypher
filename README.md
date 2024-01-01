# CaesarCypher

The most comprehensive implementations of the Caesar Cypher written in C# IT THE WORLD!!!!

I (@jmkelly) got nerdniped because ChatGpt wrote a quicker implementation than I did initially.  I have subsequently nerdsniped @xt0rted and @aarondandy who provided most of the implementations (including all the fast ones)

Various different techniques including arrays of char, unsafe string manipulation, math shortcuts, spans, vectors and more....

Adding implementations:

To automatically test, drop a sealed class (for performance reasons) into the Core/Cyphers directory / namespace.  As long as it has a static Encrypt method with text string and int shift parameters that returns a string the tests will be hooked up.

Eg.

`public sealed class AnotherEncrypter {
    public static string Encrypt(string text, int shift)
    {
        //go nuts with your implementation!
    }
}`


To run the benchmarks, goto the CaesarCypher.Cmdline directory and in your cmdline run `dotnet run -c Release`
