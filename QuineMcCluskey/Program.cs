﻿using QuineMcCluskey;
using System.Diagnostics;

//List<int> expectedOutputs = new List<int>();
//while (true)
//{
//    string? str = Console.ReadLine();
//    if (!int.TryParse(str, out int res))
//        break;

//    expectedOutputs.Add(res);
//}
//IntQuineMcCluskeySolver solver = new IntQuineMcCluskeySolver(expectedOutputs.ToArray());

//a xor b and e or g and e xor f and c or d nand h
//https://www.dcode.fr/boolean-truth-table

Stopwatch s = new Stopwatch();
s.Start();
//IntQuineMcCluskeySolver solver = new IntQuineMcCluskeySolver(1, 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 33, 41, 43, 47, 53, 59, 61, 130, 93, 74, 88, 80, 30,1024,583,567,736,475,934,459,623,593,2034,2945,2452,3453,4564,4564,2345,432,2345,3434,654,73,4844,7578);
//IntQuineMcCluskeySolver solver = new IntQuineMcCluskeySolver(1, 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 33, 41, 43, 47, 53, 59, 61);
//QuineMcCluskeySolver solver = new QuineMcCluskeySolver(0, 1, 2, 6, 8, 9, 10, 13, 14, 15);
//QuineMcCluskeySolver solver = new QuineMcCluskeySolver(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
//QuineMcCluskeySolver solver = new QuineMcCluskeySolver(2, 3, 7, 9, 11, 13);
//IntQuineMcCluskeySolver solver = new IntQuineMcCluskeySolver(1, 3, 5, 10, 11, 12, 13, 14, 15);
Iteration_Optimised result = solver.Solve().Result;
s.Stop();

result.DEBUG();
Console.WriteLine("TOOK: " + s.ElapsedMilliseconds + "ms");

return;