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
            var distances = new int[map.GetLength(0), map.GetLength(1)];
            var d = 0;
            distances[current.row, current.col] = d;
            current.Dump("start");
            while (current != end)
            {
                d++;
                foreach (var n in Map.Adjacents(current))
                    if (Map.IsInBounds(n, map) && distances[n.l, n.c] == 0)
                    {
                        if (map[n.l, n.c] == '.' || map[n.l, n.c] == 'E')
                        {
                            current = n;
                            distances[current.row, current.col] = d;
                            break;
                        }
                    }

                //current.Dump();
                //d.Dump();
            }

            var saves = Process(map, distances);
            //saves.ToArray().Dump();
            //show saves group by counts
            saves.GroupBy(s => s).Select(g => (g.Key, g.Count())).OrderBy(g => g.Key).ToArray().Dump();
            saves.Where(s => s >= min).Count().Dump().AssertSolved(sln);


            //saves.Where(s => s == 2).Count().Dump();
            //saves.Where(s => s == 12).Count().Dump();

            //d.Dump();
        }

        [Theory]
        [InlineData("sample.txt", 50, 20, 285)]
        [InlineData("input.txt", 100, 20, 994807)]
        public void Part2(string input, int minSavedTime, int cheatSize, int sln)
        {
            var map = input.ParseAsLines().ToCharMatrix();
            var start = map.IteratePos().First(x => x.Item2 == 'S').Item1;
            var end = map.IteratePos().First(x => x.Item2 == 'E').Item1;
            var current = start;
            var distances = new int[map.GetLength(0), map.GetLength(1)];
            var d = 0;
            distances[current.row, current.col] = d;
            //current.Dump("start");
            while (current != end)
            {
                d++;
                foreach (var n in Map.Adjacents(current))
                    if (Map.IsInBounds(n, map) && distances[n.l, n.c] == 0)
                    {
                        if (map[n.l, n.c] == '.' || map[n.l, n.c] == 'E')
                        {
                            current = n;
                            distances[current.row, current.col] = d;
                            break;
                        }
                    }

                //current.Dump();
                //d.Dump();
            }

            var saves = Process2(map, distances, cheatSize).Select(s => s.Item3);

            //saves.ToArray().Dump("saves");
            //show saves group by counts
            saves.GroupBy(s => s).Select(g => (g.Key, g.Count())).OrderBy(g => g).ToArray().Dump();
            saves.Where(s => s >= minSavedTime).Count().Dump().AssertSolved(sln);


            //saves.Where(s => s == 2).Count().Dump();
            //saves.Where(s => s == 12).Count().Dump();

            //d.Dump();
            //1039300 - not!
        }

        private static IEnumerable<((int, int), (int, int), int)> Process2(char[,] map, int[,] distances, int cheatsize)
        {
            foreach (var c in map.IteratePos().Where(o => o.val == '.' || o.val == 'S'))
            {
                //c.Dump("c");
                for (var ni = c.Item1.row - cheatsize; ni <= c.Item1.row + cheatsize; ni++)
                {
                    //ni.Dump("ni");
                    for (var nj = c.Item1.col - cheatsize; nj <= c.Item1.col + cheatsize; nj++)
                        if (Map.IsInBounds(ni, nj, map))
                        {
                            var cheat = Math.Abs(ni - c.Item1.row) + Math.Abs(nj - c.Item1.col);
                            if (cheat <= cheatsize)
                                if (ni != c.Item1.row || nj != c.Item1.col)
                                {


                                    //(ni, nj).Dump("n");
                                    var nout = (ni, nj);
                                    //c.Item1.Dump("c");
                                    //foreach (var n in Map.Adjacents(c.Item1))
                                    //if (Map.IsInBounds(n, map) && map[n.l, n.c] == '#')
                                    //foreach (var nout in Map.Adjacents(n))
                                    if (Map.IsInBounds(nout, map) && map[nout.ni, nout.nj] != '#')
                                    //if (n != nout && nout != c.Item1)
                                    {
                                        var saved = -(distances[c.Item1.row, c.Item1.col] + cheat) +
                                                    distances[nout.ni, nout.nj];
                                        //nout.Dump($"nout; saved={saved}");
                                        //if (map[nout.ni, nout.nj] == 'E')
                                        //{
                                        //    //saved = dis
                                        //    continue;
                                        //}
                                        //if (saved > 80)
                                        //{
                                        //    c.Item1.Dump("c");

                                        //    (ni, nj).Dump("n");
                                        //}

                                        if (saved > 0)
                                            yield return (c.Item1, (ni, nj), saved);
                                    }
                                }
                        }
                }
            }
        }

        private static IEnumerable<int> Process(char[,] map, int[,] distances)
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
                                    var saved = -(distances[c.Item1.row, c.Item1.col] + 2) + distances[nout.l, nout.c];
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