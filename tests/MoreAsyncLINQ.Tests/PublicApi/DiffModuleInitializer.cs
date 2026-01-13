using System.Runtime.CompilerServices;
using DiffEngine;
using static DiffEngine.DiffTool;

namespace MoreAsyncLINQ.Tests;

public static class DiffModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifyDiffPlex.Initialize();
        DiffTools.UseOrder(Rider, VisualStudioCode);
    }
}
