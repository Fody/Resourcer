using System.IO;
using Resourcer;

public class TargetClass
{
    public Stream WithAsStream()
    {
        return Resource.AsStream("Resource.txt");
    }
    public Stream WithAsStreamUnChecked(string path)
    {
		return Resource.AsStreamUnChecked(path);
    }
    public StreamReader WithAsStreamReader()
    {
        return Resource.AsStreamReader("Resource.txt");
    }
    public StreamReader WithAsStreamReaderUnChecked(string path)
    {
        return Resource.AsStreamReaderUnChecked(path);
    }
    public string WithAsString()
    {
        return Resource.AsString("Resource.txt");
    }
    public string FullyQualified()
    {
        return Resource.AsString("AssemblyToProcess.Resource.txt");
    }
	public string WithAsStringUnChecked(string path)
    {
        return Resource.AsStringUnChecked(path);
    }
}