using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;
using CaesarCypher.Cyphers;

namespace CaesarCypher.Cmdline;

[MemoryDiagnoser]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
[EventPipeProfiler(EventPipeProfile.CpuSampling)]

public class CaesarCypherBenchmark
{
    private const int Shift = 10;
    private string data;

    [Params(100)]
    public int N { get; set; }

    [GlobalSetup]
    public void Setup()
    {

        char[] randomString = new char[N];
        var random = new Random();

        // Generate random characters and populate the array
        for (int i = 0; i < N; i++)
        {
            // Generate a random ASCII character (32 to 126 are printable ASCII characters)
            randomString[i] = (char)random.Next(32, 127);
        }

        // Convert the character array to a string
        data = new string(randomString);
    }

    // [Benchmark]//mine is slow
    // public string MyArrayOfChar() => CaesarCypher.EncryptWithCharArray(data, Shift);
    // //
    // [Benchmark]
    // public string MyStringBuilder() => CaesarCypher.EncryptWithStringBuilder(data, Shift);
    //
    // [Benchmark]
    // public string MyUnsafe() => CaesarCypher.EncryptWithUnsafe(data, Shift);
    //
    // [Benchmark]
    // public string Gpt() => CaesarCypher.EncryptGtp(data, Shift);
    //
    // [Benchmark]
    // public string OriginalDandy() => CaesarCypher.EncryptDandyOriginal(data, Shift);
    //
    // [Benchmark]
    // public string DandyRevised() => CaesarCypher.EncryptDandyRevised(data, Shift);
    //
    // [Benchmark]
    // public string Brian() => CaesarCypher.EncryptBrian(data, Shift);
    //
    // [Benchmark]
    // public string BrianVector() => CaesarCypher.EncyptBrianVector(data, Shift);
    //
    // [Benchmark]
    // public string DandyDeconstructedTuple() => CaesarCypher.EncryptDandyDeconstructTuple(data, Shift);
    //
    // [Benchmark]
    // public string DandyGoingToBed() => CaesarCypher.EncryptDandyIsGoingToBed(data, Shift);
    //
    //
    [Benchmark]
    public string DandysGoto() => DandyGoto.Encrypt(data, Shift);
    [Benchmark]
    public string DandysCaesarSalad() => DandyCaesarSalad.Encrypt(data, Shift);
    [Benchmark]
    public string DandysCaesarSalad8() => DandyCaesarSalad8.Encrypt(data, Shift);

}

public class Program
{
    private static void Main(string[] args)
    {
        //benchmark it
        var summary = BenchmarkRunner.Run<CaesarCypherBenchmark>();
    }
}
