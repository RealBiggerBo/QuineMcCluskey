using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuineMcCluskey
{
    public struct Value
    {
        private char[] signals;

        public Value(int num, int numSignals)
        {
            signals = new char[numSignals];
            for(int i = 0; i < numSignals; i++)
            {
                signals[i] = num % 2 == 0 ? '0' : '1';
                num = num / 2;
            }
        }
        public Value(Value a, Value b)
        {
            signals = new char[a.signals.Length];
            for(int i = 0;i < a.signals.Length; i++)
            {
                if (a.signals[i] != b.signals[i])
                {
                    signals[i] = '-';
                }
                else
                {
                    signals[i] = a.signals[i];
                }
            }
            //Console.WriteLine(a + " and " + b + " becomes: " + this);
        }
        public Value(string val)
        {
            signals = val.ToCharArray();
            Array.Reverse(signals);
        }

        public int GetGroupIndex()
        {
            int groupIndex = 0;
            for (int i = 0; i < signals.Length; i++)
            {
                if (signals[i] == '1') groupIndex++;
            }
            return groupIndex;
        }

        public int GetDontCareCount() 
        {
            int count = 0;
            for (int i = 0; i < signals.Length; i++)
            {
                if (signals[i] == '-') count++;
            }
            return count;
        }

        public bool IsSimilar(Value other)
        {
            int differences = 0;
            for (int i = 0; i < signals.Length; i++)
            {
                if (signals[i] != other.signals[i])
                {
                    differences++;
                    if(differences > 1) return false;
                }
            }
            return true;
        }

        public bool Implies(Value other)
        {
            //a=>b
            for (int i = 0;i< signals.Length; i++)
            {
                if (signals[i] != other.signals[i] && signals[i] != '-' && other.signals[i]!='-')
                    return false;
            }
            return true;
        }

        public override string ToString()
        {
            string res = string.Empty;
            for(int i = signals.Length-1;i>=0;i--)
            {
                res+= signals[i].ToString();
            }
            return res;
        }

        public override bool Equals(object? obj)
        {
            if (obj?.GetType() == this.GetType())
            {
                Value other = (Value)obj;
                for (int i = 0; i < this.signals.Length; i++)
                {
                    if (this.signals[i] != other.signals[i]) return false;
                }
                return true;
            }
            return false;
        }
    }
}
