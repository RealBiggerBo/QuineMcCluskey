List<int> expectedOutputs = new List<int>();

while (true)
{
    string? str = Console.ReadLine();
    if (!int.TryParse(str, out int res))
        break;

    expectedOutputs.Add(res);
}

for (int i = 0; i<expectedOutputs.Count; i++)
{
    Console.WriteLine(expectedOutputs[i]);
}