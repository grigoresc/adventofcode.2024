using aoc.common;
using Xunit.Abstractions;

namespace day18
{
    public class Solve
    {
        [Theory]
        [InlineData("sample.txt", 12, 6 + 1, 22)]
        //[InlineData("input.txt", 1024, 70 + 1, 360)]
        public void Part1(string input, int cnt, int size, long sln)
        {
            var coords = input.ParseAsLines().Select(o =>
            {
                var (x, y, _) = o.ReadNumbers();
                return (x, y);
            });
            coords.Draw(size);
            var dist = Shortest(size, coords.Take(cnt));
            dist[(size - 1, size - 1)].Dump("sln").AssertSolved(sln);
        }

        [Theory]
        [InlineData("sample.txt", 6 + 1, "6,1")]
        [InlineData("input.txt", 70 + 1, "58,62")]
        public void Part2(string input, int size, string sln)
        {
            var coords = input.ParseAsLines().Select(o =>
            {
                var (x, y, _) = o.ReadNumbers();
                return (x, y);
            }).ToArray();

            var cntMax = coords.Count();
            var cntMin = 1;
            while (cntMin + 1 < cntMax)
            {
                var cnt = (cntMax + cntMin) / 2;
                $"{cntMin},{cntMax},{cnt}".Dump();
                var dist = Shortest(size, coords.Take(cnt));

                if (!dist.ContainsKey((size - 1, size - 1)))
                {
                    cntMax = cnt;
                }
                else
                {
                    cntMin = cnt;
                }
            }

            $"{coords[cntMin].x},{coords[cntMin].y}".Dump().AssertSolved(sln);
        }
        private Dictionary<(long, long), long> Shortest(int size, IEnumerable<(long x, long y)> coords)
        {
            var pos = (0, 0);
            var dist = new Dictionary<(long, long), long>();

            var tovisit = new Queue<(int, int)>();
            var visited = new HashSet<(int, int)>();
            dist[pos] = 0;
            tovisit.Enqueue(pos);

            while (tovisit.Count > 0)
            {
                pos = tovisit.Dequeue();
                foreach (var dir in new[] { Map.Dir.N, Map.Dir.E, Map.Dir.S, Map.Dir.W })
                {
                    var near = Map.Move(dir, pos);
                    if (
                        Map.IsInBounds(near, size)
                        && !coords.Contains(near)
                        && !visited.Contains(near))
                    {

                        if (!dist.ContainsKey(near))
                        {
                            dist[near] = dist[pos] + 1;
                        }
                        else
                        {
                            if (dist[pos] + 1 < dist[near])
                                dist[near] = dist[pos] + 1;
                        }
                        if (!tovisit.Contains(near))
                            tovisit.Enqueue(near);
                    }
                }

                visited.Add(pos);
            }

            return dist;
        }

        public Solve(ITestOutputHelper output)
        {
            this.Setup(output);
        }
    }
}