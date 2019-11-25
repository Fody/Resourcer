using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

public class PathExTests :
    VerifyBase
{
    [Fact]
    public void NoTrailingSlash()
    {
        var relativePath = PathEx.MakeRelativePath(@"C:\Code\Resourcer\AssemblyToProcess", @"c:\Code\Resourcer\AssemblyToProcess\RelativePart");
        Assert.Equal("RelativePart", relativePath);
        var linuxRelativePath = PathEx.MakeRelativePath("/Code/Resourcer/AssemblyToProcess", "/Code/Resourcer/AssemblyToProcess/RelativePart");
        Assert.Equal("RelativePart", linuxRelativePath);
    }

    [Fact]
    public void SourceIncludedFromSharedLink()
    {
        var relativePath = PathEx.MakeRelativePath(@"C:\Code\Solution\Project", @"C:\Code\Solution\Common");
        Assert.Equal("", relativePath);
        var linuxRelativePath = PathEx.MakeRelativePath("/Code/Solution/Project", "/Code/Solution/Common");
        Assert.Equal("", linuxRelativePath);
    }

    [Fact]
    public void TrailingSlash()
    {
        var relativePath = PathEx.MakeRelativePath(@"C:\Code\Resourcer\AssemblyToProcess\", @"C:\Code\Resourcer\AssemblyToProcess\RelativePart\");
        Assert.Equal("RelativePart", relativePath);
        var linuxRelativePath = PathEx.MakeRelativePath("/Code/Resourcer/AssemblyToProcess/", "/Code/Resourcer/AssemblyToProcess/RelativePart/");
        Assert.Equal("RelativePart", linuxRelativePath);
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
        var linuxRelativePath1 = PathEx.MakeRelativePath("/Code/Resourcer/AssemblyToProcess/", "/Code/Resourcer/AssemblyToProcess/");
        Assert.Equal("", linuxRelativePath1);
        var linuxRelativePath2 = PathEx.MakeRelativePath("/Code/Resourcer/AssemblyToProcess/", "/Code/Resourcer/AssemblyToProcess");
        Assert.Equal("", linuxRelativePath2);
        var linuxRelativePath3 = PathEx.MakeRelativePath("/Code/Resourcer/AssemblyToProcess", "/Code/Resourcer/AssemblyToProcess/");
        Assert.Equal("", linuxRelativePath3);
    }

    [Fact]
    public void MixedSlash()
    {
        var relativePath1 = PathEx.MakeRelativePath(@"C:\Code\Resourcer\AssemblyToProcess\", @"c:\Code\Resourcer\AssemblyToProcess\RelativePart");
        Assert.Equal("RelativePart", relativePath1);
        var relativePath2 = PathEx.MakeRelativePath(@"C:\Code\Resourcer\AssemblyToProcess", @"c:\Code\Resourcer\AssemblyToProcess\RelativePart\");
        Assert.Equal("RelativePart", relativePath2);
        var linuxRelativePath1 = PathEx.MakeRelativePath("/Code/Resourcer/AssemblyToProcess/", "/Code/Resourcer/AssemblyToProcess/RelativePart");
        Assert.Equal("RelativePart", linuxRelativePath1);
        var linuxRelativePath2 = PathEx.MakeRelativePath("/Code/Resourcer/AssemblyToProcess", "/Code/Resourcer/AssemblyToProcess/RelativePart/");
        Assert.Equal("RelativePart", linuxRelativePath2);
    }

    public PathExTests(ITestOutputHelper output) : 
        base(output)
    {
    }
}