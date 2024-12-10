namespace aoc.common;

public class Map
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

    public static (int l, int c) Move(Dir d, (int l, int c) pos) => Move(d, pos.l, pos.c);
    public static (int l, int c) Move(Dir d, int l, int c) => d switch
    {
        Dir.N => (l - 1, j: c),
        Dir.E => (i: l, c + 1),
        Dir.S => (l + 1, j: c),
        Dir.W => (i: l, c - 1),
        _ => throw new System.Exception("Invalid direction")
    };

    public static bool IsInBounds(int i, int j, int len) => i >= 0 && i < len && j >= 0 && j < len;
    public static bool IsInBounds(int i, int j, char[,] m) => i >= 0 && i < m.GetLength(0) && j >= 0 && j < m.GetLength(1);
    public static bool IsInBounds((int i, int j) pos, char[,] m) => pos.i >= 0 && pos.i < m.GetLength(0) && pos.j >= 0 && pos.j < m.GetLength(1);
    public static bool IsInBounds((int i, int j) pos, int len) => IsInBounds(pos.i, pos.j, len);
}