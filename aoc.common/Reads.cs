using System.Text.RegularExpressions;


namespace aoc.common
{
    public static class Reads
    {
        public static string[] ParseAsLines(this string input)
        {
            if (input.Contains(".txt"))//todo another way of dealing this?
                input = File.ReadAllText(input);
            var inputs = input.Split("\r\n");
            return inputs;
        }
        public static string[] ParseAsChunkOfLines(this string input)
        {
            if (input.Contains(".txt"))//todo another way of dealing this?
                input = File.ReadAllText(input);
            var inputs = input.Split("\r\n\r\n");
            return inputs;
        }

        public static string[] ReadTokens(string line, string splitPattern)
        {
            return Regex.Replace(line, splitPattern, " ").Trim().Split(' ');
        }

        public static long[] ReadNumbers(this string line)
        {
            return ReadTokens(line, @"[^\-\d]+").Select(long.Parse).ToArray();
        }

        public static string[] ReadNonNumbers(string line)
        {
            return ReadTokens(line, @"[\d]+");
        }

        public static long ReadNumber(string line)
        {
            return ReadTokens(line, @"[^\-\d]+").Select(long.Parse).First();
        }

        public static long[][] ReadMatrixOfNumbers(string[] lines)
        {
            return lines.Select(ReadNumbers).ToArray();
        }
        public static char[,] ToCharMatrix(this string[] lines)
        {
            int rows = lines.Length;
            int cols = lines[0].Length;
            char[,] matrix = new char[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = lines[i][j];
                }
            }

            return matrix;
        }
    }
}
