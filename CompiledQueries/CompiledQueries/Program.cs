// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using CompiledQueries;

internal class program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<EfCompiledQueryBenchmark>();
        Console.WriteLine("Hello, World!");
    }
}
