using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuineMcCluskey
{
    public struct CharGroup
    {
        public readonly int index;
        private readonly List<CharValue> values;
        private readonly List<bool> used;

        public CharGroup(int index)
        {
            this.index = index;
            this.values = new List<CharValue>();
            this.used = new List<bool>();
        }
        public CharGroup(CharValue value)
        {
            this.index = value.GetGroupIndex();
            this.values = new List<CharValue>();
            this.used = new List<bool>();
            Add(value);
        }
        public CharGroup(params CharValue[] values)
        {
            this.index = values[0].GetGroupIndex();
            this.values = new List<CharValue>();
            this.used = new List<bool>();
            for (int i = 0; i < values.Length; i++)
            {
                Add(values[i]);
            }
        }

        public void Add(CharValue value)
        {
            if(value.GetGroupIndex() != this.index)
            {
                throw new Exception("Tried to add value with different index");
            }
            if(!values.Contains(value)) 
            {
                values.Add(value);
                used.Add(false);
            }
        }
        public bool Remove(CharValue value)
        {
            return values.Remove(value);
        }
        public CharValue Get(int index)
        {
            return values[index];
        }
        public CharValue[] GetValues()
        {
            List<CharValue> result = new List<CharValue>();
            for (int i = 0; i < values.Count; i++)
            {
                result.Add(Get(i));
            }

            return result.ToArray();
        }
        public int GetLength()
        {
            return values.Count;
        }

        public int GetDontCareCount()
        {
            int count = 0;
            for (int i = 0; i < values.Count; i++)
            {
                count += values[i].GetDontCareCount();
            }
            return count;
        }

        public void Use(CharValue val)
        {
            int index = values.IndexOf(val);
            if(index != -1)
            {
                used[index]=true;
            }
        }

        public void SetUnused()
        {
            for (int i = 0; i < values.Count; i++)
            {
                used[i] = false;
            }
        }

        public CharGroup GetUnused()
        {
            CharGroup unused = new CharGroup(index);
            for(int i = 0; i < values.Count; i++)
            {
                if (!used[i])
                {
                    unused.Add(values[i]);
                }
            }
            return unused;
        }

        public void DEBUG()
        {
            Console.WriteLine("///Group " + this.index);
            for (int i = 0; i < values.Count; i++)
            {
                Console.WriteLine(values[i]);
            }
        }
    }
    public struct IntGroup
    {
        public readonly int index;
        private readonly List<IntValue> values;
        private readonly List<bool> used;

        public IntGroup(int index)
        {
            this.index = index;
            this.values = new List<IntValue>();
            this.used = new List<bool>();
        }
        public IntGroup(IntValue value)
        {
            this.index = value.GetGroupIndex();
            this.values = new List<IntValue>();
            this.used = new List<bool>();
            Add(value);
        }
        public IntGroup(params IntValue[] values)
        {
            this.index = values[0].GetGroupIndex();
            this.values = new List<IntValue>();
            this.used = new List<bool>();
            for (int i = 0; i < values.Length; i++)
            {
                Add(values[i]);
            }
        }

        public void Add(IntValue value)
        {
            if (value.GetGroupIndex() != this.index)
            {
                throw new Exception("Tried to add value with different index");
            }
            if (!values.Contains(value))
            {
                values.Add(value);
                used.Add(false);
            }
        }
        public bool Remove(IntValue value)
        {
            return values.Remove(value);
        }
        public IntValue Get(int index)
        {
            return values[index];
        }
        public IntValue[] GetValues()
        {
            List<IntValue> result = new List<IntValue>();
            for (int i = 0; i < values.Count; i++)
            {
                result.Add(Get(i));
            }

            return result.ToArray();
        }
        public int GetLength()
        {
            return values.Count;
        }

        public int GetDontCareCount()
        {
            int count = 0;
            for (int i = 0; i < values.Count; i++)
            {
                count += values[i].GetDontCareCount();
            }
            return count;
        }

        public void Use(IntValue val)
        {
            int index = values.IndexOf(val);
            if (index != -1)
            {
                used[index] = true;
            }
        }

        public void SetUnused()
        {
            for (int i = 0; i < values.Count; i++)
            {
                used[i] = false;
            }
        }

        public IntGroup GetUnused()
        {
            IntGroup unused = new IntGroup(index);
            for (int i = 0; i < values.Count; i++)
            {
                if (!used[i])
                {
                    unused.Add(values[i]);
                }
            }
            return unused;
        }

        public void DEBUG()
        {
            Console.WriteLine("///Group " + this.index);
            for (int i = 0; i < values.Count; i++)
            {
                Console.WriteLine(values[i]);
            }
        }
    }
}
