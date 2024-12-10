namespace aoc.common;

public class Matrix
{
    public enum Dir { N, E, S, W }
    public static Dir TurnRight(Dir d) => d switch
    {
        Dir.N => Dir.E,
        Dir.E => Dir.S,
        Dir.S => Dir.W,
        Dir.W => Dir.N,
        _ => throw new System.Exception("Invalid direction")
    };

    public static (int, int) Move(Dir d, int i, int j) => d switch
    {
        Dir.N => (i - 1, j),
        Dir.E => (i, j + 1),
        Dir.S => (i + 1, j),
        Dir.W => (i, j - 1),
        _ => throw new System.Exception("Invalid direction")
    };

    public static bool IsInBounds(int i, int j, int len) => i >= 0 && i < len && j >= 0 && j < len;
}