using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    internal static class ValueTask
    {
        public static ValueTask<TResult> FromResult<TResult>(TResult result) =>
            new ValueTask<TResult>(result);
    }
}