using Xunit;

public class PathExTests
{
    [Fact]
    public void NoTrailingSlash()
    {
        var relativePath = PathEx.MakeRelativePath(@"C:\Code\Resourcer\AssemblyToProcess", @"c:\Code\Resourcer\AssemblyToProcess\RelativePart");
        Assert.Equal("RelativePart", relativePath);
    }

    [Fact]
    public void SourceIncludedFromSharedLink()
    {
        var relativePath = PathEx.MakeRelativePath(@"C:\Code\Solution\Project", @"C:\Code\Solution\Common");
        Assert.Equal("", relativePath);
    }

    [Fact]
    public void TrailingSlash()
    {
        var relativePath = PathEx.MakeRelativePath(@"C:\Code\Resourcer\AssemblyToProcess\", @"C:\Code\Resourcer\AssemblyToProcess\RelativePart\");
        Assert.Equal("RelativePart", relativePath);
    }

    [Fact]
    public void Same()
    {
        var relativePath1 = PathEx.MakeRelativePath(@"C:\Code\Resourcer\AssemblyToProcess\", @"C:\Code\Resourcer\AssemblyToProcess\");
        Assert.Equal("", relativePath1);
        var relativePath2 = PathEx.MakeRelativePath(@"C:\Code\Resourcer\AssemblyToProcess\", @"C:\Code\Resourcer\AssemblyToProcess");
        Assert.Equal("", relativePath2);
        var relativePath3 = PathEx.MakeRelativePath(@"C:\Code\Resourcer\AssemblyToProcess", @"C:\Code\Resourcer\AssemblyToProcess\");
        Assert.Equal("", relativePath3);
    }

    [Fact]
    public void MixedSlash()
    {
        var relativePath1 = PathEx.MakeRelativePath(@"C:\Code\Resourcer\AssemblyToProcess\", @"c:\Code\Resourcer\AssemblyToProcess\RelativePart");
        Assert.Equal("RelativePart", relativePath1);
        var relativePath2 = PathEx.MakeRelativePath(@"C:\Code\Resourcer\AssemblyToProcess", @"c:\Code\Resourcer\AssemblyToProcess\RelativePart\");
        Assert.Equal("RelativePart", relativePath2);
    }
}