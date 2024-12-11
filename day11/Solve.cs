using aoc.common;
using Xunit.Abstractions;

namespace day11
{
    public class Solve
    {
        [Theory]
        [InlineData("0 1 10 99 999", 1, 7)]
        [InlineData("125 17", 6, 22)]
        [InlineData("125 17", 1, 3)]
        [InlineData("125 17", 25, 55312)]
        [InlineData("input.txt", 25, 194782)]
        [InlineData("input.txt", 75, 233007586663131)]
        public void Both(string input, int times, long sln)
        {
            var stones = input.ParseAsLines()[0].ReadNumbers();

            var cnt = (from stone in stones
                       select Blink(stone, times)).Sum();
            cnt.Dump().AssertSolved(sln);
        }

        Dictionary<(long, int), long> memo = new();

        private long Blink(long stone, int times)
        {
            if (memo.ContainsKey((stone, times)))
                return memo[(stone, times)];

            if (times == 0)
                return 1;

            var len = stone.ToString().Length;
            var ret = stone switch
            {
                0 => Blink(1, times - 1),
                _ when len % 2 == 0 =>
                    Blink(stone / (long)Math.Pow(10, len / 2), times - 1)
                    + Blink(stone % (long)Math.Pow(10, len / 2), times - 1),
                _ => Blink(stone * 2024, times - 1)

            };
            memo[(stone, times)] = ret;
            return ret;
        }
        public Solve(ITestOutputHelper output)
        {
            this.Setup(output);
        }

    }
}