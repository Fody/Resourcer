using System;
using System.IO;

public class PathEx
{
    public static String MakeRelativePath(String fromPath, String toPath)
    {
        fromPath = fromPath.TrimEnd(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        toPath = toPath.TrimEnd(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        var fromUri = new Uri(fromPath);
        var toUri = new Uri(toPath);

        var relativeUri = fromUri.MakeRelativeUri(toUri);
        var relativePath = Uri.UnescapeDataString(relativeUri.ToString());

        return relativePath.Replace('/', Path.DirectorySeparatorChar);
    }
}