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
            var program = input.ParseAsChunkOfLines();
            var registries = program[0].ReadNumbers();
            var (regA, regB, regC) = (registries[0], registries[1], registries[2]);
            //(regA, regB, regC).Dump();
            var instructions = program[1].ReadNumbers();
            //instructions.Dump();
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
                    _ => throw new Exception($"op not supported - {op}")
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
                if (!isjnz)
                    i += 2;

            }
            var ret = string.Join(",", output);
            ret.Dump().AssertSolved(sln);
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