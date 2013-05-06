using System.IO;
using Resourcer;

public class TargetClass
{
    public Stream WithAsStream()
    {
        return Resource.AsStream("EmbeddedResource.txt");
    }
    public Stream WithAsStreamUnChecked(string path)
    {
		return Resource.AsStreamUnChecked(path);
    }
    public string WithAsString()
    {
        return Resource.AsString("EmbeddedResource.txt");
    }
	public string WithAsStringUnChecked(string path)
    {
        return Resource.AsStringUnChecked(path);
    }
}