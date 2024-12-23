using aoc.common;
using Xunit.Abstractions;

namespace day23
{
    public class Solve
    {
        [Theory]
        [InlineData("sample.txt", 64, 7, "co,de,ka,ta")]
        [InlineData("input.txt", 100, 1423, "co,de,ka,ta")]
        public void Part1(string input, int min, long sln, string sln2)
        {
            var map = input.ParseAsLines();
            var links = new SortedDictionary<string, List<string>>();
            var nodes = new SortedSet<string>();
            var groupcnt = 0;
            //map.Dump("map");
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
                nodes.Add(l);
                nodes.Add(r);
            }
            foreach (var k in links.Keys.ToArray())
                links[k].Sort();

            var cnt = 0L;
            foreach (var node in nodes)
            //if (node.StartsWith("t"))
            {
                var parties = FindParties([node], links);
                foreach (var party in parties)
                    party.Dump();
                cnt += parties.Count;
            }

            cnt.Dump().AssertSolved(sln);
        }

        private List<string[]> FindParties(string[] startingParty, SortedDictionary<string, List<string>> links)
        {
            if (startingParty.Length == 3 && startingParty.Any(s => s.StartsWith("t")))
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
                ret.AddRange(FindParties(startingParty.Concat([node]).ToArray(), links));
            }

            return ret;
        }


        public Solve(ITestOutputHelper output)
        {
            this.Setup(output);
        }
    }
}