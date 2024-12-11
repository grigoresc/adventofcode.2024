using aoc.common;
using Xunit.Abstractions;

namespace day11
{
    public class Solve
    {
        [Theory]
        [InlineData("input.txt", 25, 194782)]
        [InlineData("input.txt", 75, 194782)]
        [InlineData("sample.txt", 1, 7)]
        [InlineData("125 17", 6, 22)]
        [InlineData("125 17", 1, 3)]
        [InlineData("125 17", 25, 55312)]
        public void Part1(string input, int times, int sln)
        {
            var stones = input.ParseAsLines()[0].ReadNumbers();

            var cnt1 = (from stone in stones
                        select Blink(stone, times)).Sum();
            cnt1.Dump().AssertSolved(sln);

        }

        private long Blink(long stone, int times)
        {
            if (times == 0)
            {
                //stone.Dump();
                return 1;
            }

            if (stone == 0)
                return Blink(1, times - 1);
            //if stone has even number of digits
            var len = stone.ToString().Length;
            if (len % 2 == 0)
            {
                var right = stone % (long)Math.Pow(10, len / 2);
                var left = (long)(stone / (long)Math.Pow(10, len / 2));
                return Blink(left, times - 1) + Blink(right, times - 1);
            }

            return Blink(stone * 2024, times - 1);//(long)Math.Pow(10, 11), times - 1);
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