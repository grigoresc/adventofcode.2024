using aoc.common;
using Xunit.Abstractions;

namespace day12
{
    public class Solve
    {
        [Theory]
        [InlineData("AAAA\r\nBBCD\r\nBBCC\r\nEEEC", 140, 80)]
        [InlineData("OOOOO\r\nOXOXO\r\nOOOOO\r\nOXOXO\r\nOOOOO", 772, 436)]
        [InlineData("EEEEE\r\nEXXXX\r\nEEEEE\r\nEXXXX\r\nEEEEE", 692, 236)]
        [InlineData("AAAAAA\r\nAAABBA\r\nAAABBA\r\nABBAAA\r\nABBAAA\r\nAAAAAA", 1184, 368)]
        [InlineData("RRRRIICCFF\r\nRRRRIICCCF\r\nVVRRRCCFFF\r\nVVRCCCJFFF\r\nVVVVCJJCFE\r\nVVIVCCJJEE\r\nVVIIICJJEE\r\nMIIIIIJJEE\r\nMIIISIJEEE\r\nMMMISSJEEE", 1930, 1206)]
        [InlineData("input.txt", 1381056, 834828)]
        public void Both(string input, long sln1, long sln2)
        {
            var map = input.ParseAsLines().ToCharMatrix();
            var len = map.GetLength(0);
            var visited = new HashSet<(int, int)>();
            var ret1 = 0L;
            var ret2 = 0L;
            for (int r = 0; r < len; r++)
                for (int c = 0; c < len; c++)
                    if (!visited.Contains((r, c)))
                    {
                        var (perim, area, sides) = ComputeAreaAndParameter(map, (r, c), visited);
                        ret1 += perim * area;
                        ret2 += sides * area;
                    }
            ret1.Dump().AssertSolved(sln1);
            ret2.Dump().AssertSolved(sln2);
        }

        (long, long, long) ComputeAreaAndParameter(char[,] map, (int, int) startPos, HashSet<(int, int)> visited)
        {
            var len = map.GetLength(0);
            var tovisit = new Queue<(int, int)>();

            var c = map[startPos.Item1, startPos.Item2];
            tovisit.Enqueue(startPos);
            long area = 0L;
            long perimeter = 0L;
            long sides = 0L;

            Dictionary<Map.Dir, List<(int, int)>> sidesByDir = new Dictionary<Map.Dir, List<(int, int)>>();

            sidesByDir[Map.Dir.N] = new List<(int, int)>();
            sidesByDir[Map.Dir.E] = new List<(int, int)>();
            sidesByDir[Map.Dir.S] = new List<(int, int)>();
            sidesByDir[Map.Dir.W] = new List<(int, int)>();

            while (tovisit.Any())
            {
                var pos = tovisit.Dequeue();

                if (!visited.Add(pos))
                    continue;

                foreach (var dir in new[] { Map.Dir.N, Map.Dir.E, Map.Dir.S, Map.Dir.W })
                {
                    var near = Map.Move(dir, pos);
                    if (!Map.IsInBounds(near, map) || map[near.Item1, near.Item2] != c)
                    {
                        perimeter++;
                        sidesByDir[dir].Add(pos);
                    }
                    else
                    {
                        if (!visited.Contains(near))
                            tovisit.Enqueue(near);
                    }
                }
                area++;
            }
            //process sides by dir
            foreach (var dir in new[] { Map.Dir.N, Map.Dir.E, Map.Dir.S, Map.Dir.W })
            {
                var side = sidesByDir[dir];
                var segmentsByDir = dir switch
                {
                    Map.Dir.N or Map.Dir.S =>
                                            (from s in side
                                             group s by s.Item1 into g
                                             select (g.Key, g.Select(o => o.Item2).Order().ToList())),
                    Map.Dir.E or Map.Dir.W =>
                                            (from s in side
                                             group s by s.Item2 into g
                                             select (g.Key, g.Select(o => o.Item1).Order().ToList()))
                };
                foreach (var (_, segments) in segmentsByDir)
                {
                    //consider a side two segments not connected
                    sides += 1 + segments[..^1].Zip(segments[1..]).Sum(o => o.First + 1 < o.Second ? 1 : 0);
                }
            }
            return (perimeter, area, sides);
        }

        public Solve(ITestOutputHelper output)
        {
            this.Setup(output);
        }
    }
}