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
        private Value_Base[] outputValues;

        public CharQuineMcCluskeySolver(params int[] outputs)
        {
            this.outputs = outputs;
            numSignals = MathF.ILogB(MathF.Max(1, outputs.Max())) + 1;

            outputValues = new Value_Base[outputs.Length];
            for (int i = 0; i < outputs.Length; i++)
            {
                outputValues[i] = new Value_Base(outputs[i], numSignals);
            }
        }

        public Iteration_Base Solve()
        {
            //Setup InputIteration
            Iteration_Base inputIteration = new Iteration_Base();
            for (int j = 0; j < outputValues.Length; j++)
            {
                inputIteration.Add(outputValues[j]);
            }

            //Phase one
            Iteration_Base resultPhaseOne = SolvePhaseOne(inputIteration);

            //Phase two
            Table_Base table = new Table_Base(outputValues, resultPhaseOne.GetValues());
            Iteration_Base finalResult = SolvePhaseTwo(table);

            return finalResult;
        }

        private Iteration_Base SolvePhaseOne(Iteration_Base inputIteration)
        {
            Iteration_Base result = new Iteration_Base();
            while (inputIteration.GetLength() != 0)
            {
                inputIteration.GetNextIteration(out Iteration_Base nextIteration, out Iteration_Base notUsedIteration);
                inputIteration = nextIteration;
                result.Add(notUsedIteration);
            }
            return result;
        }
        private Iteration_Base SolvePhaseTwo(Table_Base inputTable)
        {
            Iteration_Base result = new Iteration_Base();

            while (true)
            {
                bool changed = false;

                //Essentialität
                while (GetEssential(ref inputTable, out Value_Base val))
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
                List<Iteration_Base> iterations = new List<Iteration_Base>();
                for (int i = 0; i < inputTable.height; i++)
                {
                    Iteration_Base partial = new Iteration_Base(inputTable.GetImplicant(i));
                    iterations.Add(partial);
                    partial.Add(SolvePhaseTwo(new Table_Base(inputTable, i)));
                }
                int maxDontCareCount = iterations.Max((iteration) => iteration.GetDontCareCount());
                result.Add(iterations.First((iteration) => iteration.GetDontCareCount() == maxDontCareCount));
            }
            return result;
        }

        private bool GetEssential(ref Table_Base table, out Value_Base essential)
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
        private Value_Optimised[] outputValues;

        public IntQuineMcCluskeySolver(params int[] outputs)
        {
            this.outputs = outputs;
            numSignals = MathF.ILogB(MathF.Max(1, outputs.Max())) + 1;

            outputValues = new Value_Optimised[outputs.Length];
            for (int i = 0; i < outputs.Length; i++)
            {
                outputValues[i] = new Value_Optimised(outputs[i], numSignals);
            }
        }

        public Iteration_Optimised Solve()
        {
            //Setup InputIteration
            Iteration_Optimised inputIteration = new Iteration_Optimised();
            for (int j = 0; j < outputValues.Length; j++)
            {
                inputIteration.Add(outputValues[j]);
            }

            //Phase one
            Iteration_Optimised resultPhaseOne = SolvePhaseOne(inputIteration);

            //Phase two
            Table_Optimised table = new Table_Optimised(outputValues, resultPhaseOne.GetValues());
            Iteration_Optimised finalResult = SolvePhaseTwo(table);

            return finalResult;
        }

        private Iteration_Optimised SolvePhaseOne(Iteration_Optimised inputIteration)
        {
            Iteration_Optimised result = new Iteration_Optimised();
            while (inputIteration.GetLength() != 0)
            {
                inputIteration.GetNextIteration(out Iteration_Optimised nextIteration, out Iteration_Optimised notUsedIteration);
                inputIteration = nextIteration;
                result.Add(notUsedIteration);
            }
            return result;
        }
        private Iteration_Optimised SolvePhaseTwo(Table_Optimised inputTable)
        {
            Iteration_Optimised result = new Iteration_Optimised();

            while (true)
            {
                bool changed = false;

                //Essentialität
                while (GetEssential(ref inputTable, out Value_Optimised val))
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
                List<Iteration_Optimised> iterations = new List<Iteration_Optimised>();
                List<Task> tasks = new List<Task>();
                for (int i = 0; i < inputTable.height; i++)
                {
                    Iteration_Optimised partial = new Iteration_Optimised(inputTable.GetImplicant(i));
                    iterations.Add(partial);
                    partial.Add(SolvePhaseTwo(new Table_Optimised(inputTable, i)));
                }
                int maxDontCareCount = iterations.Max((iteration) => iteration.GetDontCareCount());
                result.Add(iterations.First((iteration) => iteration.GetDontCareCount() == maxDontCareCount));
            }
            return result;
        }

        private bool GetEssential(ref Table_Optimised table, out Value_Optimised essential)
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
