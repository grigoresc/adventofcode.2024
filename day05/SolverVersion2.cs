using aoc.common;
using Xunit.Abstractions;

namespace day05
{
    public class SolverVersion2
    {
        public class PageComparer(HashSet<Tuple<long, long>> order) : IComparer<long>
        {
            public int Compare(long x, long y)
            {
                if (order.Contains(new(y, x)))
                {
                    return 1;
                }

                return -1;
            }
        }

        [Theory]
        [InlineData("input.txt", 5732, 4716)]
        [InlineData("sample.txt", 143, 123)]
        public void Both(string input, long sln, long sln2)
        {
            var li = input.ParseAsChunkOfLines();
            var rules = li[0].ParseAsLines();
            var updates = li[1].ParseAsLines();

            var ord = new HashSet<Tuple<long, long>>();
            foreach (var pair in rules)
            {
                var p = pair.ReadNumbers();
                ord.Add(new(p[0], p[1]));
            }

            var res = 0L;
            var res2 = 0L;
            var comparer = new PageComparer(ord);
            foreach (var update in updates)
            {
                var seq = update.ReadNumbers().ToList();
                var seqOrig = update.ReadNumbers().ToList();

                seq.Sort(comparer);

                var ok = seq.SequenceEqual(seqOrig);

                int midIndex = seq.Count / 2;
                if (ok)
                {
                    res += seqOrig[midIndex];
                }
                else
                {
                    res2 += seq[midIndex];
                }
            }

            res.Dump().AssertSolved(sln);
            res2.Dump().AssertSolved(sln2);
        }

        public SolverVersion2(ITestOutputHelper output)
        {
            this.Setup(output);
        }

    }
}