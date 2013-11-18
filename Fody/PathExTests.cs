using NUnit.Framework;

[TestFixture]
public class PathExTests
{
    [Test]
    public void NoTrailingSlash()
    {
        var relativePath = PathEx.MakeRelativePath(@"C:\Code\Resourcer\AssemblyToProcess", @"c:\Code\Resourcer\AssemblyToProcess\RelativePart");
        Assert.AreEqual("RelativePart", relativePath);
    }
    [Test]
    public void TrailingSlash()
    {
        var relativePath = PathEx.MakeRelativePath(@"C:\Code\Resourcer\AssemblyToProcess\", @"C:\Code\Resourcer\AssemblyToProcess\RelativePart\");
        Assert.AreEqual("RelativePart", relativePath);
    }
    [Test]
    public void Same()
    {
        var relativePath1 = PathEx.MakeRelativePath(@"C:\Code\Resourcer\AssemblyToProcess\", @"C:\Code\Resourcer\AssemblyToProcess\");
        Assert.AreEqual(@"\", relativePath1);
        var relativePath2 = PathEx.MakeRelativePath(@"C:\Code\Resourcer\AssemblyToProcess\", @"C:\Code\Resourcer\AssemblyToProcess");
        Assert.AreEqual(@"\", relativePath2);
        var relativePath3 = PathEx.MakeRelativePath(@"C:\Code\Resourcer\AssemblyToProcess", @"C:\Code\Resourcer\AssemblyToProcess\");
        Assert.AreEqual(@"\", relativePath3);

    }
    [Test]
    public void MixedSlash()
    {
        var relativePath1 = PathEx.MakeRelativePath(@"C:\Code\Resourcer\AssemblyToProcess\", @"c:\Code\Resourcer\AssemblyToProcess\RelativePart");
        Assert.AreEqual("RelativePart", relativePath1);
        var relativePath2 = PathEx.MakeRelativePath(@"C:\Code\Resourcer\AssemblyToProcess", @"c:\Code\Resourcer\AssemblyToProcess\RelativePart\");
        Assert.AreEqual("RelativePart", relativePath2);
    }

}