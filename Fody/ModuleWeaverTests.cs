using System;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Pdb;
using NUnit.Framework;
[TestFixture]
public class ModuleWeaverTests
{
    string beforeAssemblyPath;
    string afterAssemblyPath;
    Assembly assembly;

    public ModuleWeaverTests()
    {
        beforeAssemblyPath = Path.GetFullPath(@"..\..\..\AssemblyToProcess\bin\Debug\AssemblyToProcess.dll");
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
                    ProjectDirectoryPath = Path.GetFullPath(@"..\..\..\AssemblyToProcess")
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
        }
    }
    [Test]
    public void AsStreamUnChecked()
    {
        var instance = GetInstance("TargetClass");
        using (var stream = (Stream)instance.WithAsStreamUnChecked())
        {
            Assert.Null(stream);
        }
    }

    public dynamic GetInstance(string className)
    {
        var type = assembly.GetType(className, true);
        return Activator.CreateInstance(type);
    }
    [Test]
    public void PeVerify()
    {
        Verifier.Validate( afterAssemblyPath);
    }
}

