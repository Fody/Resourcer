using System.IO;
using Resourcer;

public class TargetClass
{
    public Stream WithAsStream()
    {
        return Resource.AsStream("PreEmbeddedResource.txt");
    }
    public Stream WithAsStreamUnChecked()
    {
        return Resource.AsStreamUnChecked("fakePath");
    }
    //public string WithAsString()
    //{
    //    return Resource.AsString("PreEmbeddedResource.txt");
    //}
    //public string WithAsStringUnChecked()
    //{
    //    return Resource.AsStringUnChecked("fakePath");
    //}
}