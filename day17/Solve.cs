using aoc.common;
using Xunit.Abstractions;

namespace day17
{
    public class Solve
    {
        [Theory]
        [InlineData("sample.txt", "4,6,3,5,6,3,5,2,1,0")]
        //[InlineData("sample2.txt", "0,3,5,4,3,0")]
        [InlineData("input.txt", "3,4,3,1,7,6,5,6,0")]
        public void Part1(string input, string sln)
        {
            var (regA, regB, regC, instructions) = Parse(input);
            var (output, _) = Run(instructions, regA, regB, regC);
            var ret = string.Join(",", output);
            ret.Dump().AssertSolved(sln);
        }

        [Theory]
        [InlineData("sample2.txt", 117440)]
        [InlineData("input.txt", 109019930331546)]
        public void Part2(string input, long sln)
        {
            var (_, _, _, instructions) = Parse(input);

            var aSln = new List<long>();
            aSln.Add(0);

            for (var pos = instructions.Length - 1; pos >= 0; pos--)
            {
                var aLst = aSln.ToArray();
                aSln.Clear();
                foreach (var a in aLst)
                    for (var remainder = 0; remainder < 8; remainder++)
                    {
                        var tA = a * 8 + remainder;
                        var (output, _) = Run(instructions, tA, 0, 0);
                        if (instructions[pos..].SequenceEqual(output))
                        {
                            aSln.Add(tA);
                        }
                    }
            }
            aSln.Min().Dump().AssertSolved(sln);

        }
        private static (long regA, long regB, long regC, long[] instructions) Parse(string input)
        {
            var program = input.ParseAsChunkOfLines();
            var registries = program[0].ReadNumbers();
            var (regA, regB, regC) = (registries[0], registries[1], registries[2]);
            var instructions = program[1].ReadNumbers();
            return (regA, regB, regC, instructions);
        }

        private (long[], long) Run(long[] instructions, long regA, long regB, long regC)
        {
            var i = 0;
            var output = new List<long>();
            while (i < instructions.Length - 1)
            {
                var (ins, op) = (instructions[i], instructions[i + 1]);
                var combo = op switch
                {
                    0 or 1 or 2 or 3 => op,
                    4 => regA,
                    5 => regB,
                    6 => regC,
                    _ => 11111111//throw new Exception($"op not supported - {op}")
                };

                var isjnz = false;
                switch (ins)
                {
                    //adv
                    case 0: regA = regA / pow2(combo); break;
                    //bxl
                    case 1: regB = regB ^ op; break;
                    //bst
                    case 2: regB = combo % 8; break;
                    //jnz
                    case 3: if (regA != 0) { i = (int)op; isjnz = true; } break;
                    //bxc
                    case 4: regB = regB ^ regC; break;
                    //out
                    case 5: output.Add(combo % 8); break;
                    //bdv
                    case 6: regB = regA / pow2(combo); break;
                    //cdv
                    case 7: regC = regA / pow2(combo); break;
                }
                // 2,4, bst 
                // 1,5, bxl 
                // 7,5, cdv
                // 4,5, bxc
                // 0,3, adv
                // 1,6, bxl
                // 5,5, out
                // 3,0 go to start is a>0 

                if (!isjnz)
                    i += 2;

            }

            return (output.ToArray(), regA);
            //output.ToArray().Dump();// "output:");
        }

        private long pow2(long val)
        {
            var ret = 1;
            for (int i = 0; i < val; i++)
            {
                ret *= 2;
            }
            return ret;
        }

        public Solve(ITestOutputHelper output)
        {
            this.Setup(output);
        }
    }
}