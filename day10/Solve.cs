using aoc.common;
using Xunit.Abstractions;

namespace day10
{
    public class Solve
    {
        [Theory]
        [InlineData("input.txt", 674, 1372)]
        [InlineData("..90..9\r\n...1.98\r\n...2..7\r\n6543456\r\n765.987\r\n876....\r\n987....", 4, 13)]
        [InlineData("sample.txt", 36, 81)]
        public void Both(string input, int sln1, int sln2)
        {
            var matrix = input.ParseAsLines().ToCharMatrix();

            var x = from i in Enumerable.Range(0, matrix.GetLength(0))
                    from j in Enumerable.Range(0, matrix.GetLength(1))
                    where matrix[i, j] == '0'
                    select Compute((i, j), matrix);
            x.Sum(o => o.Item1.Count).Dump().AssertSolved(sln1);
            x.Sum(o => o.Item2).Dump().AssertSolved(sln2);
        }

        private (HashSet<(int, int)>, int) Compute((int l, int c) pos, char[,] m)
        {
            var c = m[pos.l, pos.c];
            if (c == '9')
                return ([pos], 1);
            var y = from dir in new[] { Map.Dir.N, Map.Dir.E, Map.Dir.S, Map.Dir.W }
                    let next = Map.Move(dir, pos)
                    where Map.IsInBounds(next, m)
                    where m[next.l, next.c] == c + 1
                    select Compute(next, m);
            var reths = y.SelectMany(o => o.Item1).ToHashSet();
            var retc = y.Select(o => o.Item2).Sum();
            return (reths, retc);
        }

        public Solve(ITestOutputHelper output)
        {
            this.Setup(output);
        }

    }
}