using Xunit;
using Xunit.Abstractions;
namespace aoc.common;
public static class Outputs
{
    static ITestOutputHelper output;
    public static void Setup<T>(this T o, ITestOutputHelper output)
    {
        Outputs.output = output;
    }

    static Action<string> printer
    {
        get
        {
            if (output != null)
                return output.WriteLine;
            else
                return Console.WriteLine;
        }
    }
    public static void DumpMap(this string[] map, string? title = "")
    {
        Prints.printItem($"~~~~~~~~~~~~~~{title}~~~~~~~~~~~~~~", printer);
        foreach (var line in map)
        {
            Prints.printItem(new string(line), printer);
        }
    }
    public static void Dump(this char[][] map, string? title = "")
    {
        Prints.printItem($"~~~~~~~~~~~~~~{title}~~~~~~~~~~~~~~", printer);
        foreach (var line in map)
        {
            Prints.printItem(new string(line), printer);
        }
    }
    public static void Dump(this char[,] map, string? title = "")
    {
        Prints.printItem($"~~~~~~~~~~~~~~{title}~~~~~~~~~~~~~~", printer);
        for (int i = 0; i < map.GetLength(0); i++)
        {
            var l = "";
            for (int j = 0; j < map.GetLength(1); j++)
                l += map[i, j].ToString();
            printer(l);
        }
    }
    public static object Dump<K, V>(this Dictionary<K, V> dict, string? expl = null)
    {
        if (!string.IsNullOrEmpty(expl))
            Prints.printItem($"{expl}", printer);
        Prints.print(dict, printer);

        return dict;
    }

    public static object Dump<T>(this T[][] o, string? expl = null)
    {
        if (!string.IsNullOrEmpty(expl))
            Prints.printItem($"{expl}", printer);
        Prints.print(o, printer);

        return o;
    }

    public static object Dump<T>(this T[][][] o, string? expl = null)
    {
        if (!string.IsNullOrEmpty(expl))
            Prints.printItem($"{expl}", printer);
        Prints.print(o, printer);
        return o;
    }

    public static T[] Dump<T>(this T[] o, string? expl = null)
    {
        if (!string.IsNullOrEmpty(expl))
            Prints.printItem($"{expl}", printer);
        Prints.print(o, printer);
        return o;
    }

    public static T Dump<T>(this T o, string? expl = null)
    {
        if (!string.IsNullOrEmpty(expl))
            Prints.printItem($"{expl} {o}", printer);
        else
        {
            Prints.printItem(o, printer);
        }
        return o;
    }
    public static void Draw(this IEnumerable<(long, long)> coords, int size)
    {
        var m = new char[size, size];
        for (var i = 0; i < size; i++)
            for (var j = 0; j < size; j++)
                m[i, j] = ' ';
        foreach (var (x, y) in coords)
            m[y, x] = '#';
        m.Dump();
    }
    public static T AssertSolved<T>(this T o, T expected)
    {
        Assert.Equal(expected, o);
        return o;
    }
}

