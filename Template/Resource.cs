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
}