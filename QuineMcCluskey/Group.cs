using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuineMcCluskey
{
    internal class Group
    {
        public int index;
        public List<Value> values;
        private List<bool> used;

        public Group(int index)
        {
            this.index = index;
            this.values = new List<Value>();
            this.used = new List<bool>();
        }
        public Group(int index, Value value)
        {
            this.index = index;
            this.values = new List<Value>();
            this.used = new List<bool>();
            Add(value);
        }

        public void Add(Value value)
        {
            if(!values.Contains(value)) 
            {
                values.Add(value);
                used.Add(false);
            }
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
    }
}
