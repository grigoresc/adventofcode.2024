namespace aoc.common
{
    public static class ArrayExtensions
    {
        public static void Deconstruct<T>(this T[] arr, out T first, out T[] rest)
        {
            first = arr.Length > 0 ? arr[0] : default(T);
            rest = arr[1..];
        }
    }
}
