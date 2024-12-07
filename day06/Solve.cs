using aoc.common;
using Xunit.Abstractions;

namespace day06
{
    public class Solve
    {
        [Theory]
        [InlineData("input.txt", 4656, 1575)]
        [InlineData(@"sample.txt", 41, 6)]
        public void Both(string input, long sln1, long sln2)
        {
            var matrix = input.ParseAsLines().ToCharMatrix();
            var len = matrix.GetLength(0);
            var ci = 0; var cj = 0;
            for (int i = 0; i < len; i++)
                for (int j = 0; j < len; j++)
                {
                    if (matrix[i, j] == '^')
                    {
                        ci = i;
                        cj = j;
                        break;
                    }
                }

            var dir = Dir.N;

            var run1 = matrix.Clone() as char[,];
            Run(run1, ci, cj, dir, len);
            var cnt1 = 0L;
            for (int i = 0; i < len; i++)
                for (int j = 0; j < len; j++)
                {
                    if (run1[i, j] == 'X')
                        cnt1++;
                }
            cnt1.AssertSolved(sln1).Dump();

            var cnt2 = 0L;
            dir = Dir.N;
            for (int i = 0; i < len; i++)
                for (int j = 0; j < len; j++)
                {
                    if (run1[i, j] == 'X')//we try blocking this one
                    {
                        var run2 = matrix.Clone() as char[,];
                        run2[i, j] = '#';

                        if (!Run(run2, ci, cj, dir, len))
                            cnt2++;
                    }
                }
            cnt2.AssertSolved(sln2).Dump();
        }

        private bool Run(char[,] matrix, int ci, int cj, Dir dir, int len)
        {
            matrix[ci, cj] = '.';
            var loopcnt = 0;
            while (true)
            {
                if (loopcnt > len * len)//todo - something that can be improved here.. (memorize the directions)
                    return false;
                //next pos
                var (ni, nj) = Move(dir, ci, cj);
                if (IsInBounds(ni, nj, len))
                    if (matrix[ni, nj] == '#')
                    {
                        dir = TurnRight(dir);
                        continue;
                    }
                    else
                    {
                        ci = ni;
                        cj = nj;
                        matrix[ci, cj] = 'X';
                        loopcnt++;
                    }
                else
                {
                    break;
                }
            }

            return true;
        }

        private enum Dir { N, E, S, W }

        private Dir TurnRight(Dir d) => d switch
        {
            Dir.N => Dir.E,
            Dir.E => Dir.S,
            Dir.S => Dir.W,
            Dir.W => Dir.N,
            _ => throw new System.Exception("Invalid direction")
        };

        private (int, int) Move(Dir d, int i, int j) => d switch
        {
            Dir.N => (i - 1, j),
            Dir.E => (i, j + 1),
            Dir.S => (i + 1, j),
            Dir.W => (i, j - 1),
            _ => throw new System.Exception("Invalid direction")
        };

        bool IsInBounds(int i, int j, int len) => i >= 0 && i < len && j >= 0 && j < len;

        public Solve(ITestOutputHelper output)
        {
            this.Setup(output);
        }

    }
}