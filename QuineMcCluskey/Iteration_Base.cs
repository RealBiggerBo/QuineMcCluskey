using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuineMcCluskey
{
#pragma warning disable
    public struct Iteration_Base
    {
        List<Group_Base> groups;

        public Iteration_Base()
        {
            this.groups = new List<Group_Base>();
        }
        public Iteration_Base(params Value_Base[] values)
        {
            this.groups = new List<Group_Base>();
            for (int i = 0; i < values.Length; i++)
            {
                Add(values[i]);
            }
        }
        public Iteration_Base(params Group_Base[] groups)
        {
            this.groups = new List<Group_Base>();
            for (int i = 0; i < groups.Length; i++)
            {
                Add(groups[i]);
            }
        }

        public void Add(Value_Base value)
        {
            Group_Base? matchingGroup = GetGroupByIndex(value.GetGroupIndex());
            if (matchingGroup == null)
            {
                groups.Add(new Group_Base(value));
            }
            else
            {
                matchingGroup?.Add(value);
            }
        }
        public void Add(Group_Base group)
        {
            for (int i = 0; i < group.GetLength(); i++) 
            {
                Add(group.Get(i));
            }
        }
        public void Add(Iteration_Base iteration)
        {
            for(int i = 0; i<iteration.groups.Count; i++)
            {
                Add(iteration.groups[i]);
            }
        }

        public bool Remove(Value_Base value) 
        {
            for(int i = 0; i < groups.Count; i++)
            {
                if(groups[i].Remove(value))
                    return true;
            }
            return false;
        }

        public Group_Base? GetGroupByIndex(int index)
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
        public Value_Base[] GetValues()
        {
            List<Value_Base> values = new List<Value_Base>();
            for(int i = 0;i < groups.Count;i++)
            {
                values.AddRange(groups[i].GetValues());
            }

            return values.ToArray();
        }

        public int GetDontCareCount()
        {
            int count = 0;
            for (int i = 0; i < groups.Count; i++)
            {
                count += groups[i].GetDontCareCount();
            }
            return count;
        }

        public Iteration_Base GetUnused()
        {
            Iteration_Base unusedIteration = new Iteration_Base();
            for (int i = 0; i < groups.Count; i++)
            {
                unusedIteration.Add(groups[i].GetUnused());
            }
            return unusedIteration;
        }

        public void GetNextIteration(out Iteration_Base nextIteration, out Iteration_Base notUsedIteration)
        {
            nextIteration = new Iteration_Base();
            for (int i = 0; i < groups.Count; i++)
            {
                Group_Base groupA = groups[i];
                Group_Base? groupB = GetGroupByIndex(groupA.index + 1);
                if(groupB == null)
                {
                    
                }
                else
                {
                    for(int j = 0; j < groupA.GetLength(); j++)
                    {
                        Value_Base valA = groupA.Get(j);
                        for(int k = 0; k < groupB?.GetLength(); k++)
                        {
                            Value_Base valB = ((Group_Base)groupB).Get(k);
                            if (valA.IsSimilar(valB))
                            {
                                nextIteration.Add(new Value_Base(valA, valB));
                                groupA.Use(valA);
                                ((Group_Base)groupB).Use(valB);
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
#pragma warning restore
    public readonly struct Iteration_Optimised
    {
        private readonly List<Group_Optimised> groups;

        public Iteration_Optimised()
        {
            this.groups = new List<Group_Optimised>();
        }
        public Iteration_Optimised(params Value_Optimised[] values)
        {
            this.groups = new List<Group_Optimised>();
            for (int i = 0; i < values.Length; i++)
            {
                Add(values[i]);
            }
        }
        public Iteration_Optimised(params Group_Optimised[] groups)
        {
            this.groups = new List<Group_Optimised>();
            for (int i = 0; i < groups.Length; i++)
            {
                Add(groups[i]);
            }
        }

        public void Add(Value_Optimised value)
        {
            if (!GetGroupByIndex(value.GetGroupIndex(), out Group_Optimised matchingGroup))
            {
                groups.Add(new Group_Optimised(value));
            }
            else
            {
                matchingGroup.Add(value);
            }
        }
        public void Add(Group_Optimised group)
        {
            for (int i = 0; i < group.GetLength(); i++)
            {
                Add(group.Get(i));
            }
        }
        public void Add(Iteration_Optimised iteration)
        {
            for (int i = 0; i < iteration.groups.Count; i++)
            {
                Add(iteration.groups[i]);
            }
        }

        public bool Remove(Value_Optimised value)
        {
            for (int i = 0; i < groups.Count; i++)
            {
                if (groups[i].Remove(value))
                    return true;
            }
            return false;
        }

        public bool GetGroupByIndex(int index,out Group_Optimised group)
        {
            if (groups.Where((g) => g.index == index).Count() == 0)
            {
                group = default;
                return false;
            }
            else
            {
                group = groups.First((g) => g.index == index);
                return true;
            }
        }

        public int GetLength()
        {
            return groups.Count;
        }
        public List<Value_Optimised> GetValues()
        {
            List<Value_Optimised> values = new List<Value_Optimised>();
            for (int i = 0; i < groups.Count; i++)
            {
                values.AddRange(groups[i].GetAllValues());
            }

            return values;
        }

        public int GetDontCareCount()
        {
            int count = 0;
            for (int i = 0; i < groups.Count; i++)
            {
                count += groups[i].GetDontCareCount();
            }
            return count;
        }

        public Iteration_Optimised GetUnused()
        {
            Iteration_Optimised unusedIteration = new Iteration_Optimised();
            for (int i = 0; i < groups.Count; i++)
            {
                unusedIteration.Add(groups[i].GetUnused());
            }
            return unusedIteration;
        }

        public void GetNextIteration(out Iteration_Optimised nextIteration, out Iteration_Optimised notUsedIteration)
        {
            nextIteration = new Iteration_Optimised();

            for (int i = 0; i < groups.Count; i++)
            {
                Group_Optimised groupA = groups[i];
                if (GetGroupByIndex(groupA.index + 1, out Group_Optimised groupB))
                {
                    for (int j = 0; j < groupA.GetLength(); j++)
                    {
                        Value_Optimised valA = groupA.Get(j);
                        for (int k = 0; k < groupB.GetLength(); k++)
                        {
                            Value_Optimised valB = ((Group_Optimised)groupB).Get(k);
                            if (valA.IsSimilar(valB))
                            {
                                nextIteration.Add(new Value_Optimised(valA, valB));
                                groupA.Use(valA);
                                (groupB).Use(valB);
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
            if (groups.Count == 0)
                Console.WriteLine("EMPTY ITERATION");
            Console.WriteLine("//////////////////////////////");
        }
    }
}
