using System.Reflection;
using Resourcer;

public class TemplateClass
{
    public void Foo()
    {
        var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Foo");
    }

    public void Bar()
    {
        var stream = Resource.AsStream("Foo");
    }
}