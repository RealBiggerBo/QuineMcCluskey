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
            numSignals = MathF.ILogB(MathF.Max(1, outputs.Max())) + 1;
        }

        public void Solve()
        {
            Console.WriteLine("NumSignals: " + numSignals);
            outputValues = new Value[outputs.Length];
            for (int i = 0; i < outputs.Length; i++)
            {
                outputValues[i] = new Value(outputs[i], numSignals);
                //Console.WriteLine(outputValues[i] + "||-----");
            }

            List<Group> groups = new List<Group>();
            for (int i = 0; i < numSignals + 1; i++)
            {
                groups.Add(new Group(i));
                for (int j = 0; j < outputValues.Length; j++)
                {
                    if (outputValues[j].GetGroup() == i)
                    {
                        groups[i].Add(outputValues[j]);
                    }
                }
            }

            groups = FindSimilar(groups);

            DEBUG(groups);
        }

        void DEBUG(List<Group> groups)
        {
            Console.WriteLine("//////////////////////////////");
            for (int i = 0; i < groups.Count; i++)
            {
                Console.WriteLine("///Group " + i);
                for (int j = 0; j < groups[i].values.Count; j++)
                {
                    Console.WriteLine(groups[i].values[j]);
                }
            }
            Console.WriteLine("//////////////////////////////");
        }

        private List<Group> FindSimilar(List<Group> groups)
        {
            if (groups.Count == 1)
            {
                Console.WriteLine("ONLY one group left:" + groups[0].index);
                return groups;
            }
            List<Group> nextIterationGroups = new List<Group>();
            List<Group> result = new List<Group>();
            for (int i = 0; i < groups.Count - 1; i++)
            {
                nextIterationGroups.Add(CompareGroups(groups[i], groups[i + 1], out List<Group> notUsedValues));
                result = CombineIterations(result, notUsedValues);
            }

            DEBUG(nextIterationGroups);



            return CombineIterations(result, FindSimilar(nextIterationGroups));
        }

        //Returns the not used values and group of combined values
        Group CompareGroups(Group a, Group b, out List<Group> notUsedValues)
        {
            if (Math.Abs(a.index - b.index) != 1)
            {
                throw new Exception("Difference was more than one");
            }
            a.SetUnused();
            b.SetUnused();

            Group combinedValues = new Group(Math.Min(a.index, b.index));
            //notUsedValues = new Group(Math.Min(a.index, b.index));

            for (int i = 0; i < a.values.Count; i++)
            {
                Value valA = a.values[i];
                for (int j = 0; j < b.values.Count; j++)
                {
                    Value valB = b.values[j];
                    if (valA.IsSimilar(valB))
                    {
                        a.Use(valA);
                        b.Use(valB);
                        combinedValues.Add(new Value(valA, valB));
                    }
                }
            }
            notUsedValues = new List<Group>() { a.GetUnused(), b.GetUnused() };
            return combinedValues;
        }

        List<Group> CombineIterations(List<Group> a, List<Group> b)
        {
            List<Group> result = new List<Group>();
            for (int i = 0; i < a.Count; i++)
            {
                result.Add(a[i]);
            }
            for (int i = 0; i < b.Count; i++)
            {
                if (result.Where((group) => group.index == b[i].index).Count() == 0)
                    result.Add(b[i]);
                else
                {
                    for (int j = 0; j < b[i].values.Count; j++)
                    {
                        result.First((group) => group.index == b[i].index).Add(b[i].values[j]);
                    }
                }
            }
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i].values.Count == 0)
                    result.Remove(result[i]);
            }

            return result;
        }
    }
}
