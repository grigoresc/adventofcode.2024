using aoc.common;
using Xunit.Abstractions;

namespace day19
{
    public class Solve
    {
        [Theory]
        [InlineData("sample.txt", 6, 16)]
        [InlineData("input.txt", 278, 569808947758890)]
        public void Both(string input, int sln1, long sln2)
        {
            var (patterns, designs) = Parse(input);

            var matches = (from design in designs select Match(design, patterns)).ToList();
            var (cnt1, cnt2) = (matches.Count(x => x > 0), matches.Sum());

            cnt1.Dump().AssertSolved(sln1);
            cnt2.Dump().AssertSolved(sln2);
        }

        private static (string[] patterns, string[] designs) Parse(string input)
        {
            var l = input.ParseAsChunkOfLines();
            var patterns = l[0].ReadTokens(", ");
            var designs = l[1].ParseAsLines();
            return (patterns, designs);
        }

        Dictionary<string, long> memo = new();
        private long Match(string design, string[] patterns)
        {
            if (design == "")
                return 1;

            if (memo.ContainsKey(design))
                return memo[design];

            var s = from p in patterns
                    where design.Length >= p.Length && design[0..p.Length] == p
                    select Match(design[p.Length..], patterns);

            memo[design] = s.Sum();
            return memo[design];
        }

        public Solve(ITestOutputHelper output)
        {
            this.Setup(output);
        }
    }
}