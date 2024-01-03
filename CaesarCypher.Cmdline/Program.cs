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
    private string data;

    [Params(10, 100, 1000)]
    //[Params(10)]
    public int N { get; set; }

    [Params(2, 25, 100)]
    //[Params(1, 25)]
    public int Shift { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        //generates ok reasonable looking text from a snippet from chatgpt
        data = Text.GetRandomishButRealisticText(N);
    }

    [Benchmark(Baseline = true)]
    public string Gpt() => ChatGpt.Encrypt(data, Shift);
    [Benchmark]
    public string DandysGoto() => DandyGoto.Encrypt(data, Shift);
    [Benchmark]
    public string DandysCaesarSalad() => DandyCaesarSalad.Encrypt(data, Shift);
    [Benchmark]
    public string DandysCaesarSalad8() => DandyCaesarSalad8.Encrypt(data, Shift);
    [Benchmark]
    public string BriansVector() => BrianVector.Encrypt(data, Shift);
    [Benchmark]
    public string DandysVector() => DandyVector.Encrypt(data, Shift);
    [Benchmark]
    public string BriansScalarShift() => BrianScalarShift.Encrypt(data, Shift);
}

public class Program
{
    private static void Main(string[] args)
    {
        //benchmark it
        var summary = BenchmarkRunner.Run<CaesarCypherBenchmark>();
    }
}
