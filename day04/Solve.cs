using aoc.common;
using Xunit.Abstractions;

namespace day04
{
    public class Solve
    {
        [Theory]
        [InlineData("input.txt", 2644)]
        [InlineData(@"MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX", 18)]
        public void Part1(string input, int sln)
        {
            var m = input.ParseAsLines();
            var res = 0;
            var len = m.Length;
            for (int i = 0; i < len; i++)
                for (int j = 0; j < len - 3; j++)
                {
                    var h = m[i][j..(j + 4)];
                    res += Ok(h) ? 1 : 0;
                }
            for (int i = 0; i < len - 3; i++)
                for (int j = 0; j < len; j++)
                {
                    var d = new String(new[] { m[i][j], m[i + 1][j], m[i + 2][j], m[i + 3][j] });
                    res += Ok(d) ? 1 : 0;
                }
            for (int i = 0; i < len - 3; i++)
                for (int j = 0; j < len - 3; j++)
                {
                    var d = new String(new[] { m[i][j], m[i + 1][j + 1], m[i + 2][j + 2], m[i + 3][j + 3] });
                    res += Ok(d) ? 1 : 0;
                }
            for (int i = 3; i < len; i++)
                for (int j = 0; j < len - 3; j++)
                {
                    var v = new String(new[] { m[i][j], m[i - 1][j + 1], m[i - 2][j + 2], m[i - 3][j + 3] });
                    res += Ok(v) ? 1 : 0;
                }
            res.Dump().AssertSolved(sln);
        }
        bool Ok(string w) => w == "XMAS" || w == "SAMX";

        [Theory]
        [InlineData("input.txt", 1952)]
        [InlineData(@"MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX", 9)]
        public void Part2(string input, int sln)
        {
            var m = input.ParseAsLines();
            var res = 0;
            var len = m.Length;
            for (int i = 0; i < len - 2; i++)
                for (int j = 0; j < len - 2; j++)
                {
                    var d = new String(new[] { m[i][j], m[i + 1][j + 1], m[i + 2][j + 2] });
                    var d2 = new String(new[] { m[i][j + 2], m[i + 1][j + 1], m[i + 2][j] });
                    res += Ok2(d) && Ok2(d2) ? 1 : 0;
                }
            res.Dump().AssertSolved(sln);
        }
        bool Ok2(string w) => w == "SAM" || w == "MAS";
        public Solve(ITestOutputHelper output)
        {
            this.Setup(output);
        }

    }
}