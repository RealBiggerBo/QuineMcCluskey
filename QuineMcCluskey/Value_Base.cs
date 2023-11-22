using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QuineMcCluskey
{
#pragma warning disable
    public readonly struct Value_Base
    {
        private readonly char[] signals;

        public Value_Base(int num, int numSignals)
        {
            signals = new char[numSignals];
            for (int i = 0; i < numSignals; i++)
            {
                signals[i] = num % 2 == 0 ? '0' : '1';
                num = num / 2;
            }
        }
        public Value_Base(Value_Base a, Value_Base b)
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
        public Value_Base(string val)
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

        public bool IsSimilar(Value_Base other)
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

        public bool Implies(Value_Base other)
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
                Value_Base other = (Value_Base)obj;
                for (int i = 0; i < this.signals.Length; i++)
                {
                    if (this.signals[i] != other.signals[i]) return false;
                }
                return true;
            }
            return false;
        }
    }
#pragma warning restore
    public readonly struct Value_Optimised : IEquatable<Value_Optimised>
    {
        private readonly int numSignals;
        private readonly int signals;

        public Value_Optimised(int num, int numSignals)
        {
            this.numSignals = numSignals;
            this.signals = 0;
            for (int i = 0; i < numSignals; i++)
            {
                signals |= num % 2 == 0 ? (0b01 << (2 * i)) : (0b10 << (2 * i));
                num /= 2;
            }
            signals <<=1;
        }
        public Value_Optimised(Value_Optimised baseValue, bool used)
        {
            this.numSignals = baseValue.numSignals;
            if (used)
                this.signals = baseValue.signals | 0b1;
            else
                this.signals = baseValue.signals & ~0b1;
        }
        public Value_Optimised(Value_Optimised a, Value_Optimised b)
        {
            this.numSignals = a.numSignals;
            this.signals = (a.signals | b.signals) & ~0b1;
        }
        public Value_Optimised(string val)
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

            signals <<= 1;
        }

        public int GetGroupIndex()
        {
            int groupIndex = 0;
            int value = signals >> 1;
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
            int value = signals >> 1;
            for (int i = 0; i < numSignals; i++)
            {
                if ((value & 0b11) == 0b11)
                    count++;
                value >>= 2;
            }
            return count;
        }

        public bool WasUsed()
        {
            return (signals % 2) == 1;
        }

        public bool IsSimilar(Value_Optimised other)
        {
            int differences = 0;
            int valueA = this.signals >> 1;
            int valueB = other.signals >> 1;
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

        public bool Implies(Value_Optimised other)
        {
            //a=>b
            int impliedValue = (~this.signals | (this.signals & other.signals)) >> 1;
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
            int value = this.signals >> 1;
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
                return (this.signals >> 1) == (((Value_Optimised)obj).signals >> 1);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return signals.GetHashCode();
        }

        public bool Equals(Value_Optimised other)
        {
            return (this.signals >> 1) == (other.signals >> 1);
        }

        public static bool operator ==(Value_Optimised left, Value_Optimised right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Value_Optimised left, Value_Optimised right)
        {
            return !(left == right);
        }
    }
}
