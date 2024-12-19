using aoc.common;
using Xunit.Abstractions;

namespace day19
{
    public class Solve
    {
        [Theory]
        [InlineData("sample.txt", 6)]
        [InlineData("input.txt", 278)]
        public void Part1(string input, long sln)
        {
            var l = input.ParseAsChunkOfLines();
            var patterns = l[0].ReadTokens(", ");
            patterns.Dump();
            var designs = l[1].ParseAsLines();
            designs.Dump();
            var cnt = 0L;
            foreach (var design in designs)
            {
                var ok = Match(design, patterns);
                if (ok)
                    cnt++;

            }

            cnt.Dump().AssertSolved(sln);
        }


        Dictionary<string, bool> memo = new();
        private bool Match(string design, string[] patterns)
        {
            //throw new NotImplementedException();
            if (design == "")
                return true;
            //design.Dump("match");

            if (memo.ContainsKey(design))
                return memo[design];

            var m = false;
            foreach (var p in patterns)
            {
                //p.Dump("pattern");
                if (design.Length >= p.Length && design.Substring(0, p.Length) == p)
                    if (Match(design.Substring(p.Length), patterns))
                    {
                        m = true;
                        break;
                    }
            }

            memo[design] = m;
            return m;
        }

        public Solve(ITestOutputHelper output)
        {
            this.Setup(output);
        }
    }
}