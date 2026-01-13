using PublicApiGenerator;

namespace MoreAsyncLINQ.Tests;

public class PublicApiTests
{
    [Fact]
    public Task VerifyPublicApi()
    {
        var publicApi = typeof(MoreAsyncEnumerable).Assembly.GeneratePublicApi(new ApiGeneratorOptions { IncludeAssemblyAttributes = false });
        
        return Verify(publicApi).UseFileName(nameof(MoreAsyncLINQ));
    }
}