using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using QuineMcCluskey;

namespace QMC_Performance
{
    internal class ProgramPerformance
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

        [Benchmark]
        public void RunBranchingTest()
        {
            QuineMcCluskeySolver solver = new QuineMcCluskeySolver(1, 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 33, 41, 43, 47, 53, 59, 61);

            solver.Solve();
        }
    }
}
