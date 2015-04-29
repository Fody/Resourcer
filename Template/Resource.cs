using System;
using System.IO;
using System.Reflection;

internal static class Resource
{
    static Assembly assembly;

    static Resource()
    {
        assembly = Assembly.GetExecutingAssembly();
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
            if (streamReader != null)
            {
                streamReader.Dispose();
            }
            if (stream != null)
            {
                stream.Dispose();
            }
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