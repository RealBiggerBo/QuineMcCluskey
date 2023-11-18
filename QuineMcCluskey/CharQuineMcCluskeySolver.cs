using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuineMcCluskey
{
    public class CharQuineMcCluskeySolver
    {
        private int numSignals;
        private int[] outputs;
        private CharValue[] outputValues;

        public CharQuineMcCluskeySolver(params int[] outputs)
        {
            this.outputs = outputs;
            numSignals = MathF.ILogB(MathF.Max(1, outputs.Max())) + 1;

            outputValues = new CharValue[outputs.Length];
            for (int i = 0; i < outputs.Length; i++)
            {
                outputValues[i] = new CharValue(outputs[i], numSignals);
            }
        }

        public CharIteration Solve()
        {
            //Setup InputIteration
            CharIteration inputIteration = new CharIteration();
            for (int j = 0; j < outputValues.Length; j++)
            {
                inputIteration.Add(outputValues[j]);
            }

            //Phase one
            CharIteration resultPhaseOne = SolvePhaseOne(inputIteration);

            //Phase two
            CharTable table = new CharTable(outputValues, resultPhaseOne.GetValues());
            CharIteration finalResult = SolvePhaseTwo(table);

            return finalResult;
        }

        private CharIteration SolvePhaseOne(CharIteration inputIteration)
        {
            CharIteration result = new CharIteration();
            while (inputIteration.GetLength() != 0)
            {
                inputIteration.GetNextIteration(out CharIteration nextIteration, out CharIteration notUsedIteration);
                inputIteration = nextIteration;
                result.Add(notUsedIteration);
            }
            return result;
        }
        private CharIteration SolvePhaseTwo(CharTable inputTable)
        {
            CharIteration result = new CharIteration();

            while (true)
            {
                bool changed = false;

                //Essentialität
                while (GetEssential(ref inputTable, out CharValue val))
                {
                    changed = true;
                    result.Add(val);
                }

                //Zeilendominanz
                while (inputTable.GetRowDominance(ref inputTable))
                {
                    changed = true;
                }

                //Reihendominanz
                while (inputTable.GetColumnDominance(ref inputTable))
                {
                    changed = true;
                }

                if (!changed || inputTable.GetLength() == 0)
                    break;
            }
            //Verzweigungsmethode
            //TODO
            if(inputTable.GetLength() != 0)
            {
                List<CharIteration> iterations = new List<CharIteration>();
                for (int i = 0; i < inputTable.height; i++)
                {
                    CharIteration partial = new CharIteration(inputTable.GetImplicant(i));
                    iterations.Add(partial);
                    partial.Add(SolvePhaseTwo(new CharTable(inputTable, i)));
                }
                int maxDontCareCount = iterations.Max((iteration) => iteration.GetDontCareCount());
                result.Add(iterations.First((iteration) => iteration.GetDontCareCount() == maxDontCareCount));
            }
            return result;
        }

        private bool GetEssential(ref CharTable table, out CharValue essential)
        {
            essential = default;

            int essentialY = -1;
            for (int x = 0;x < table.width;x++)
            {
                bool prime = false;
                for (int y = 0;y < table.height;y++) 
                {
                    int marked = table.Get(x, y);
                    if (marked == 1)
                    {
                        prime = !prime;
                        if (!prime)
                        {
                            break;
                        }
                        essential = table.GetImplicant(y);
                        essentialY = y;
                    }
                }

                if(prime)
                {
                    table.RemovePrimeImplicant(essentialY);
                    return true;
                }
            }
            return false;
        }
    }

    public class IntQuineMcCluskeySolver
    {
        private int numSignals;
        private int[] outputs;
        private IntValue[] outputValues;

        public IntQuineMcCluskeySolver(params int[] outputs)
        {
            this.outputs = outputs;
            numSignals = MathF.ILogB(MathF.Max(1, outputs.Max())) + 1;

            outputValues = new IntValue[outputs.Length];
            for (int i = 0; i < outputs.Length; i++)
            {
                outputValues[i] = new IntValue(outputs[i], numSignals);
            }
        }

        public IntIteration Solve()
        {
            //Setup InputIteration
            IntIteration inputIteration = new IntIteration();
            for (int j = 0; j < outputValues.Length; j++)
            {
                inputIteration.Add(outputValues[j]);
            }

            //Phase one
            IntIteration resultPhaseOne = SolvePhaseOne(inputIteration);

            //Phase two
            IntTable table = new IntTable(outputValues, resultPhaseOne.GetValues());
            IntIteration finalResult = SolvePhaseTwo(table);

            return finalResult;
        }

        private IntIteration SolvePhaseOne(IntIteration inputIteration)
        {
            IntIteration result = new IntIteration();
            while (inputIteration.GetLength() != 0)
            {
                inputIteration.GetNextIteration(out IntIteration nextIteration, out IntIteration notUsedIteration);
                inputIteration = nextIteration;
                result.Add(notUsedIteration);
            }
            return result;
        }
        private IntIteration SolvePhaseTwo(IntTable inputTable)
        {
            IntIteration result = new IntIteration();

            while (true)
            {
                bool changed = false;

                //Essentialität
                while (GetEssential(ref inputTable, out IntValue val))
                {
                    changed = true;
                    result.Add(val);
                }

                //Zeilendominanz
                while (inputTable.GetRowDominance(ref inputTable))
                {
                    changed = true;
                }

                //Reihendominanz
                while (inputTable.GetColumnDominance(ref inputTable))
                {
                    changed = true;
                }

                if (!changed || inputTable.GetLength() == 0)
                    break;
            }
            //Verzweigungsmethode
            //TODO
            if (inputTable.GetLength() != 0)
            {
                List<IntIteration> iterations = new List<IntIteration>();
                List<Task> tasks = new List<Task>();
                for (int i = 0; i < inputTable.height; i++)
                {
                    IntIteration partial = new IntIteration(inputTable.GetImplicant(i));
                    iterations.Add(partial);
                    partial.Add(SolvePhaseTwo(new IntTable(inputTable, i)));
                }
                int maxDontCareCount = iterations.Max((iteration) => iteration.GetDontCareCount());
                result.Add(iterations.First((iteration) => iteration.GetDontCareCount() == maxDontCareCount));
            }
            return result;
        }

        private bool GetEssential(ref IntTable table, out IntValue essential)
        {
            essential = default;

            int essentialY = -1;
            for (int x = 0; x < table.width; x++)
            {
                bool prime = false;
                for (int y = 0; y < table.height; y++)
                {
                    int marked = table.Get(x, y);
                    if (marked == 1)
                    {
                        prime = !prime;
                        if (!prime)
                        {
                            break;
                        }
                        essential = table.GetImplicant(y);
                        essentialY = y;
                    }
                }

                if (prime)
                {
                    table.RemovePrimeImplicant(essentialY);
                    return true;
                }
            }
            return false;
        }
    }
}
