using aoc.common;
using Xunit.Abstractions;

namespace day06
{
    public class Solve
    {
        [Theory]
        [InlineData("input.txt", 4656, 1575)]
        [InlineData(@"sample.txt", 41, 6)]
        public void Both(string input, long sln, long sln2)
        {
            var matrix = input.ParseAsLines().ToCharMatrix();
            var cnt2 = 0L;
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

            for (int i = 0; i < len; i++)
                for (int j = 0; j < len; j++)
                {
                    if (matrix[i, j] == '.')
                    {

                        var matrixWip = matrix.Clone() as char[,];

                        matrixWip[i, j] = '#';

                        if (!Run(matrixWip, ci, cj, dir, len))
                            cnt2++;
                    }
                }
            Run(matrix, ci, cj, dir, len);
            var cnt = 0L;
            for (int i = 0; i < len; i++)
                for (int j = 0; j < len; j++)
                {
                    if (matrix[i, j] == 'X')
                        cnt++;
                }
            cnt.AssertSolved(sln).Dump();
            cnt2.AssertSolved(sln2).Dump();
        }

        private bool Run(char[,] matrix, int ci, int cj, Dir dir, int len)
        {
            matrix[ci, cj] = '.';
            var loopcnt = 0;
            while (true)
            {
                if (loopcnt > len * len)
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