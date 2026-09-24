using TUnit.Core;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;

public class PathExTests
{
    [Test]
    public async Task NoTrailingSlash()
    {
        var relativePath = PathEx.MakeRelativePath(@"C:\Code\Resourcer\AssemblyToProcess", @"c:\Code\Resourcer\AssemblyToProcess\RelativePart");
        await Assert.That(relativePath).IsEqualTo("RelativePart");
        var linuxRelativePath = PathEx.MakeRelativePath("/Code/Resourcer/AssemblyToProcess", "/Code/Resourcer/AssemblyToProcess/RelativePart");
        await Assert.That(linuxRelativePath).IsEqualTo("RelativePart");
    }

    [Test]
    public async Task SourceIncludedFromSharedLink()
    {
        var relativePath = PathEx.MakeRelativePath(@"C:\Code\Solution\Project", @"C:\Code\Solution\Common");
        await Assert.That(relativePath).IsEqualTo("");
        var linuxRelativePath = PathEx.MakeRelativePath("/Code/Solution/Project", "/Code/Solution/Common");
        await Assert.That(linuxRelativePath).IsEqualTo("");
    }

    [Test]
    public async Task TrailingSlash()
    {
        var relativePath = PathEx.MakeRelativePath(@"C:\Code\Resourcer\AssemblyToProcess\", @"C:\Code\Resourcer\AssemblyToProcess\RelativePart\");
        await Assert.That(relativePath).IsEqualTo("RelativePart");
        var linuxRelativePath = PathEx.MakeRelativePath("/Code/Resourcer/AssemblyToProcess/", "/Code/Resourcer/AssemblyToProcess/RelativePart/");
        await Assert.That(linuxRelativePath).IsEqualTo("RelativePart");
    }

    [Test]
    public async Task Same()
    {
        var relativePath1 = PathEx.MakeRelativePath(@"C:\Code\Resourcer\AssemblyToProcess\", @"C:\Code\Resourcer\AssemblyToProcess\");
        await Assert.That(relativePath1).IsEqualTo("");
        var relativePath2 = PathEx.MakeRelativePath(@"C:\Code\Resourcer\AssemblyToProcess\", @"C:\Code\Resourcer\AssemblyToProcess");
        await Assert.That(relativePath2).IsEqualTo("");
        var relativePath3 = PathEx.MakeRelativePath(@"C:\Code\Resourcer\AssemblyToProcess", @"C:\Code\Resourcer\AssemblyToProcess\");
        await Assert.That(relativePath3).IsEqualTo("");
        var linuxRelativePath1 = PathEx.MakeRelativePath("/Code/Resourcer/AssemblyToProcess/", "/Code/Resourcer/AssemblyToProcess/");
        await Assert.That(linuxRelativePath1).IsEqualTo("");
        var linuxRelativePath2 = PathEx.MakeRelativePath("/Code/Resourcer/AssemblyToProcess/", "/Code/Resourcer/AssemblyToProcess");
        await Assert.That(linuxRelativePath2).IsEqualTo("");
        var linuxRelativePath3 = PathEx.MakeRelativePath("/Code/Resourcer/AssemblyToProcess", "/Code/Resourcer/AssemblyToProcess/");
        await Assert.That(linuxRelativePath3).IsEqualTo("");
    }

    [Test]
    public async Task MixedSlash()
    {
        var relativePath1 = PathEx.MakeRelativePath(@"C:\Code\Resourcer\AssemblyToProcess\", @"c:\Code\Resourcer\AssemblyToProcess\RelativePart");
        await Assert.That(relativePath1).IsEqualTo("RelativePart");
        var relativePath2 = PathEx.MakeRelativePath(@"C:\Code\Resourcer\AssemblyToProcess", @"c:\Code\Resourcer\AssemblyToProcess\RelativePart\");
        await Assert.That(relativePath2).IsEqualTo("RelativePart");
        var linuxRelativePath1 = PathEx.MakeRelativePath("/Code/Resourcer/AssemblyToProcess/", "/Code/Resourcer/AssemblyToProcess/RelativePart");
        await Assert.That(linuxRelativePath1).IsEqualTo("RelativePart");
        var linuxRelativePath2 = PathEx.MakeRelativePath("/Code/Resourcer/AssemblyToProcess", "/Code/Resourcer/AssemblyToProcess/RelativePart/");
        await Assert.That(linuxRelativePath2).IsEqualTo("RelativePart");
    }
}