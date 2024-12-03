using aoc.common;
using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace day03
{
    public class Solve
    {
        [Theory]
        [InlineData("input.txt", 181345830)]
        [InlineData(@"xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))", 161)]
        public void Part1(string input, int sln)
        {
            var lines = input.ParseAsLines();
            var regex = new Regex(@"mul\([\d]{1,3},[\d]{1,3}\)");

            var res = 0L;
            foreach (var line in lines)
            {
                var matches = regex.Matches(line);
                foreach (Match m in matches)
                {
                    var n = m.Value.ReadNumbers();
                    res += n[0] * n[1];
                }
            }

            res.Dump().AssertSolved(sln);
        }

        [Theory]
        [InlineData("input.txt", 98729041)]
        [InlineData(@"xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))", 48)]
        public void Part2(string input, int sln)
        {
            var lines = input.ParseAsLines();
            var regex = new Regex(@"(mul\([\d]{1,3},[\d]{1,3}\))|(don't\(\))|(do\(\))");

            var res = 0L;
            var active = true;
            foreach (var line in lines)
            {
                var matches = regex.Matches(line);
                foreach (Match m in matches)
                {
                    if (m.Value == "do()")
                    {
                        active = true;
                        continue;
                    }
                    else if (m.Value == "don't()")
                    {
                        active = false;
                        continue;
                    }
                    var n = m.Value.ReadNumbers();
                    if (active)
                        res += n[0] * n[1];
                }
            }

            res.Dump().AssertSolved(sln);
        }

        public Solve(ITestOutputHelper output)
        {
            this.Setup(output);
        }

    }
}