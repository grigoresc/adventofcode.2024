using aoc.common;
using Xunit.Abstractions;

namespace day08
{
    public class Solve
    {
        [Theory]
        [InlineData("input.txt", 259, 927)]
        [InlineData("sample.txt", 14, 34)]
        public void Both(string input, int sln1, int sln2)
        {
            var matrix = input.ParseAsLines().ToCharMatrix();
            var len = matrix.GetLength(0);

            var antennas = new Dictionary<char, List<(int, int)>>();
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < len; j++)
                {
                    if (matrix[i, j] != '.')
                    {
                        if (!antennas.ContainsKey(matrix[i, j]))
                        {
                            antennas[matrix[i, j]] = new List<(int, int)>();
                        }
                        antennas[matrix[i, j]].Add((i, j));
                    }
                }
            }

            var cnt1 = ComputeAntinodes(antennas, len, (1, 1)).Count;
            var cnt2 = ComputeAntinodes(antennas, len, (0, len)).Count;
            cnt1.Dump().AssertSolved(sln1);
            cnt2.Dump().AssertSolved(sln2);
        }

        private static HashSet<(int, int)> ComputeAntinodes(Dictionary<char, List<(int, int)>> antennas, int len, (int, int) mulRange)
        {
            var antinodes = new HashSet<(int, int)>();
            foreach (var (_, positions) in antennas)
            {
                for (int i = 0; i < positions.Count; i++)
                    for (int j = i + 1; j < positions.Count; j++)
                    {
                        var pos1 = positions[i];
                        var pos2 = positions[j];
                        var dx = pos1.Item1 - pos2.Item1;
                        var dy = pos1.Item2 - pos2.Item2;
                        for (int m = mulRange.Item1; m <= mulRange.Item2; m++)
                        {

                            var antinode1 = (pos1.Item1 + m * dx,
                                pos1.Item2 + m * dy);
                            var antinode2 = (pos2.Item1 - m * dx,
                                pos2.Item2 - m * dy);

                            if (Map.IsInBounds(antinode1, len))
                                antinodes.Add(antinode1);
                            if (Map.IsInBounds(antinode2, len))
                                antinodes.Add(antinode2);
                        }
                    }
            }

            return antinodes;
        }

        public Solve(ITestOutputHelper output)
        {
            this.Setup(output);
        }

    }
}