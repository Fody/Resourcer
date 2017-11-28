using System;
using System.IO;
using System.Reflection;

static class Resource
{
    static Assembly assembly;

    static Resource()
    {
        assembly = typeof(Resource).GetTypeInfo().Assembly;
    }

    public static string AsString(string path)
    {
        StreamReader streamReader = null;
        Stream stream = null;
        string value;
        try
        {
            stream = assembly.GetManifestResourceStream(path);
            if (stream == null)
            {
	            var message = string.Concat("Could not find a resource named '", path,"'.");
	            throw new Exception(message);
            }
	        streamReader = new StreamReader(stream);
            value = streamReader.ReadToEnd();
        }
        finally
        {
            streamReader?.Dispose();
            stream?.Dispose();
        }
        return value;
    }

    public static Stream AsStream(string path)
    {
        return assembly.GetManifestResourceStream(path);
    }

    public static StreamWriter AsStreamReader(string path)
    {
        var stream = assembly.GetManifestResourceStream(path);
        if (stream == null)
        {
            return null;
        }
        return new StreamWriter(stream);
    }
}