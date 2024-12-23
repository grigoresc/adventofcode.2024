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
            var groups = new SortedDictionary<string, int>();
            var groupcnt = 0;
            //map.Dump("map");
            foreach (var line in map)
            {
                var link = line.ReadTokens("-").ToArray();
                var (l, r) = (link[0], link[1]);
                //$"{l}-{r}".Dump("line");

                if (groups.ContainsKey(l) && groups.ContainsKey(r))
                {
                    var newval = groups[l];
                    foreach (var gro in groups.Keys.ToArray())
                        if (groups[gro] == groups[r])
                            groups[gro] = newval;
                }
                else
                {
                    var g = -1;
                    if (groups.ContainsKey(l))
                        g = groups[l];
                    else if (groups.ContainsKey(r))
                        g = groups[r];
                    else
                        g = groupcnt++;
                    groups[l] = g;
                    groups[r] = g;
                }
            }

            //groups.Dump("groups");
            var gr = groups.GroupBy(o => o.Value).Select(g => (g.Key, g.Select(o => o.Key).ToArray()));
            var cnt = 0L;
            foreach (var group in gr)
            {
                //group.Key.Dump("group");

                for (var i = 0; i < group.Item2.Length; i++)
                    for (var j = i + 1; j < group.Item2.Length; j++)
                        for (var k = j + 1; k < group.Item2.Length; k++)

                        {
                            var l1 = group.Item2[i];
                            var l2 = group.Item2[j];
                            var l3 = group.Item2[k];
                            if (!(l1[0] == 't' || l2[0] == 't' || l3[0] == 't'))
                                continue;
                            var linked = true;
                            linked &= map.Contains($"{l1}-{l2}") || map.Contains($"{l2}-{l1}");
                            linked &= map.Contains($"{l3}-{l2}") || map.Contains($"{l2}-{l3}");
                            linked &= map.Contains($"{l1}-{l3}") || map.Contains($"{l3}-{l1}");

                            if (!linked)
                                continue;
                            //$"{l1}-{l2}-{l3}".Dump();
                            {
                                //$"found {l1}-{l2}-{l3}".Dump();
                                cnt++;
                            }
                        }

            }
            gr.ToArray().Dump();
            cnt.Dump().AssertSolved(sln);
        }


        public Solve(ITestOutputHelper output)
        {
            this.Setup(output);
        }
    }
}