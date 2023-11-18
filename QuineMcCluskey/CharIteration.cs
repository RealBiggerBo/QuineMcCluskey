using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuineMcCluskey
{
    public struct CharIteration
    {
        List<CharGroup> groups;

        public CharIteration()
        {
            this.groups = new List<CharGroup>();
        }
        public CharIteration(params CharValue[] values)
        {
            this.groups = new List<CharGroup>();
            for (int i = 0; i < values.Length; i++)
            {
                Add(values[i]);
            }
        }
        public CharIteration(params CharGroup[] groups)
        {
            this.groups = new List<CharGroup>();
            for (int i = 0; i < groups.Length; i++)
            {
                Add(groups[i]);
            }
        }

        public void Add(CharValue value)
        {
            CharGroup? matchingGroup = GetGroupByIndex(value.GetGroupIndex());
            if (matchingGroup == null)
            {
                groups.Add(new CharGroup(value));
            }
            else
            {
                matchingGroup?.Add(value);
            }
        }
        public void Add(CharGroup group)
        {
            for (int i = 0; i < group.GetLength(); i++) 
            {
                Add(group.Get(i));
            }
        }
        public void Add(CharIteration iteration)
        {
            for(int i = 0; i<iteration.groups.Count; i++)
            {
                Add(iteration.groups[i]);
            }
        }

        public bool Remove(CharValue value) 
        {
            for(int i = 0; i < groups.Count; i++)
            {
                if(groups[i].Remove(value))
                    return true;
            }
            return false;
        }

        public CharGroup? GetGroupByIndex(int index)
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
        public CharValue[] GetValues()
        {
            List<CharValue> values = new List<CharValue>();
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

        public CharIteration GetUnused()
        {
            CharIteration unusedIteration = new CharIteration();
            for (int i = 0; i < groups.Count; i++)
            {
                unusedIteration.Add(groups[i].GetUnused());
            }
            return unusedIteration;
        }

        public void GetNextIteration(out CharIteration nextIteration, out CharIteration notUsedIteration)
        {
            nextIteration = new CharIteration();
            for (int i = 0; i < groups.Count; i++)
            {
                CharGroup groupA = groups[i];
                CharGroup? groupB = GetGroupByIndex(groupA.index + 1);
                if(groupB == null)
                {
                    
                }
                else
                {
                    for(int j = 0; j < groupA.GetLength(); j++)
                    {
                        CharValue valA = groupA.Get(j);
                        for(int k = 0; k < groupB?.GetLength(); k++)
                        {
                            CharValue valB = ((CharGroup)groupB).Get(k);
                            if (valA.IsSimilar(valB))
                            {
                                nextIteration.Add(new CharValue(valA, valB));
                                groupA.Use(valA);
                                ((CharGroup)groupB).Use(valB);
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
    public struct IntIteration
    {
        readonly List<IntGroup> groups;

        public IntIteration()
        {
            this.groups = new List<IntGroup>();
        }
        public IntIteration(params IntValue[] values)
        {
            this.groups = new List<IntGroup>();
            for (int i = 0; i < values.Length; i++)
            {
                Add(values[i]);
            }
        }
        public IntIteration(params IntGroup[] groups)
        {
            this.groups = new List<IntGroup>();
            for (int i = 0; i < groups.Length; i++)
            {
                Add(groups[i]);
            }
        }

        public void Add(IntValue value)
        {
            IntGroup? matchingGroup = GetGroupByIndex(value.GetGroupIndex());
            if (matchingGroup == null)
            {
                groups.Add(new IntGroup(value));
            }
            else
            {
                matchingGroup?.Add(value);
            }
        }
        public void Add(IntGroup group)
        {
            for (int i = 0; i < group.GetLength(); i++)
            {
                Add(group.Get(i));
            }
        }
        public void Add(IntIteration iteration)
        {
            for (int i = 0; i < iteration.groups.Count; i++)
            {
                Add(iteration.groups[i]);
            }
        }

        public bool Remove(IntValue value)
        {
            for (int i = 0; i < groups.Count; i++)
            {
                if (groups[i].Remove(value))
                    return true;
            }
            return false;
        }

        public IntGroup? GetGroupByIndex(int index)
        {
            if (groups.Where((g) => g.index == index).Count() == 0)
                return null;
            else
                return groups.First((g) => g.index == index);
        }

        public int GetLength()
        {
            return groups.Count;
        }
        public IntValue[] GetValues()
        {
            List<IntValue> values = new List<IntValue>();
            for (int i = 0; i < groups.Count; i++)
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

        public IntIteration GetUnused()
        {
            IntIteration unusedIteration = new IntIteration();
            for (int i = 0; i < groups.Count; i++)
            {
                unusedIteration.Add(groups[i].GetUnused());
            }
            return unusedIteration;
        }

        public void GetNextIteration(out IntIteration nextIteration, out IntIteration notUsedIteration)
        {
            nextIteration = new IntIteration();
            for (int i = 0; i < groups.Count; i++)
            {
                IntGroup groupA = groups[i];
                IntGroup? groupB = GetGroupByIndex(groupA.index + 1);
                if (groupB == null)
                {

                }
                else
                {
                    for (int j = 0; j < groupA.GetLength(); j++)
                    {
                        IntValue valA = groupA.Get(j);
                        for (int k = 0; k < groupB?.GetLength(); k++)
                        {
                            IntValue valB = ((IntGroup)groupB).Get(k);
                            if (valA.IsSimilar(valB))
                            {
                                nextIteration.Add(new IntValue(valA, valB));
                                groupA.Use(valA);
                                ((IntGroup)groupB).Use(valB);
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
