using aoc.common;
using Xunit.Abstractions;

namespace _2024
{
    public class Day02
    {
        [Theory]
        [InlineData("Day02.txt", 218, 290)]
        [InlineData(@"7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9", 2, 4)]
        public void Solve(string input, int sln1, int sln2)
        {
            var lines = input.ParseAsLines();

            var cnt = 0;
            var cnt2 = 0;
            foreach (var line in lines)
            {
                var vals = line.Split(' ').Select(int.Parse).ToArray();
                if (ReportIsSafe(vals))
                {
                    cnt++;
                    cnt2++;
                }
                else
                {
                    for (int i = 0; i < vals.Count(); i++)
                    {
                        if (ReportIsSafe(vals.Take(i).Concat(vals.Skip(i + 1)).ToArray()))
                        {
                            cnt2++;
                            break;
                        }
                    }

                }

            }

            cnt.Dump().AssertSolved(sln1);
            cnt2.Dump().AssertSolved(sln2);
        }

        static bool CheckRules(int[] vals, int inc, int diffsOk)
        {
            return diffsOk == vals.Count() - 1 && Math.Abs(inc) == vals.Count() - 1;
        }

        static bool ReportIsSafe(int[] vals)
        {
            var inc = 0;
            var diffsOk = 0;
            for (int i = 1; i < vals.Count(); i++)
            {
                var diff = vals[i] - vals[i - 1];
                if (diff > 0)
                    inc++;
                else
                    inc--;
                if (1 <= Math.Abs(diff) && Math.Abs(diff) <= 3)
                {
                    diffsOk++;
                }
            }
            return CheckRules(vals, inc, diffsOk);
        }

        public Day02(ITestOutputHelper output)
        {
            this.Setup<Day02>(output);
        }

    }
}
