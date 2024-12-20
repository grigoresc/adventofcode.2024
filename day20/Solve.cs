using aoc.common;
using Xunit.Abstractions;

namespace day20
{
    public class Solve
    {
        [Theory]
        [InlineData("sample.txt", 64, 1)]
        [InlineData("input.txt", 100, 1399)]
        public void Part1(string input, int min, int sln)
        {
            var map = input.ParseAsLines().ToCharMatrix();
            var start = map.IteratePos().First(x => x.Item2 == 'S').Item1;
            var end = map.IteratePos().First(x => x.Item2 == 'E').Item1;
            var current = start;
            var distance = new int[map.GetLength(0), map.GetLength(1)];
            var d = 0;
            distance[current.row, current.col] = d;
            current.Dump("start");
            while (current != end)
            {
                d++;
                foreach (var n in Map.Adjacents(current))
                    if (Map.IsInBounds(n, map) && distance[n.l, n.c] == 0)
                    {
                        if (map[n.l, n.c] == '.' || map[n.l, n.c] == 'E')
                        {
                            current = n;
                            distance[current.row, current.col] = d;
                            break;
                        }
                    }

                //current.Dump();
                //d.Dump();
            }

            var saves = Process(map, distance);
            //saves.ToArray().Dump();
            //show saves group by counts
            saves.GroupBy(s => s).Select(g => (g.Key, g.Count())).OrderBy(g => g.Key).ToArray().Dump();
            saves.Where(s => s >= min).Count().Dump().AssertSolved(sln);


            //saves.Where(s => s == 2).Count().Dump();
            //saves.Where(s => s == 12).Count().Dump();

            //d.Dump();
        }

        private static IEnumerable<int> Process(char[,] map, int[,] distance)
        {
            foreach (var c in map.IteratePos().Where(o => o.val == '.' || o.val == 'S'))
            {
                //c.Item1.Dump("c");
                foreach (var n in Map.Adjacents(c.Item1))
                    if (Map.IsInBounds(n, map) && map[n.l, n.c] == '#')
                        foreach (var nout in Map.Adjacents(n))
                            if (Map.IsInBounds(nout, map) && map[nout.l, nout.c] != '#')
                                if (n != nout && nout != c.Item1)
                                {
                                    //nout.Dump("nout");
                                    var saved = -(distance[c.Item1.row, c.Item1.col] + 2) + distance[nout.l, nout.c];
                                    if (saved > 0)
                                        yield return saved;
                                }
            }
        }

        public Solve(ITestOutputHelper output)
        {
            this.Setup(output);
        }
    }
}