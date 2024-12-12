namespace aoc.common;

public static class MatrixExtensions
{
    public static IEnumerable<(int row, int col, T val)> Iterate<T>(this T[,] matrix)
    {
        for (int r = 0; r < matrix.GetLength(0); r++)
            for (int c = 0; c < matrix.GetLength(1); c++)
                yield return (r, c, matrix[r, c]);
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