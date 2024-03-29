﻿using System.Threading.Tasks;

namespace MoreAsyncLINQ
{
    internal static class ValueTasks
    {
        public static ValueTask<TResult> FromResult<TResult>(TResult result) =>
            new ValueTask<TResult>(result);
    }
}