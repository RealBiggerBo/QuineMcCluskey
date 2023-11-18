using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QuineMcCluskey
{
    public readonly struct CharValue
    {
        private readonly char[] signals;

        public CharValue(int num, int numSignals)
        {
            signals = new char[numSignals];
            for (int i = 0; i < numSignals; i++)
            {
                signals[i] = num % 2 == 0 ? '0' : '1';
                num = num / 2;
            }
        }
        public CharValue(CharValue a, CharValue b)
        {
            signals = new char[a.signals.Length];
            for (int i = 0; i < a.signals.Length; i++)
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
        public CharValue(string val)
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

        public bool IsSimilar(CharValue other)
        {
            int differences = 0;
            for (int i = 0; i < signals.Length; i++)
            {
                if (signals[i] != other.signals[i])
                {
                    differences++;
                    if (differences > 1) return false;
                }
            }
            return true;
        }

        public bool Implies(CharValue other)
        {
            //a=>b
            for (int i = 0; i < signals.Length; i++)
            {
                if (signals[i] != other.signals[i] && signals[i] != '-' && other.signals[i] != '-')
                    return false;
            }
            return true;
        }

        public override string ToString()
        {
            string res = string.Empty;
            for (int i = signals.Length - 1; i >= 0; i--)
            {
                res += signals[i].ToString();
            }
            return res;
        }

        public override bool Equals(object? obj)
        {
            if (obj?.GetType() == this.GetType())
            {
                CharValue other = (CharValue)obj;
                for (int i = 0; i < this.signals.Length; i++)
                {
                    if (this.signals[i] != other.signals[i]) return false;
                }
                return true;
            }
            return false;
        }
    }

    public readonly struct IntValue
    {
        private readonly int numSignals;
        private readonly int signals;

        public IntValue(int num, int numSignals)
        {
            this.numSignals = numSignals;
            this.signals = 0;
            for (int i = 0; i < numSignals; i++)
            {
                //signals = signals << 2;
                signals |= num % 2 == 0 ? (0b01 << (2 * i)) : (0b10 << (2 * i));
                num /= 2;
            }
        }
        public IntValue(IntValue a, IntValue b)
        {
            this.numSignals = a.numSignals;
            this.signals = a.signals | b.signals;
        }
        public IntValue(string val)
        {
            char[] chars = val.ToCharArray();

            numSignals = chars.Length;
            for (int i = 0; i < chars.Length; i++)
            {
                signals = signals << 2;
                if (chars[i] == '-')
                    signals |= 0b11;
                else if (chars[i] == '0')
                    signals |= 0b01;
                else if (chars[i] == '1')
                    signals |= 0b10;
                else
                    throw new ArgumentException("CONTAINED NOT ALOWED SYMBOL");
            }
        }

        public int GetGroupIndex()
        {
            int groupIndex = 0;
            int value = signals;
            for (int i = 0; i < numSignals; i++)
            {
                if ((value & 0b11) == 0b10)
                    groupIndex++;
                value >>= 2;
            }
            return groupIndex;
        }

        public int GetDontCareCount()
        {
            int count = 0;
            int value = signals;
            for (int i = 0; i < numSignals; i++)
            {
                if ((value & 0b11) == 0b11)
                    count++;
                value >>= 2;
            }
            return count;
        }

        public bool IsSimilar(IntValue other)
        {
            int differences = 0;
            int valueA = this.signals;
            int valueB = other.signals;
            for (int i = 0; i < numSignals; i++)
            {
                if ((valueA & 0b11) != (valueB & 0b11))
                {
                    differences++;
                    if (differences > 1) return false;
                }
                valueA >>= 2;
                valueB >>= 2;
            }
            return true;
        }

        public bool Implies(IntValue other)
        {
            //a=>b
            int impliedValue = ~this.signals | (this.signals & other.signals);
            for (int i = 0; i < numSignals; i++)
            {
                if ((impliedValue & 0b11) != 0b11)
                    return false;
                impliedValue >>= 2;
            }
            return true;
        }

        public override string ToString()
        {
            string res = "";
            int value = this.signals;
            for (int i = 0; i < numSignals; i++)
            {
                if ((value & 0b11) == 0b11)
                    res = "-" + res;
                else if ((value & 0b11) == 0b01)
                    res = "0" + res;
                else if ((value & 0b11) == 0b10)
                    res = "1" + res;
                else
                    throw new ArgumentException("CONTAINED NOT ALOWED SYMBOL");
                value >>= 2;
            }
            return res;
        }

        public override bool Equals(object? obj)
        {
            if (obj?.GetType() == this.GetType())
            {
                return this.signals == ((IntValue)obj).signals;
            }
            return false;
        }
    }
}
