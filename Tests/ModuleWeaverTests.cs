using System;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Pdb;
using NUnit.Framework;

[TestFixture]
public class ModuleWeaverTests
{
    string afterAssemblyPath;
    Assembly assembly;
    string beforeAssemblyPath;

    public ModuleWeaverTests()
    {
        beforeAssemblyPath = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\AssemblyToProcess\bin\Debug\AssemblyToProcess.dll"));
#if (!DEBUG)
        beforeAssemblyPath = beforeAssemblyPath.Replace("Debug", "Release");
#endif
        afterAssemblyPath = beforeAssemblyPath.Replace(".dll", "2.dll");
        var oldpdb = beforeAssemblyPath.Replace(".dll", ".pdb");
        var newpdb = beforeAssemblyPath.Replace(".dll", "2.pdb");
        File.Copy(beforeAssemblyPath, afterAssemblyPath, true);
        File.Copy(oldpdb, newpdb, true);

        var assemblyResolver = new MockAssemblyResolver
            {
                Directory = Path.GetDirectoryName(beforeAssemblyPath)
            };

        using (var symbolStream = File.OpenRead(newpdb))
        {
            var readerParameters = new ReaderParameters
                {
                    ReadSymbols = true,
                    SymbolStream = symbolStream,
                    SymbolReaderProvider = new PdbReaderProvider()
                };
            var moduleDefinition = ModuleDefinition.ReadModule(afterAssemblyPath, readerParameters);

            var weavingTask = new ModuleWeaver
                {
                    ModuleDefinition = moduleDefinition,
                    AssemblyResolver = assemblyResolver,
                    ProjectDirectoryPath = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\AssemblyToProcess\"))
                };

            weavingTask.Execute();
            moduleDefinition.Write(afterAssemblyPath);
        }
        assembly = Assembly.LoadFile(afterAssemblyPath);
    }

    [Test]
    public void AsStream()
    {
        var instance = GetInstance("TargetClass");
        using (var stream = (Stream)instance.WithAsStream())
        {
            Assert.IsNotNull(stream);
            using (var streamReader = new StreamReader(stream))
            {
                Assert.AreEqual("contents", streamReader.ReadToEnd());
            }
        }
    }


    [Test]
    public void AsStreamUnChecked()
    {
        var instance = GetInstance("TargetClass");
        using (var stream = (Stream)instance.WithAsStreamUnChecked("fakePath"))
        {
            Assert.Null(stream);
        }
    }

    [Test]
    public void AsStreamReader()
    {
        var instance = GetInstance("TargetClass");
        using (var streamReader = (StreamReader) instance.WithAsStreamReader())
        {
            Assert.IsNotNull(streamReader);
            Assert.AreEqual("contents", streamReader.ReadToEnd());
        }
    }

    [Test]
    public void AsStreamReaderUnChecked()
    {
        var instance = GetInstance("TargetClass");
        using (var streamReader = (StreamReader)instance.WithAsStreamReaderUnChecked("fakePath"))
        {
            Assert.Null(streamReader);
        }
    }

    [Test]
    public void AsString()
    {
        var instance = GetInstance("TargetClass");
	    var result = (string) instance.WithAsString();
	    Assert.IsNotNull(result);
		Assert.AreEqual("contents", result);
    }

    [Test]
    public void FullyQualified()
    {
        var instance = GetInstance("TargetClass");
        var result = (string)instance.FullyQualified();
	    Assert.IsNotNull(result);
		Assert.AreEqual("contents", result);
    }

    [Test]
	public void AsStringCustomNamespace()
	{
		var instance = GetInstance("AssemblyToProcess.CustomNamespace.TargetClass");
	    var result = (string) instance.WithAsString();
	    Assert.IsNotNull(result);
		Assert.AreEqual("contents in namespace", result);
    }

    [Test]
    public void AsStringInLinkProject()
	{
        var instance = GetInstance("TargetClassInLinkProject");
	    var result = (string) instance.WithAsString();
	    Assert.IsNotNull(result);
        Assert.AreEqual("content in link project", result);
    }

    [Test]
    public void FullyQualifiedCustomNamespace()
	{
		var instance = GetInstance("AssemblyToProcess.CustomNamespace.TargetClass");
        var result = (string)instance.FullyQualified();
	    Assert.IsNotNull(result);
		Assert.AreEqual("contents in namespace", result);
    }


    [Test]
    public void FullyQualifiedMisMatchNamespace()
	{
        var instance = GetInstance("AssemblyToProcess.DiffNamespace.TargetClass");
        var result = (string)instance.FullyQualified();
	    Assert.IsNotNull(result);
        Assert.AreEqual("contents in mismatch namespace", result);
    }

    [Test]
	public void MisMatchNamespace()
	{
        var instance = GetInstance("AssemblyToProcess.DiffNamespace.TargetClass");
	    var result = (string) instance.WithAsString();
	    Assert.IsNotNull(result);
        Assert.AreEqual("contents in mismatch namespace", result);
    }

    [Test]
    public void AsStringUnCheckedGoodPath()
    {
        var instance = GetInstance("TargetClass");
        var result = (string)instance.WithAsStringUnChecked("AssemblyToProcess.Resource.txt");
	    Assert.AreEqual("contents", result);
    }

    [Test]
    public void AsStringUnChecked()
    {
        var instance = GetInstance("TargetClass");
		Assert.Throws<Exception>(() => instance.WithAsStringUnChecked("fakePath"), "Could not find a resource named 'fakePath'.");
    }

    dynamic GetInstance(string className)
    {
        var type = assembly.GetType(className, true);
        return Activator.CreateInstance(type);
    }

    [Test]
    public void PeVerify()
    {
        Verifier.Verify(beforeAssemblyPath, afterAssemblyPath);
    }
}