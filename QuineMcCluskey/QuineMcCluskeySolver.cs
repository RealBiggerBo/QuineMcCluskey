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
    internal class QuineMcCluskeySolver
    {
        private int numSignals;
        private int[] outputs;
        private Value[] outputValues;

        public QuineMcCluskeySolver(params int[] outputs)
        {
            this.outputs = outputs;
            numSignals = MathF.ILogB(MathF.Max(1, outputs.Max())) + 1;
            Console.WriteLine("NumSignals: " + numSignals);
            outputValues = new Value[outputs.Length];
            for (int i = 0; i < outputs.Length; i++)
            {
                outputValues[i] = new Value(outputs[i], numSignals);
                //Console.WriteLine(outputValues[i] + "||-----");
            }
        }

        public Iteration Solve()
        {
            //Setup Inputiteration
            Iteration inputIteration = new Iteration();
            for (int j = 0; j < outputValues.Length; j++)
            {
                inputIteration.Add(outputValues[j]);
            }

            Iteration result = new Iteration();
            while (inputIteration.GetLength() != 0)
            {
                inputIteration.GetNextIteration(out Iteration nextIteration, out Iteration notUsedIteration);
                inputIteration = nextIteration;
                result.Add(notUsedIteration);
            }

            return result;
        }

        public bool GetEssential(ref Table table, out Value essential)
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

        public void SolveTable(Iteration results)
        {
            Iteration finalResult = new Iteration();
            Table table = new Table(outputValues, results.GetValues());

            table.DEBUG();

            while (true)
            {
                bool changed = false;

                //Essentialität
                while (GetEssential(ref table, out Value val))
                {
                    changed = true;
                    finalResult.Add(val);
                }

                Console.WriteLine("After Essentialität:");
                table.DEBUG();

                //Zeilendominanz
                while(table.GetRowDominance(ref table))
                {
                    changed = true;
                }

                Console.WriteLine("After Row dominance:");
                table.DEBUG();

                //Reihendominanz
                while (table.GetColumnDominance(ref table))
                {
                    changed = true;
                }

                Console.WriteLine("After Col dominance:");
                table.DEBUG();

                if (!changed || table.GetLength() == 0)
                    break;
            }
            //Verzweigungsmethode

            table.DEBUG();
            finalResult.DEBUG();
        }
    }
}
