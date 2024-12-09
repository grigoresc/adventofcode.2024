using aoc.common;
using Xunit.Abstractions;

namespace day09
{
    public class Solve
    {
        [Theory]
        [InlineData("input.txt", 6463499258318, 927)]
        [InlineData("sample.txt", 1928, 34)]
        public void Both(string input, long sln1, long sln2)
        {
            var map = input.ParseAsLines()[0].Select(c => long.Parse(c.ToString())).ToArray();
            map.Dump();
            var cnt1 = 0L;
            cnt1 = Defrag(map).Sum();
            cnt1.Dump().AssertSolved(sln1);
        }

        private static IEnumerable<long> Defrag(long[] map)
        {
            var indexToMove = map.Length / 2;
            var blockSizeToMove = map[indexToMove * 2];
            //var freeSizeToMove = map[indexToMove * 2 + 1];

            var len = 0L;
            var i = 0;
            while (i <= indexToMove)
            {
                var blockSize = map[i * 2];
                for (var bi = 0; bi < blockSize; bi++)
                {
                    //i.Dump(">");
                    yield return len * i;
                    len++;
                }
                if (i == indexToMove)
                    break;

                var freeSize = map[i * 2 + 1];
                while (freeSize > 0)
                {
                    var use = Math.Min(freeSize, blockSizeToMove);
                    freeSize -= use;
                    blockSizeToMove -= use;
                    map[indexToMove * 2] -= use;

                    for (var bi = 0; bi < use; bi++)
                    {
                        yield return len * indexToMove;
                        //indexToMove.Dump("!");
                        len++;
                    }

                    if (blockSizeToMove == 0)
                    {
                        indexToMove--;
                        blockSizeToMove = map[indexToMove * 2];
                        //freeSizeToMove = map[indexToMove * 2 + 1];
                    }
                }

                i++;
                //blockSize.Dump();
            }



            //i.Dump("final i");
            indexToMove.Dump("final indexToMove");
        }

        public Solve(ITestOutputHelper output)
        {
            this.Setup(output);
        }

    }
}