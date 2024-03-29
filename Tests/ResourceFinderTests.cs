﻿using System.IO;
using Mono.Cecil;
using Xunit;

public class ResourceFinderTests
{
    [Fact]
    public void FullyQualified()
    {
        var expected = new EmbeddedResource("AssemblyName.Namespace1.ResourceName", ManifestResourceAttributes.Public, (Stream)null);
        var definition = ModuleDefinition.CreateModule("AssemblyName", ModuleKind.Dll);
        definition.Resources.Add(expected);
        var weaver = new ModuleWeaver
        {
            ModuleDefinition = definition
        };
        var actual = weaver.FindResource("AssemblyName.Namespace1.ResourceName", null, null, null, null);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RelativeBasedOnNamespace()
    {
        var expected = new EmbeddedResource("AssemblyName.Namespace1.ResourceName", ManifestResourceAttributes.Public, (Stream)null);
        var definition = ModuleDefinition.CreateModule("AssemblyName", ModuleKind.Dll);
        definition.Resources.Add(expected);
        var weaver = new ModuleWeaver
        {
            ModuleDefinition = definition
        };
        var actual = weaver.FindResource("ResourceName", "AssemblyName.Namespace1", null, null, null);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RelativeBasedOnDir()
    {
        var expected = new EmbeddedResource("AssemblyName.Namespace1.ResourceName", ManifestResourceAttributes.Public, (Stream)null);
        var definition = ModuleDefinition.CreateModule("AssemblyName", ModuleKind.Dll);
        definition.Resources.Add(expected);
        var weaver = new ModuleWeaver
        {
            ModuleDefinition = definition
        };
        var actual = weaver.FindResource("ResourceName", "BadPrefix", "Namespace1", null, null);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RelativeBasedOnDirUpOneLevel()
    {
        var expected = new EmbeddedResource("AssemblyName.ResourceName", ManifestResourceAttributes.Public, (Stream)null);
        var definition = ModuleDefinition.CreateModule("AssemblyName", ModuleKind.Dll);
        definition.Resources.Add(expected);
        var weaver = new ModuleWeaver
        {
            ModuleDefinition = definition
        };
        var actual = weaver.FindResource(@"..\ResourceName", "BadPrefix", "Namespace1", null, null);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RelativeBasedOnDirUpOneLevelMultipleNamespaces()
    {
        var expected = new EmbeddedResource("AssemblyName.Namespace1.ResourceName", ManifestResourceAttributes.Public, (Stream)null);
        var definition = ModuleDefinition.CreateModule("AssemblyName", ModuleKind.Dll);
        definition.Resources.Add(expected);
        var weaver = new ModuleWeaver
        {
            ModuleDefinition = definition
        };
        var actual = weaver.FindResource(@"..\ResourceName", "BadPrefix", @"Namespace1\Namespace2", null, null);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RelativeBasedOnDirUpTwoLevelsMultipleNamespaces()
    {
        var expected = new EmbeddedResource("AssemblyName.ResourceName", ManifestResourceAttributes.Public, (Stream)null);
        var definition = ModuleDefinition.CreateModule("AssemblyName", ModuleKind.Dll);
        definition.Resources.Add(expected);
        var weaver = new ModuleWeaver
        {
            ModuleDefinition = definition
        };
        var actual = weaver.FindResource(@"..\..\ResourceName", "BadPrefix", @"Namespace1\Namespace2", null, null);
        Assert.Equal(expected, actual);
    }
}