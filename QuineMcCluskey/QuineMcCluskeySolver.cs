using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public QuineMcCluskeySolver(int[] outputs) 
        {
            this.outputs = outputs;
            numSignals = MathF.ILogB(MathF.Max(1,outputs.Max()))+1;
        }

        public void Solve()
        {
            Console.WriteLine("NumSignals: " + numSignals);
            outputValues = new Value[outputs.Length];
            for (int i = 0; i < outputs.Length; i++)
            {
                outputValues[i] = new Value(outputs[i], numSignals);
                Console.WriteLine(outputValues[i] + "||-----");
            }

            List<Value>[] groups = new List<Value>[numSignals+1];
            for (int i = 0; i < groups.Length; i++)
            {
                groups[i] = new List<Value>();
                for (int j = 0; j < outputValues.Length; j++)
                {
                    if (outputValues[j].GetGroup() == i)
                    {
                        groups[i].Add(outputValues[j]);
                    }
                }
            }

            groups = FindSimilar(FindSimilar(groups));
            for (int i = 0; i < groups.Length; i++)
            {
                Console.WriteLine("///Group " + i);
                for (int j = 0; j < groups[i].Count; j++)
                {
                    Console.WriteLine(groups[i][j]);
                }
            }
        }

        private List<Value>[] FindSimilar(List<Value>[] groups)
        {
            if (groups.Length == 0)
                return groups;
            List<Value>[] groups2 = new List<Value>[groups.Length -1];
            for (int i = 0; i < groups.Length - 1; i++)
            {

                groups2[i] = new List<Value>();
                for (int j = 0; j < groups[i].Count; j++)
                {
                    bool used = false;
                    for (int k = 0; k < groups[i + 1].Count; k++)
                    {
                        Console.WriteLine("Compare " + groups[i][j] + " to " + groups[i + 1][k] + " : " + groups[i][j].IsSimilar(groups[i + 1][k]));
                        if (groups[i][j].IsSimilar(groups[i + 1][k]))
                        {
                            used = true;
                            Value newValue = new Value(groups[i][j], groups[i + 1][k]);
                            if (!groups2[i].Contains(newValue))
                                groups2[i].Add(newValue);
                        }
                    }

                    //if (!used)
                        //groups2[i].Add(groups[i][j]);
                }
            }
            return groups2;
        }
    }
}
