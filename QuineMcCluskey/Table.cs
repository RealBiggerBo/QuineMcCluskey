using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuineMcCluskey
{
    internal struct Table
    {
        private List<int> data;
        private List<Value> implicants;
        public int width;
        public int height;

        public Table(Value[] outputs, Value[] implicants)
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

        public void Set(int x,int y,int value)
        {
            data[y * width + x] = value;
        }
        public int Get(int x,int y)
        {
            return data[y * width + x];
        }
        public Value GetImplicant(int y)
        {
            return implicants[y];
        }

        public void RemoveEssential(int _y)
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

        public bool GetRowDominance(ref Table table)
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

        public bool GetColumnDominance(ref Table table)
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
            row1.ForEach((i)=>Console.Write(i));
            Console.Write(" Dominates?: ");
            row2.ForEach((i) => Console.Write(i));
            Console.WriteLine();
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
}
