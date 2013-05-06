using System;
using System.IO;

public class PathEx
{
    public static String MakeRelativePath(String fromPath, String toPath)
    {
        var fromUri = new Uri(fromPath);
        var toUri = new Uri(toPath);

        var relativeUri = fromUri.MakeRelativeUri(toUri);
        var relativePath = Uri.UnescapeDataString(relativeUri.ToString());

        return relativePath.Replace('/', Path.DirectorySeparatorChar);
    }
}