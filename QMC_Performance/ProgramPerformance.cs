using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;
using QuineMcCluskey;

namespace QMC_Performance
{
    internal class ProgramPerformance
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Random_Benchmark>();
        }
    }

    [MemoryDiagnoser]
    public class QMC_Benchmark
    {
        [Benchmark]
        public void DefaultTest_Char()
        {
            CharQuineMcCluskeySolver solver = new CharQuineMcCluskeySolver(1, 3, 5, 10, 11, 12, 13, 14, 15);

            solver.Solve();
        }

        [Benchmark]
        public void DefaultTest_Int()
        {
            IntQuineMcCluskeySolver solver = new IntQuineMcCluskeySolver(1, 3, 5, 10, 11, 12, 13, 14, 15);

            solver.Solve();
        }

        [Benchmark]
        public void BranchingTest_Char()
        {
            CharQuineMcCluskeySolver solver = new CharQuineMcCluskeySolver(1, 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 33, 41, 43, 47, 53, 59, 61);

            solver.Solve();
        }

        [Benchmark]
        public void BranchingTest_Int()
        {
            IntQuineMcCluskeySolver solver = new IntQuineMcCluskeySolver(1, 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 33, 41, 43, 47, 53, 59, 61);

            solver.Solve();
        }
    }

    [MemoryDiagnoser]
    public class Value_Benchmark
    {
        CharValue charValA;
        CharValue charValB;
        CharValue intValA;
        CharValue intValB;

        [GlobalSetup]
        public void Startup()
        {
            charValA = new CharValue("0001");
            charValB = new CharValue("0011");
            intValA = new CharValue("0001");
            intValB = new CharValue("0011");
        }

        [Benchmark]
        public void IsSimilar_CharValue()
        {
            charValA.IsSimilar(charValB);
            charValB.IsSimilar(charValA);
        }

        [Benchmark]
        public void IsSimilar_IntValue()
        {
            intValA.IsSimilar(intValB);
            intValB.IsSimilar(intValA);
        }

        [Benchmark]
        public void CreateSimilar_CharValue()
        {
            CharValue newCharVal = new CharValue(charValA, charValB);
        }

        [Benchmark]
        public void CreateSimilar_IntValue()
        {
            CharValue newIntVal = new CharValue(intValA, intValB);
        }

        [Benchmark]
        public void Implies_CharValue()
        {
            charValA.Implies(charValB);
            charValB.Implies(charValA);
        }

        [Benchmark]
        public void Implies_IntValue()
        {
            intValA.Implies(intValB);
            intValB.Implies(intValA);
        }

        [Benchmark]
        public void GetDontCare_CharValue()
        {
            charValA.GetDontCareCount();
            charValB.GetDontCareCount();
        }

        [Benchmark]
        public void GetDontCare_IntValue()
        {
            intValA.GetDontCareCount();
            intValB.GetDontCareCount();
        }

        [Benchmark]
        public void GetGroupIndex_CharValue()
        {
            charValA.GetGroupIndex();
            charValB.GetGroupIndex();
        }

        [Benchmark]
        public void GetGroupIndex_IntValue()
        {
            intValA.GetGroupIndex();
            intValB.GetGroupIndex();
        }
    }

    public class Random_Benchmark
    {
        List<IntGroup> groupList;

        [GlobalSetup]
        public void Setup()
        {
            groupList = [new IntGroup(new IntValue("0001"), new IntValue("0010"), new IntValue("0100"), new IntValue("1000"))];
        }

        [Benchmark]
        public void Any()
        {
            bool b = groupList.Any();
        }

        [Benchmark]
        public void Count()
        {
            bool b = groupList.Count() == 0;
        }
    }
}
