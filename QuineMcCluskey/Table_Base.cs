using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuineMcCluskey
{
#pragma warning disable
    public struct Table_Base
    {
        private List<int> data;
        private List<Value_Base> implicants;
        public int width;
        public int height;

        public Table_Base(Value_Base[] outputs, Value_Base[] implicants)
        {
            data = new List<int>();
            this.implicants = implicants.ToList();
            width = outputs.Length;
            height = implicants.Length;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (outputs[j].Implies(implicants[i]))
                        data.Add(1);
                    else
                        data.Add(0);
                }
            }
        }
        public Table_Base(Table_Base original, int excludeRow)
        {
            data = new List<int>(original.data);
            implicants = new List<Value_Base>(original.implicants);
            this.width = original.width;
            this.height = original.height;
            RemovePrimeImplicant(excludeRow);
        }

        public void Set(int x,int y,int value)
        {
            data[y * width + x] = value;
        }
        public int Get(int x,int y)
        {
            return data[y * width + x];
        }
        public Value_Base GetImplicant(int y)
        {
            return implicants[y];
        }

        public void RemovePrimeImplicant(int _y)
        {
            int[] row = data.GetRange(_y * width, width).ToArray();
            int removedColls = 0;
            for(int y = height-1; y >=0; y--)
            {
                for (int x = width - 1; x >= 0; x--)
                {
                    if (y == _y)
                    {
                        if (row[x] == 1)
                            removedColls++;
                        data.RemoveAt(y * width + x);
                    }
                    else if (row[x] == 1)
                        data.RemoveAt(y * width + x);
                }
            }

            width -= removedColls;
            height--;

            implicants.RemoveAt(_y);
        }

        public bool GetRowDominance(ref Table_Base table)
        {
            for(int y = 0; y < height; y++)
            {
                List<int> row1 = data.GetRange(y * width, width);
                for (int y2 = 0; y2 < height; y2++)
                {
                    if (y == y2)
                        continue;
                    List<int> row2 = data.GetRange(y2 * width, width);
                    if (Dominates(row1, row2))
                    {
                        RemoveRow(y2);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool GetColumnDominance(ref Table_Base table)
        {
            for (int x = 0;x < width; x++)
            {
                List<int> col1 = GetColumn(x);
                for (int x2 = 0; x2 < width; x2++)
                {
                    if (x == x2) continue;
                    List<int> col2 = GetColumn(x2);
                    if(Dominates(col1, col2))
                    {
                        RemoveCol(x);
                        return true;
                    }
                }
            }
            return false;
        }

        private List<int> GetColumn(int x)
        {
            List<int> res = new List<int>();
            for (int index = 0; index < height; index++)
            {
                res.Add(data[x + index*width]);
            }
            return res;
        }

        public int GetLength()
        {
            return data.Count;
        }

        private void RemoveRow(int rowIndex)
        {
            for (int index = (rowIndex+1)*width -1; index >= rowIndex*width; index--)
            {
                data.RemoveAt(index);
            }
            implicants.RemoveAt(rowIndex);
            height--;
        }

        private void RemoveCol(int colIndex) 
        {
            int startIndex = (height - 1) * width + colIndex;
            for (int index = 0; index < height; index++)
            {
                data.RemoveAt(startIndex - index*width);
            }
            width--;
        }

        private bool Dominates(List<int> row1, List<int> row2)
        {
            for (int i = 0; i < row1.Count; i++)
            {
                if (row2[i] == 1 && row1[i] != 1)
                    return false;
            }
            return true;
        }

        public void DEBUG()
        {
            Console.WriteLine("///TABLE");
            for (int y = 0; y < height; y++)
            {
                Console.Write(implicants[y] + "\t");
                for (int x = 0; x < width; x++)
                {
                    Console.Write(Get(x,y) + " ");
                }
                Console.WriteLine();
            }
            if(data.Count == 0) 
                Console.WriteLine("EMPTY");
        }
    }
#pragma warning restore
    public struct Table_Optimised
    {
        private readonly BitArray data;
        private readonly List<Value_Optimised> implicants;
        public int width;
        public int height;

        public Table_Optimised(Value_Optimised[] outputs, List<Value_Optimised> implicants)
        {
            //data = new List<int>();
            data = new BitArray(outputs.Length * implicants.Count);
            this.implicants = new List<Value_Optimised>(implicants);
            width = outputs.Length;
            height = implicants.Count;

            //for (int i = 0; i < height; i++)
            //{
            //    for (int j = 0; j < width; j++)
            //    {
            //        if (outputs[j].Implies(implicants[i]))
            //            data.Add(1);
            //        else
            //            data.Add(0);
            //    }
            //}
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Set(x, y, outputs[x].Implies(implicants[y]));
                }
            }
        }
        public Table_Optimised(Table_Optimised original, int excludeRow)
        {
            //data = new List<int>(original.data);
            data = (BitArray)original.data.Clone();
            implicants = new List<Value_Optimised>(original.implicants);
            this.width = original.width;
            this.height = original.height;
            RemovePrimeImplicant(excludeRow);
        }

        public readonly void Set(int x, int y, bool value)
        {
            data.Set(y * width + x, value);
        }
        public readonly bool Get(int x,int y)
        {
            return data.Get(y* width + x);
        }
        public readonly Value_Optimised GetImplicant(int y)
        {
            return implicants[y];
        }

        public void RemovePrimeImplicant(int _y)
        {
            for (int x = width - 1; x >= 0; x--)
            {
                if (Get(x,_y) == true)
                {
                    RemoveCol(x);
                }
            }
            RemoveRow(_y);
        }

        public bool RemoveDominatedRow()
        {
            for (int y = 0; y < height; y++)
            {
                for (int y2 = 0; y2 < height; y2++)
                {
                    if (y == y2)
                        continue;
                    if (CheckRowDomination(y, y2))
                    {
                        RemoveRow(y2);
                        return true;
                    }
                }
            }
            return false;
        }
        public bool RemoveDominatedCol()
        {
            for (int x = 0; x < width; x++)
            {
                for (int x2 = 0; x2 < width; x2++)
                {
                    if (x == x2) continue;
                    if (CheckColDomination(x, x2))
                    {
                        RemoveCol(x);
                        return true;
                    }
                }
            }
            return false;
        }

        private void RemoveRow(int rowIndex)
        {
            for (int i = (rowIndex+1)*width; i < data.Count; i++)
            {
                data.Set(i - width, data.Get(i));
            }
            implicants.RemoveAt(rowIndex);
            height--;
        }
        private void RemoveCol(int colIndex)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = i*width+colIndex - i; j < (i+1)*width+colIndex - 1; j++)
                {
                    if (j + 1 + i >= data.Count)
                        break;
                    data.Set(j, data.Get(j + 1 + i));
                }
            }
            width--;
        }

        private readonly bool CheckRowDomination(int row1, int row2)
        {
            int startIndex1 = row1 * width;
            int startIndex2 = row2 * width;
            for (int i = 0; i < width; i++)
            {
                if (data.Get(startIndex2+i) == true && data.Get(startIndex1+i) != true)
                    return false;
            }
            return true;
        }
        private readonly bool CheckColDomination(int col1, int col2)
        {
            for (int i = 0; i < height; i++)
            {
                if (data.Get(col2 + i*width) == true && data.Get(col1 + i*width) != true)
                    return false;
            }
            return true;
        }

        public readonly int GetLength()
        {
            return width*height;
        }

        public readonly void DEBUG()
        {
            Console.WriteLine("///TABLE");
            for (int y = 0; y < height; y++)
            {
                Console.Write(implicants[y] + "\t");
                for (int x = 0; x < width; x++)
                {
                    Console.Write(Get(x, y) + " ");
                }
                Console.WriteLine();
            }
            if (data.Count == 0)
                Console.WriteLine("EMPTY");
        }
    }
}
