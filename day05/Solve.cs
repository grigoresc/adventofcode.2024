using aoc.common;
using Xunit.Abstractions;

namespace day05
{
    public class Solve
    {
        [Theory]
        [InlineData("input.txt", 5732, 4716)]
        [InlineData(@"sample.txt", 143, 123)]
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
            foreach (var update in updates)
            {
                var seq = update.ReadNumbers();
                var ok = true;
                for (int i = 0; i < seq.Length; i++)
                {
                    var v = seq[i];
                    for (int j = i; j < seq.Length; j++)
                    {
                        var c = seq[j];
                        if (ord.Contains(new(c, v)))
                        {
                            ok = false;
                            seq[i] = c;
                            seq[j] = v;
                            //restart comparing for same index i
                            i = -1;
                            break;
                        }
                    }
                }

                int middleIndex = seq.Length / 2;
                if (ok)
                {
                    res += seq[middleIndex];
                }
                else
                {
                    res2 += seq[middleIndex];
                }
            }

            res.Dump().AssertSolved(sln);
            res2.Dump().AssertSolved(sln2);
        }

        public Solve(ITestOutputHelper output)
        {
            this.Setup(output);
        }

    }
}