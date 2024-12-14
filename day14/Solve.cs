using aoc.common;
using Xunit.Abstractions;

namespace day14
{
    public class Solve
    {
        [Theory]
        [InlineData("sample.txt", 12, 11, 7, 100)]
        [InlineData("input.txt", 230436441, 101, 103, 100)]
        public void Part1(string input, int sln, int wide, int tall, int seconds)
        {
            var (robotsPos, robotsVel) = Parse(input);

            Move(wide, tall, robotsPos, robotsVel)
                .ElementAt(seconds - 1)
                .quadrants.Aggregate(1, (x, y) => x * y).Dump().AssertSolved(sln);
        }

        [Theory]
        [InlineData("input.txt", 8270, 101, 103, 100)]
        public void Part2(string input, int sln, int wide, int tall, int seconds)
        {
            var (robotsPos, robotsVel) = Parse(input);

            var minRobotsPerQuadrant = 250;
            Move(wide, tall, robotsPos, robotsVel)
                .First(o => o.quadrants.Any(c => c >= minRobotsPerQuadrant))
                .Seconds.Dump().AssertSolved(sln);
        }

        private static (List<(long, long)> robotsPos, List<(long, long)> robotsVel) Parse(string input)
        {
            var robots = input.ParseAsLines();
            var robotsPos = new List<(long, long)>();
            var robotsVel = new List<(long, long)>();
            foreach (var robot in robots)
            {
                var n = robot.ReadNumbers();

                var (x, y) = (n[0], n[1]);
                var (vx, vy) = (n[2], n[3]);
                robotsPos.Add((x, y));
                robotsVel.Add((vx, vy));
            }

            return (robotsPos, robotsVel);
        }

        private IEnumerable<(int[] quadrants, int Seconds)> Move(int wide, int tall, List<(long, long)> robotsPos, List<(long, long)> robotsVel)
        {
            //var m = new char[tall, wide];
            var seconds = 0;

            var quadrantsLimits = new[] {(
                    0, (wide / 2), 0, (tall / 2))
                , ((wide / 2)+1, wide, 0, (tall / 2))
                , (0, (wide / 2), (tall / 2)+1, tall)
                , ((wide / 2) + 1, wide, (tall / 2) + 1, tall)};

            var s = 0L;
            while (true)
            {
                var cnts = new int[4];
                //for (var i = 0; i < tall; i++)
                //    for (var j = 0; j < wide; j++)
                //        m[i, j] = ' ';

                for (var r = 0; r < robotsPos.Count; r++)
                {
                    var (x, y) = robotsPos[r];
                    var (vx, vy) = robotsVel[r];
                    x = (x + vx + wide) % wide;
                    y = (y + vy + tall) % tall;
                    robotsPos[r] = (x, y);

                    var q = 0;
                    foreach (var (x1, x2, y1, y2) in quadrantsLimits)
                    {
                        if (x1 <= x && x < x2)
                            if (y1 <= y && y < y2)
                                cnts[q]++;
                        q++;
                    }
                    //m[y, x] = '#';
                }

                seconds++;
                yield return (cnts, seconds);
            }
        }

        public Solve(ITestOutputHelper output)
        {
            this.Setup(output);
        }
    }
}