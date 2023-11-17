using QuineMcCluskey;

List<int> expectedOutputs = new List<int>();

//while (true)
//{
//    string? str = Console.ReadLine();
//    if (!int.TryParse(str, out int res))
//        break;

//    expectedOutputs.Add(res);
//}

QuineMcCluskeySolver solver = new QuineMcCluskeySolver(0, 1, 2, 6, 8, 9, 10, 13, 14, 15);
//QuineMcCluskeySolver solver = new QuineMcCluskeySolver(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
//QuineMcCluskeySolver solver = new QuineMcCluskeySolver(2, 3, 7, 9, 11, 13);
//QuineMcCluskeySolver solver = new QuineMcCluskeySolver(1, 3, 5, 10, 11, 12, 13, 14, 15);
Iteration result = solver.Solve();
result.DEBUG();


return;