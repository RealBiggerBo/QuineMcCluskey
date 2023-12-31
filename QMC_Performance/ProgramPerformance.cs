﻿using BenchmarkDotNet.Attributes;
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
            BenchmarkRunner.Run<QMC_Benchmark>();
        }
    }

    //[Config(typeof(AntiVirusFriendlyConfig))]
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

        [Benchmark]
        public void BranchingTest2_Char()
        {
            CharQuineMcCluskeySolver solver = new CharQuineMcCluskeySolver(1, 6, 12, 14, 16, 20, 26, 31, 34, 38, 42, 46, 50, 56, 57, 63);

            solver.Solve();
        }

        [Benchmark]
        public void BranchingTest2_Int()
        {
            IntQuineMcCluskeySolver solver = new IntQuineMcCluskeySolver(1, 6, 12, 14, 16, 20, 26, 31, 34, 38, 42, 46, 50, 56, 57, 63);

            solver.Solve();
        }
    }

    [MemoryDiagnoser]
    public class Value_Benchmark
    {
        Value_Base charValA;
        Value_Base charValB;
        Value_Optimised intValA;
        Value_Optimised intValB;

        [GlobalSetup]
        public void Startup()
        {
            charValA = new Value_Base("0001");
            charValB = new Value_Base("0011");
            intValA = new Value_Optimised("0001");
            intValB = new Value_Optimised("0011");
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
            Value_Base newCharVal = new Value_Base(charValA, charValB);
        }

        [Benchmark]
        public void CreateSimilar_IntValue()
        {
            Value_Optimised newIntVal = new Value_Optimised(intValA, intValB);
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
        [Benchmark]
        public void BitAnd()
        {
            int res = 13 & 0b1;
        }

        [Benchmark]
        public void Modulo()
        {
            int res = 13 % 2;
        }
    }

    public class AntiVirusFriendlyConfig : ManualConfig
    {
        public AntiVirusFriendlyConfig()
        {
            AddJob(Job.MediumRun.WithToolchain(InProcessNoEmitToolchain.Instance));
        }
    }
}
