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
        public void Both(string input, long sln1, long sln2)
        {
            var matrix = input.ParseAsLines().ToCharMatrix();
            var len = matrix.GetLength(0);
            var cnt1 = 0L;
            var cnt2 = 0L;
            for (int i = 0; i < len; i++)
                for (int j = 0; j < len; j++)
                    if (matrix[i, j] == '0')
                    {
                        var (c, hs) = Compute((i, j), matrix, len);
                        cnt2 += c;
                        cnt1 += hs.Count;
                    }

            cnt1.Dump().AssertSolved(sln1);
            cnt2.Dump().AssertSolved(sln2);
        }

        private (int, HashSet<(int, int)>) Compute((int l, int c) pos, char[,] m, int len)
        {
            if (m[pos.l, pos.c] == '9')
                return (1, [pos]);
            var reths = new HashSet<(int, int)>();
            var retc = 0;
            var c = m[pos.l, pos.c];
            foreach (var dir in new[] { Map.Dir.N, Map.Dir.E, Map.Dir.S, Map.Dir.W })
            {
                var (nl, nc) = Map.Move(dir, pos.l, pos.c);
                if (Map.IsInBounds(nl, nc, len))
                    if (m[nl, nc] == c + 1)
                    {
                        var (cnt, hs) = Compute((nl, nc), m, len);
                        retc += cnt;
                        foreach (var p in hs)
                        {
                            reths.Add(p);
                        }
                    }
            }
            return (retc, reths);
        }

        public Solve(ITestOutputHelper output)
        {
            this.Setup(output);
        }

    }
}