﻿#pragma warning disable 168
// ReSharper disable UnusedVariable

public class TemplateClass
{
    public void AsStreamAfter()
    {
        var assembly = typeof(TemplateClass).Assembly;
        var stream = assembly.GetManifestResourceStream("Foo");
    }

    public void AsStreamBefore()
    {
        var stream = Resource.AsStream("Foo");
    }

    public void AsStringBefore()
    {
        var @string = Resource.AsString("Foo");
    }
}