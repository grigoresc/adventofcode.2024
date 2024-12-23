using aoc.common;
using Xunit.Abstractions;

namespace day23
{
    public class Solve
    {
        [Theory]
        [InlineData("sample.txt", 7, "co,de,ka,ta")]
        [InlineData("input.txt", 1423, "gt,ha,ir,jn,jq,kb,lr,lt,nl,oj,pp,qh,vy")]
        public void Both(string input, long sln1, string sln2)
        {
            var map = input.ParseAsLines();
            var links = new SortedDictionary<string, List<string>>();
            foreach (var line in map)
            {
                var link = line.ReadTokens("-").ToArray();
                var (l, r) = (link[0], link[1]);
                if (!links.ContainsKey(l))
                    links[l] = new List<string>();
                if (!links.ContainsKey(r))
                    links[r] = new List<string>();
                links[l].Add(r);
                links[r].Add(l);
            }
            foreach (var k in links.Keys.ToArray())
                links[k].Sort();

            var cnt = 0L;
            foreach (var node in links.Keys)
            {
                cnt += FindParties([node], links, (a) => a.Length == 3 && a.Any(s => s.StartsWith("t"))).Count();
            }
            cnt.Dump().AssertSolved(sln1);

            var max = 0L;
            var str = "";
            foreach (var node in links.Keys)
            {
                FindParties([node], links, (a) =>
                {
                    if (a.Length > max)
                    {
                        max = a.Length;
                        str = string.Join(",", a);
                    }

                    return false;
                });
            }

            str.Dump().AssertSolved(sln2);
        }

        private List<string[]> FindParties(string[] startingParty,
            SortedDictionary<string, List<string>> links,
            Func<string[], bool> stopCondition)
        {
            if (stopCondition(startingParty))
                return new List<string[]> { startingParty };

            HashSet<string> candidates = new HashSet<string>();
            var frst = true;
            foreach (var node in startingParty)
            {
                if (frst)
                {
                    frst = false;
                    candidates = new HashSet<string>(links[node]);
                }
                candidates.IntersectWith(links[node]);
            }


            var ret = new List<string[]>();
            foreach (var node in candidates)
            {
                var ok = true;
                foreach (var existingNode in startingParty)
                    if (existingNode.CompareTo(node) > 0)
                        ok = false;
                if (!ok)
                    continue;
                ret.AddRange(FindParties(startingParty.Concat([node]).ToArray(), links, stopCondition));
            }

            return ret;
        }

        public Solve(ITestOutputHelper output)
        {
            this.Setup(output);
        }
    }
}