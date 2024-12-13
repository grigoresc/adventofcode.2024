using aoc.common;
using Xunit.Abstractions;

namespace day13
{
    public class Solve
    {
        [Theory]
        [InlineData("sample.txt", 480)]
        [InlineData("input.txt", 29388)]
        public void Part1(string input, long sln)
        {
            var machines = input.ParseAsChunkOfLines();
            var ret = 0L;
            var aCost = 3;
            var bCost = 1;
            var maxPresses = 100;
            foreach (var machine in machines)
            {
                var (aX, aY, bX, bY, finalX, finalY) = Parse(machine);
                var min = -1L;
                for (long i = 0; i <= maxPresses; i++)
                    for (long j = 0; j <= maxPresses; j++)
                    {
                        if (aX * i + bX * j == finalX && aY * i + bY * j == finalY)
                        {
                            var cost = aCost * i + bCost * j;
                            if (min == -1 || cost < min)
                                min = cost;
                        }
                    }

                if (min != -1)
                    ret += min;
            }
            ret.Dump().AssertSolved(sln);
        }

        [Theory]
        [InlineData("sample.txt", 875318608908)]
        [InlineData("Button A: X+26, Y+66\r\nButton B: X+67, Y+21\r\nPrize: X=12748, Y=12176", 459236326669)]
        [InlineData("input.txt", 99548032866004)]
        public void Part2(string input, long sln)
        {
            var machines = input.ParseAsChunkOfLines();
            var ret = 0L;
            var aCost = 3;
            var bCost = 1;

            foreach (var machine in machines)
            {
                var (aX, aY, bX, bY, finalX, finalY) = Parse(machine);
                finalX += 10000000000000;
                finalY += 10000000000000;

                //resolve a system of two equations and two unknowns
                var numerator = aX * finalY - aY * finalX;
                var denominator = aX * bY - aY * bX;
                if (numerator % denominator == 0)
                {
                    var cj = numerator / denominator;
                    var ci = (finalX - cj * bX) / aX;
                    if (finalY == ci * aY + cj * bY)
                    {
                        var cost = ci * aCost + cj * bCost;
                        ret += cost;
                    }
                }
            }

            ret.Dump().AssertSolved(sln);
        }
        private static (long aX, long aY, long bX, long bY, long finalX, long finalY) Parse(string machine)
        {
            var s = machine.ParseAsLines();
            var (aX, aY, _) = s[0].ReadNumbers();
            var (bX, bY, _) = s[1].ReadNumbers();
            var (finalX, finalY, _) = s[2].ReadNumbers();
            return (aX, aY, bX, bY, finalX, finalY);
        }

        public Solve(ITestOutputHelper output)
        {
            this.Setup(output);
        }
    }
}