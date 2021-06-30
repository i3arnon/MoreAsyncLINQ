using System.Threading.Tasks;

namespace MoreAsyncLinq
{
    internal static class ValueTasks
    {
        public static ValueTask<TResult> FromResult<TResult>(TResult result) =>
            new ValueTask<TResult>(result);
    }
}