using QuineMcCluskey;

List<int> expectedOutputs = new List<int>();

//while (true)
//{
//    string? str = Console.ReadLine();
//    if (!int.TryParse(str, out int res))
//        break;

//    expectedOutputs.Add(res);
//}

//Table a = new Table([new Value(1, 4), new Value(3, 4), new Value(5, 4)], [new Value("00-1"), new Value("0-01"), new Value("-011"), new Value("-101")]);
//a.DEBUG();
//a.GetRowDominance(ref a);
//a.DEBUG();
//a.GetRowDominance(ref a);
//a.DEBUG();
//a.GetRowDominance(ref a);
//a.DEBUG();

QuineMcCluskeySolver solver = new QuineMcCluskeySolver(1, 3, 5, 10, 11, 12, 13, 14, 15);
Iteration result = solver.Solve();
result.DEBUG();


return;