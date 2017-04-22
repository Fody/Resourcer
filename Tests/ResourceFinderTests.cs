using System.IO;
using Mono.Cecil;
using NUnit.Framework;

[TestFixture]
public class ResourceFinderTests
{

    [Test]
    public void FullyQualified()
    {
        var expected = new EmbeddedResource("AssemblyName.Namespace1.ResourceName", ManifestResourceAttributes.Public, (Stream) null);
        var definition = ModuleDefinition.CreateModule("AssemblyName", ModuleKind.Dll);
        definition.Resources.Add(expected);
        var moduleWeaver = new ModuleWeaver
                           {
                               ModuleDefinition = definition
                           };
        var actual = moduleWeaver.FindResource("AssemblyName.Namespace1.ResourceName", null, null, null, null);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void RelativeBasedOnNamespace()
    {
        var expected = new EmbeddedResource("AssemblyName.Namespace1.ResourceName", ManifestResourceAttributes.Public, (Stream) null);
        var definition = ModuleDefinition.CreateModule("AssemblyName", ModuleKind.Dll);
        definition.Resources.Add(expected);
        var moduleWeaver = new ModuleWeaver
                           {
                               ModuleDefinition = definition
                           };
        var actual = moduleWeaver.FindResource("ResourceName", "AssemblyName.Namespace1", null, null, null);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void RelativeBasedOnDir()
    {
        var expected = new EmbeddedResource("AssemblyName.Namespace1.ResourceName", ManifestResourceAttributes.Public, (Stream) null);
        var definition = ModuleDefinition.CreateModule("AssemblyName", ModuleKind.Dll);
        definition.Resources.Add(expected);
        var moduleWeaver = new ModuleWeaver
                           {
                               ModuleDefinition = definition
                           };
        var actual = moduleWeaver.FindResource("ResourceName", "BadPrefix", @"Namespace1", null, null);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void RelativeBasedOnDirUpOneLevel()
    {
        var expected = new EmbeddedResource("AssemblyName.ResourceName", ManifestResourceAttributes.Public, (Stream) null);
        var definition = ModuleDefinition.CreateModule("AssemblyName", ModuleKind.Dll);
        definition.Resources.Add(expected);
        var moduleWeaver = new ModuleWeaver
                           {
                               ModuleDefinition = definition
                           };
        var actual = moduleWeaver.FindResource(@"..\ResourceName", "BadPrefix", @"Namespace1", null, null);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void RelativeBasedOnDirUpOneLevelMultipleNamespaces()
    {
        var expected = new EmbeddedResource("AssemblyName.Namespace1.ResourceName", ManifestResourceAttributes.Public, (Stream) null);
        var definition = ModuleDefinition.CreateModule("AssemblyName", ModuleKind.Dll);
        definition.Resources.Add(expected);
        var moduleWeaver = new ModuleWeaver
                           {
                               ModuleDefinition = definition
                           };
        var actual = moduleWeaver.FindResource(@"..\ResourceName", "BadPrefix", @"Namespace1\Namespace2", null, null);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void RelativeBasedOnDirUpTwoLevelsMultipleNamespaces()
    {
        var expected = new EmbeddedResource("AssemblyName.ResourceName", ManifestResourceAttributes.Public, (Stream) null);
        var definition = ModuleDefinition.CreateModule("AssemblyName", ModuleKind.Dll);
        definition.Resources.Add(expected);
        var moduleWeaver = new ModuleWeaver
                           {
                               ModuleDefinition = definition
                           };
        var actual = moduleWeaver.FindResource(@"..\..\ResourceName", "BadPrefix", @"Namespace1\Namespace2", null, null);
        Assert.AreEqual(expected, actual);
    }
}