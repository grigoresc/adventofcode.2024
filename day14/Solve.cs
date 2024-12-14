using aoc.common;
using Xunit.Abstractions;

namespace day14
{
    public class Solve
    {
        [Theory]
        [InlineData("sample.txt", 12, 11, 7, 100)]
        [InlineData("input.txt", 230436441, 101, 103, 100)]
        public void Part1(string input, long sln, int wide, int tall, int seconds)
        {
            var robots = input.ParseAsLines();
            var ret = Run(wide, tall, seconds, robots);
            ret.Dump().AssertSolved(sln);
        }

        private static long Run(int wide, int tall, int seconds, string[] robots)
        {
            var quadrands = new[] {(
                    0, (wide / 2), 0, (tall / 2))
                , ((wide / 2)+1, wide, 0, (tall / 2))
                , (0, (wide / 2), (tall / 2)+1, tall)
                , ((wide / 2) + 1, wide, (tall / 2) + 1, tall)};
            var cnts = new int[4];
            foreach (var robot in robots)
            {
                var n = robot.ReadNumbers();
                var (x, y) = (n[0], n[1]);
                var (vx, vy) = (n[2], n[3]);
                for (var i = 0; i < seconds; i++)
                {
                    x = x + vx;
                    if (x < 0) x += wide;
                    x = x % wide;
                    Assert.True(x >= 0, "nu se poate!");
                    y = y + vy;
                    if (y < 0) y += tall;
                    y = y % tall;
                    Assert.True(y >= 0, "nu se poate!");
                }

                var q = 0;
                foreach (var (x1, x2, y1, y2) in quadrands)
                {
                    if (x1 <= x && x < x2)
                        if (y1 <= y && y < y2)
                            cnts[q]++;
                    q++;
                }

                //(x, y).Dump();
            }
            var ret = 1L;
            cnts.Dump();
            foreach (var cnt in cnts) ret *= cnt;
            return ret;
        }

        [Theory]
        [InlineData("sample.txt", 12, 11, 7, 100)]
        [InlineData("input.txt", 230436441, 101, 103, 100)]
        public void Part2(string input, long sln, int wide, int tall, int seconds)
        {
            var robots = input.ParseAsLines();
            var m = new char[tall, wide];
            var tries = 100;
            while (true)
            {
                for (var i = 0; i < tall; i++)
                    for (var j = 0; j < wide; j++)
                        m[i, j] = ' ';
                foreach (var robot in robots)
                {
                    var n = robot.ReadNumbers();
                    var (x, y) = (n[0], n[1]);
                    var (vx, vy) = (n[2], n[3]);
                    for (var i = 0; i < seconds; i++)
                    {
                        x = x + vx;
                        if (x < 0) x += wide;
                        x = x % wide;
                        y = y + vy;
                        if (y < 0) y += tall;
                        y = y % tall;
                    }
                    m[y, x] = '*';

                }


                if (tries-- == 0)
                    break;
                m.Dump();
            }
            //ret.Dump().AssertSolved(sln);
        }
        public Solve(ITestOutputHelper output)
        {
            this.Setup(output);
        }
    }
}