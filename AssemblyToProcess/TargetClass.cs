using System.IO;
using Resourcer;

public class TargetClass
{
    public Stream WithAsStream()
    {
        return Resource.AsStream("PreEmbeddedResource.txt");
    }
}