using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuineMcCluskey
{
    public struct Iteration
    {
        List<Group> groups;

        public Iteration()
        {
            this.groups = new List<Group>();
        }
        public Iteration(params Value[] values)
        {
            this.groups = new List<Group>();
            for (int i = 0; i < values.Length; i++)
            {
                Add(values[i]);
            }
        }
        public Iteration(params Group[] groups)
        {
            this.groups = new List<Group>();
            for (int i = 0; i < groups.Length; i++)
            {
                Add(groups[i]);
            }
        }

        public void Add(Value value)
        {
            Group? matchingGroup = GetGroupByIndex(value.GetGroupIndex());
            if (matchingGroup == null)
            {
                groups.Add(new Group(value));
            }
            else
            {
                matchingGroup?.Add(value);
            }
        }
        public void Add(Group group)
        {
            for (int i = 0; i < group.GetLength(); i++) 
            {
                Add(group.Get(i));
            }
        }
        public void Add(Iteration iteration)
        {
            for(int i = 0; i<iteration.groups.Count; i++)
            {
                Add(iteration.groups[i]);
            }
        }

        public bool Remove(Value value) 
        {
            for(int i = 0; i < groups.Count; i++)
            {
                if(groups[i].Remove(value))
                    return true;
            }
            return false;
        }

        public Group? GetGroupByIndex(int index)
        {
            if(groups.Where((g) => g.index == index).Count() == 0)
                return null;
            else
                return groups.First((g)=> g.index == index);
        }

        public int GetLength()
        {
            return groups.Count;
        }
        public Value[] GetValues()
        {
            List<Value> values = new List<Value>();
            for(int i = 0;i < groups.Count;i++)
            {
                values.AddRange(groups[i].GetValues());
            }

            return values.ToArray();
        }

        public Iteration GetUnused()
        {
            Iteration unusedIteration = new Iteration();
            for (int i = 0; i < groups.Count; i++)
            {
                unusedIteration.Add(groups[i].GetUnused());
            }
            return unusedIteration;
        }

        public void GetNextIteration(out Iteration nextIteration, out Iteration notUsedIteration)
        {
            nextIteration = new Iteration();
            for (int i = 0; i < groups.Count; i++)
            {
                Group groupA = groups[i];
                Group? groupB = GetGroupByIndex(groupA.index + 1);
                if(groupB == null)
                {
                    
                }
                else
                {
                    for(int j = 0; j < groupA.GetLength(); j++)
                    {
                        Value valA = groupA.Get(j);
                        for(int k = 0; k < groupB?.GetLength(); k++)
                        {
                            Value valB = ((Group)groupB).Get(k);
                            if (valA.IsSimilar(valB))
                            {
                                nextIteration.Add(new Value(valA, valB));
                                groupA.Use(valA);
                                ((Group)groupB).Use(valB);
                                groupB.Equals(valB);
                            }
                        }
                    }
                }
            }
            notUsedIteration = GetUnused();
        }

        public void DEBUG()
        {
            Console.WriteLine("//////////////////////////////");
            for (int i = 0; i < groups.Count; i++)
            {
                groups[i].DEBUG();
            }
            if(groups.Count == 0)
                Console.WriteLine("EMPTY ITERATION");
            Console.WriteLine("//////////////////////////////");
        }
    }
}
