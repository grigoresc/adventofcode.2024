using aoc.common;
using Xunit.Abstractions;

namespace day07
{
    public class Solve
    {
        [Theory]
        [InlineData("input.txt", 5030892084481, 91377448644679)]
        [InlineData(@"sample.txt", 3749, 11387)]
        public void Both(string input, long sln, long sln2)
        {
            var lines = input.ParseAsLines();
            long cnt = 0;
            long cnt2 = 0;
            foreach (var line in lines)
            {
                (long v, long[] n) = line.ReadNumbers();
                if (canCompute(v, n[0], n[1..], false))
                    cnt += v;
                if (canCompute(v, n[0], n[1..], true))
                    cnt2 += v;
            }

            cnt.Dump().AssertSolved(sln);
            cnt2.Dump().AssertSolved(sln2);
        }
        long pow(long i, int exp) => (exp == 0) ? 1 : i * pow(i, exp - 1);
        private bool canCompute(long l, long acc, long[] longs, bool use3rdOperator)
        {
            if (longs.Length == 0)
                return l == acc;
            if (use3rdOperator)
                if (canCompute(l, pow(10, longs[0].ToString().Length) * acc + longs[0], longs[1..], use3rdOperator))
                    return true;
            if (canCompute(l, acc + longs[0], longs[1..], use3rdOperator))
                return true;
            return canCompute(l, acc * longs[0], longs[1..], use3rdOperator);
        }

        public Solve(ITestOutputHelper output)
        {
            this.Setup(output);
        }

    }
}