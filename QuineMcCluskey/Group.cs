using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuineMcCluskey
{
    public struct Group
    {
        public readonly int index;
        private readonly List<Value> values;
        private readonly List<bool> used;

        public Group(int index)
        {
            this.index = index;
            this.values = new List<Value>();
            this.used = new List<bool>();
        }
        public Group(Value value)
        {
            this.index = value.GetGroupIndex();
            this.values = new List<Value>();
            this.used = new List<bool>();
            Add(value);
        }
        public Group(params Value[] values)
        {
            this.index = values[0].GetGroupIndex();
            this.values = new List<Value>();
            this.used = new List<bool>();
            for (int i = 0; i < values.Length; i++)
            {
                Add(values[i]);
            }
        }

        public void Add(Value value)
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
        public bool Remove(Value value)
        {
            return values.Remove(value);
        }
        public Value Get(int index)
        {
            return values[index];
        }
        public Value[] GetValues()
        {
            List<Value> result = new List<Value>();
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

        public void Use(Value val)
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

        public Group GetUnused()
        {
            Group unused = new Group(index);
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
}
