using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using QuineMcCluskey;

namespace QMC_Performance
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<QMC_Benchmark>();
        }
    }

    public class QMC_Benchmark
    {
        [Benchmark]
        public void RunDefaultTest()
        {
            QuineMcCluskeySolver solver = new QuineMcCluskeySolver(1, 3, 5, 10, 11, 12, 13, 14, 15);

            solver.Solve();
        }
    }
}
