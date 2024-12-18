using aoc.common;
using Xunit.Abstractions;

namespace day18
{
    public class Solve
    {
        [Theory]
        [InlineData("sample.txt", 12, 6 + 1, 22)]
        [InlineData("input.txt", 1024, 70 + 1, 360)]
        public void Part1(string input, int cnt, int size, long sln)
        {
            var coords = input.ParseAsLines().Take(cnt).Select(o =>
            {
                var (x, y, _) = o.ReadNumbers();
                return (x, y);
            });
            var pos = (0, 0);
            var dist = new Dictionary<(long, long), long>();

            Draw(coords, size);
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
                        near.Dump();

                        if (!dist.ContainsKey(near))
                        {
                            dist[near] = dist[pos] + 1;
                        }
                        else
                        {
                            if (dist[pos] + 1 < dist[near])
                                dist[near] = dist[pos] + 1;
                        }
                        if (!tovisit.Contains(near))//trebuie?
                            tovisit.Enqueue(near);
                    }
                }

                visited.Add(pos);
            }
            dist.Dump();
            dist[(size - 1, size - 1)].Dump("sln").AssertSolved(sln);
        }

        void Draw(IEnumerable<(long, long)> coords, int size)
        {
            var m = new char[size, size];
            for (var i = 0; i < size; i++)
                for (var j = 0; j < size; j++)
                    m[i, j] = ' ';
            foreach (var (x, y) in coords)
                m[y, x] = '#';
            m.Dump();
        }

        public Solve(ITestOutputHelper output)
        {
            this.Setup(output);
        }
    }
}