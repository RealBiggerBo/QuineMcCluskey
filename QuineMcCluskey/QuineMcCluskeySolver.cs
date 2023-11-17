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
    public class QuineMcCluskeySolver
    {
        private int numSignals;
        private int[] outputs;
        private Value[] outputValues;

        public QuineMcCluskeySolver(params int[] outputs)
        {
            this.outputs = outputs;
            numSignals = MathF.ILogB(MathF.Max(1, outputs.Max())) + 1;

            outputValues = new Value[outputs.Length];
            for (int i = 0; i < outputs.Length; i++)
            {
                outputValues[i] = new Value(outputs[i], numSignals);
            }
        }

        public Iteration Solve()
        {
            //Setup InputIteration
            Iteration inputIteration = new Iteration();
            for (int j = 0; j < outputValues.Length; j++)
            {
                inputIteration.Add(outputValues[j]);
            }

            //Phase one
            Iteration resultPhaseOne = SolvePhaseOne(inputIteration);

            //Phase two
            Table table = new Table(outputValues, resultPhaseOne.GetValues());
            Iteration finalResult = SolvePhaseTwo(table);

            return finalResult;
        }

        private Iteration SolvePhaseOne(Iteration inputIteration)
        {
            Iteration result = new Iteration();
            while (inputIteration.GetLength() != 0)
            {
                inputIteration.GetNextIteration(out Iteration nextIteration, out Iteration notUsedIteration);
                inputIteration = nextIteration;
                result.Add(notUsedIteration);
            }
            return result;
        }
        private Iteration SolvePhaseTwo(Table inputTable)
        {
            Iteration result = new Iteration();

            while (true)
            {
                bool changed = false;

                //Essentialität
                while (GetEssential(ref inputTable, out Value val))
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
                Console.WriteLine("NOT EMPTY TABLE!!!");
            }
            return result;
        }

        private bool GetEssential(ref Table table, out Value essential)
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
                    table.RemoveEssential(essentialY);
                    return true;
                }
            }
            return false;
        }
    }
}
