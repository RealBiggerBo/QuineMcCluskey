using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuineMcCluskey
{
#pragma warning disable
    public struct Group_Base
    {
        public readonly int index;
        private readonly List<Value_Base> values;
        private readonly List<bool> used;

        public Group_Base(int index)
        {
            this.index = index;
            this.values = new List<Value_Base>();
            this.used = new List<bool>();
        }
        public Group_Base(Value_Base value)
        {
            this.index = value.GetGroupIndex();
            this.values = new List<Value_Base>();
            this.used = new List<bool>();
            Add(value);
        }
        public Group_Base(params Value_Base[] values)
        {
            this.index = values[0].GetGroupIndex();
            this.values = new List<Value_Base>();
            this.used = new List<bool>();
            for (int i = 0; i < values.Length; i++)
            {
                Add(values[i]);
            }
        }

        public void Add(Value_Base value)
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
        public bool Remove(Value_Base value)
        {
            return values.Remove(value);
        }
        public Value_Base Get(int index)
        {
            return values[index];
        }
        public Value_Base[] GetValues()
        {
            List<Value_Base> result = new List<Value_Base>();
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

        public void Use(Value_Base val)
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

        public Group_Base GetUnused()
        {
            Group_Base unused = new Group_Base(index);
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
#pragma warning restore
    public readonly struct Group_Optimised
    {
        public readonly int index;
        private readonly List<Value_Optimised> values;

        public Group_Optimised(int index)
        {
            this.index = index;
            this.values = new List<Value_Optimised>();
        }
        public Group_Optimised(Value_Optimised value)
        {
            this.index = value.GetGroupIndex();
            this.values = new List<Value_Optimised>();
            Add(value);
        }
        public Group_Optimised(params Value_Optimised[] values)
        {
            this.index = values[0].GetGroupIndex();
            this.values = new List<Value_Optimised>();
            for (int i = 0; i < values.Length; i++)
            {
                Add(values[i]);
            }
        }

        public void Add(Value_Optimised value)
        {
            if (value.GetGroupIndex() != this.index)
            {
                throw new Exception("Tried to add value with different index");
            }
            if (!values.Contains(value))
            {
                values.Add(value);
            }
        }
        public bool Remove(Value_Optimised value)
        {
            return values.Remove(value);
        }
        public Value_Optimised Get(int index)
        {
            return values[index];
        }
        public List<Value_Optimised> GetAllValues()
        {
            return new List<Value_Optimised>(values);
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

        public void Use(Value_Optimised val)
        {
            int idx = values.IndexOf(val);
            if (idx != -1)
            {
                values[idx] = new Value_Optimised(values[idx], true);
            }
        }

        public void SetUnused()
        {
            for (int i = 0; i < values.Count; i++)
            {
                values[i] = new Value_Optimised(values[i], false);
            }
        }

        public Group_Optimised GetUnused()
        {
            Group_Optimised unused = new Group_Optimised(index);
            for (int i = 0; i < values.Count; i++)
            {
                if (!values[i].WasUsed())
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
