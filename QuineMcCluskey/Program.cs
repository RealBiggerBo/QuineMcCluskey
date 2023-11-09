using QuineMcCluskey;

List<int> expectedOutputs = new List<int>();

while (true)
{
    string? str = Console.ReadLine();
    if (!int.TryParse(str, out int res))
        break;

    expectedOutputs.Add(res);
}

new QuineMcCluskeySolver(expectedOutputs.ToArray()).Solve();