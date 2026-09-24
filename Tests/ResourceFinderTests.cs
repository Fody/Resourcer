using System.IO;
using Mono.Cecil;
using TUnit.Core;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;

public class ResourceFinderTests
{
    [Test]
    public async Task FullyQualified()
    {
        var expected = new EmbeddedResource("AssemblyName.Namespace1.ResourceName", ManifestResourceAttributes.Public, (Stream)null);
        var definition = ModuleDefinition.CreateModule("AssemblyName", ModuleKind.Dll);
        definition.Resources.Add(expected);
        var weaver = new ModuleWeaver
        {
            ModuleDefinition = definition
        };
        var actual = weaver.FindResource("AssemblyName.Namespace1.ResourceName", null, null, null, null);
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task RelativeBasedOnNamespace()
    {
        var expected = new EmbeddedResource("AssemblyName.Namespace1.ResourceName", ManifestResourceAttributes.Public, (Stream)null);
        var definition = ModuleDefinition.CreateModule("AssemblyName", ModuleKind.Dll);
        definition.Resources.Add(expected);
        var weaver = new ModuleWeaver
        {
            ModuleDefinition = definition
        };
        var actual = weaver.FindResource("ResourceName", "AssemblyName.Namespace1", null, null, null);
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task RelativeBasedOnDir()
    {
        var expected = new EmbeddedResource("AssemblyName.Namespace1.ResourceName", ManifestResourceAttributes.Public, (Stream)null);
        var definition = ModuleDefinition.CreateModule("AssemblyName", ModuleKind.Dll);
        definition.Resources.Add(expected);
        var weaver = new ModuleWeaver
        {
            ModuleDefinition = definition
        };
        var actual = weaver.FindResource("ResourceName", "BadPrefix", "Namespace1", null, null);
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task RelativeBasedOnDirUpOneLevel()
    {
        var expected = new EmbeddedResource("AssemblyName.ResourceName", ManifestResourceAttributes.Public, (Stream)null);
        var definition = ModuleDefinition.CreateModule("AssemblyName", ModuleKind.Dll);
        definition.Resources.Add(expected);
        var weaver = new ModuleWeaver
        {
            ModuleDefinition = definition
        };
        var actual = weaver.FindResource(@"..\ResourceName", "BadPrefix", "Namespace1", null, null);
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task RelativeBasedOnDirUpOneLevelMultipleNamespaces()
    {
        var expected = new EmbeddedResource("AssemblyName.Namespace1.ResourceName", ManifestResourceAttributes.Public, (Stream)null);
        var definition = ModuleDefinition.CreateModule("AssemblyName", ModuleKind.Dll);
        definition.Resources.Add(expected);
        var weaver = new ModuleWeaver
        {
            ModuleDefinition = definition
        };
        var actual = weaver.FindResource(@"..\ResourceName", "BadPrefix", @"Namespace1\Namespace2", null, null);
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task RelativeBasedOnDirUpTwoLevelsMultipleNamespaces()
    {
        var expected = new EmbeddedResource("AssemblyName.ResourceName", ManifestResourceAttributes.Public, (Stream)null);
        var definition = ModuleDefinition.CreateModule("AssemblyName", ModuleKind.Dll);
        definition.Resources.Add(expected);
        var weaver = new ModuleWeaver
        {
            ModuleDefinition = definition
        };
        var actual = weaver.FindResource(@"..\..\ResourceName", "BadPrefix", @"Namespace1\Namespace2", null, null);
        await Assert.That(actual).IsEqualTo(expected);
    }
}