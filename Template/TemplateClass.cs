
#pragma warning disable 168
using System.Reflection;

public class TemplateClass
{
    public void AsStreamAfter()
    {
        var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Foo");
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
#pragma warning restore 168
