using System;
using System.IO;
using Fody;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable

public class ModuleWeaverTests :
    XunitApprovalBase
{
    static TestResult testResult;

    static ModuleWeaverTests()
    {
        var weavingTask = new ModuleWeaver
        {
            ProjectDirectoryPath = Path.GetFullPath(@"..\..\..\..\AssemblyToProcess\")
        };

        testResult = weavingTask.ExecuteTestRun(
            assemblyPath: "AssemblyToProcess.dll");
    }

    [Fact]
    public void AsStream()
    {
        var instance = testResult.GetInstance("TargetClass");
        using (var stream = (Stream)instance.WithAsStream())
        {
            Assert.NotNull(stream);
            using (var streamReader = new StreamReader(stream))
            {
                Assert.Equal("contents", streamReader.ReadToEnd());
            }
        }
    }

    [Fact]
    public void AsStreamUnChecked()
    {
        var instance = testResult.GetInstance("TargetClass");
        using (var stream = (Stream)instance.WithAsStreamUnChecked("fakePath"))
        {
            Assert.Null(stream);
        }
    }

    [Fact]
    public void AsStreamReader()
    {
        var instance = testResult.GetInstance("TargetClass");
        using (var streamReader = (StreamReader)instance.WithAsStreamReader())
        {
            Assert.NotNull(streamReader);
            Assert.Equal("contents", streamReader.ReadToEnd());
        }
    }

    [Fact]
    public void AsStreamReaderUnChecked()
    {
        var instance = testResult.GetInstance("TargetClass");
        using (var streamReader = (StreamReader)instance.WithAsStreamReaderUnChecked("fakePath"))
        {
            Assert.Null(streamReader);
        }
    }

    [Fact]
    public void AsString()
    {
        var instance = testResult.GetInstance("TargetClass");
        var result = (string)instance.WithAsString();
        Assert.NotNull(result);
        Assert.Equal("contents", result);
    }

    [Fact]
    public void FullyQualified()
    {
        var instance = testResult.GetInstance("TargetClass");
        var result = (string)instance.FullyQualified();
        Assert.NotNull(result);
        Assert.Equal("contents", result);
    }

    [Fact]
    public void AsStringCustomNamespace()
    {
        var instance = testResult.GetInstance("AssemblyToProcess.CustomNamespace.TargetClass");
        var result = (string)instance.WithAsString();
        Assert.NotNull(result);
        Assert.Equal("contents in namespace", result);
    }

    [Fact]
    public void AsStringInLinkProject()
    {
        var instance = testResult.GetInstance("TargetClassInLinkProject");
        var result = (string)instance.WithAsString();
        Assert.NotNull(result);
        Assert.Equal("content in link project", result);
    }

    [Fact]
    public void FullyQualifiedCustomNamespace()
    {
        var instance = testResult.GetInstance("AssemblyToProcess.CustomNamespace.TargetClass");
        var result = (string)instance.FullyQualified();
        Assert.NotNull(result);
        Assert.Equal("contents in namespace", result);
    }


    [Fact]
    public void FullyQualifiedMisMatchNamespace()
    {
        var instance = testResult.GetInstance("AssemblyToProcess.DiffNamespace.TargetClass");
        var result = (string)instance.FullyQualified();
        Assert.NotNull(result);
        Assert.Equal("contents in mismatch namespace", result);
    }

    [Fact]
    public void MisMatchNamespace()
    {
        var instance = testResult.GetInstance("AssemblyToProcess.DiffNamespace.TargetClass");
        var result = (string)instance.WithAsString();
        Assert.NotNull(result);
        Assert.Equal("contents in mismatch namespace", result);
    }

    [Fact]
    public void AsStringUnCheckedGoodPath()
    {
        var instance = testResult.GetInstance("TargetClass");
        var result = (string)instance.WithAsStringUnChecked("AssemblyToProcess.Resource.txt");
        Assert.Equal("contents", result);
    }

    [Fact]
    public void AsStringUnChecked()
    {
        var instance = testResult.GetInstance("TargetClass");
        var exception = Assert.Throws<Exception>(() => instance.WithAsStringUnChecked("fakePath"));
        Assert.Equal("Could not find a resource named 'fakePath'.", exception.Message);
    }

    public ModuleWeaverTests(ITestOutputHelper output) :
        base(output)
    {
    }
}