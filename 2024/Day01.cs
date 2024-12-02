using aoc.common;
using Xunit.Abstractions;

namespace _2024
{
    public class Day01
    {
        [Theory]
        [InlineData(@"3   4
4   3
2   5
1   3
3   9
3   3", 11, 31)]
        [InlineData("Day01.txt", 3508942, 26593248)]
        public void Solve(string input, int sln1, int sln2)
        {
            var lines = input.ParseAsLines();

            var k = lines.Select(x => int.Parse(x.Split("   ")[0])).ToList();
            var v = lines.Select(x => int.Parse(x.Split("   ")[1])).ToList();

            k.Sort();
            v.Sort();

            var sum = 0;
            for (int i = 0; i < k.Count; i++)
            {
                sum += Math.Abs(k[i] - v[i]);
            }

            sum.Dump().AssertSolved(sln1);

            //calc freq in v list
            var freq = v.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            var sum2 = 0;

            for (int i = 0; i < v.Count; i++)
            {
                if (freq.ContainsKey(k[i]))
                {
                    sum2 += k[i] * freq[k[i]];
                }
            }

            sum2.Dump().AssertSolved(sln2);
        }

        public Day01(ITestOutputHelper output)
        {
            this.Setup(output);
        }

    }
}
