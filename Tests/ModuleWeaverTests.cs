using System;
using System.IO;
using Fody;
using TestResult = Fody.TestResult;
using TestAttribute = TUnit.Core.TestAttribute;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;

// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable

public class ModuleWeaverTests
{
    static TestResult testResult;

    static ModuleWeaverTests()
    {
        var weaver = new ModuleWeaver
        {
            ProjectDirectoryPath = Path.GetFullPath(@"..\..\..\..\AssemblyToProcess\")
        };

        testResult = weaver.ExecuteTestRun(
            assemblyPath: "AssemblyToProcess.dll");
    }

    [Test]
    public async Task AsStream()
    {
        var instance = testResult.GetInstance("TargetClass");
        using (var stream = (Stream)instance.WithAsStream())
        {
            await Assert.That(stream).IsNotNull();
            using (var streamReader = new StreamReader(stream))
            {
                await Assert.That(streamReader.ReadToEnd()).IsEqualTo("contents");
            }
        }
    }

    [Test]
    public async Task AsStreamUnChecked()
    {
        var instance = testResult.GetInstance("TargetClass");
        using (var stream = (Stream)instance.WithAsStreamUnChecked("fakePath"))
        {
            await Assert.That(stream).IsNull();
        }
    }

    [Test]
    public async Task AsStreamReader()
    {
        var instance = testResult.GetInstance("TargetClass");
        using (var streamReader = (StreamReader)instance.WithAsStreamReader())
        {
            await Assert.That(streamReader).IsNotNull();
            await Assert.That(streamReader.ReadToEnd()).IsEqualTo("contents");
        }
    }

    [Test]
    public async Task AsStreamReaderUnChecked()
    {
        var instance = testResult.GetInstance("TargetClass");
        using (var streamReader = (StreamReader)instance.WithAsStreamReaderUnChecked("fakePath"))
        {
            await Assert.That(streamReader).IsNull();
        }
    }

    [Test]
    public async Task AsString()
    {
        var instance = testResult.GetInstance("TargetClass");
        var result = (string)instance.WithAsString();
        await Assert.That(result).IsNotNull();
        await Assert.That(result).IsEqualTo("contents");
    }

    [Test]
    public async Task FullyQualified()
    {
        var instance = testResult.GetInstance("TargetClass");
        var result = (string)instance.FullyQualified();
        await Assert.That(result).IsNotNull();
        await Assert.That(result).IsEqualTo("contents");
    }

    [Test]
    public async Task AsStringCustomNamespace()
    {
        var instance = testResult.GetInstance("AssemblyToProcess.CustomNamespace.TargetClass");
        var result = (string)instance.WithAsString();
        await Assert.That(result).IsNotNull();
        await Assert.That(result).IsEqualTo("contents in namespace");
    }

    [Test]
    public async Task AsStringInLinkProject()
    {
        var instance = testResult.GetInstance("TargetClassInLinkProject");
        var result = (string)instance.WithAsString();
        await Assert.That(result).IsNotNull();
        await Assert.That(result).IsEqualTo("content in link project");
    }

    [Test]
    public async Task FullyQualifiedCustomNamespace()
    {
        var instance = testResult.GetInstance("AssemblyToProcess.CustomNamespace.TargetClass");
        var result = (string)instance.FullyQualified();
        await Assert.That(result).IsNotNull();
        await Assert.That(result).IsEqualTo("contents in namespace");
    }


    [Test]
    public async Task FullyQualifiedMisMatchNamespace()
    {
        var instance = testResult.GetInstance("AssemblyToProcess.DiffNamespace.TargetClass");
        var result = (string)instance.FullyQualified();
        await Assert.That(result).IsNotNull();
        await Assert.That(result).IsEqualTo("contents in mismatch namespace");
    }

    [Test]
    public async Task MisMatchNamespace()
    {
        var instance = testResult.GetInstance("AssemblyToProcess.DiffNamespace.TargetClass");
        var result = (string)instance.WithAsString();
        await Assert.That(result).IsNotNull();
        await Assert.That(result).IsEqualTo("contents in mismatch namespace");
    }

    [Test]
    public async Task AsStringUnCheckedGoodPath()
    {
        var instance = testResult.GetInstance("TargetClass");
        var result = (string)instance.WithAsStringUnChecked("AssemblyToProcess.Resource.txt");
        await Assert.That(result).IsEqualTo("contents");
    }

    [Test]
    public async Task AsStringUnChecked()
    {
        var instance = testResult.GetInstance("TargetClass");
        var exception = await Assert.That(() => (object) instance.WithAsStringUnChecked("fakePath")).Throws<Exception>();
        await Assert.That(exception!.Message).IsEqualTo("Could not find a resource named 'fakePath'.");
    }
}