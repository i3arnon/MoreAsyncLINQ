namespace MoreAsyncLINQ
{
    internal static class Option
    {
        public static (bool HasValue, T Value) Some<T>(T value) => (true, value);

        public static T? OrDefault<T>(this (bool HasValue, T Value) option) =>
            option is (true, var value) ? value : default;
    }
}