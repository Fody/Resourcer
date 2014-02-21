using System;
using System.IO;

public class PathEx
{
    public static string MakeRelativePath(String fromPath, String toPath)
    {
        toPath = toPath.TrimEnd('\\', '/');
        fromPath = fromPath.TrimEnd('\\', '/');

        if (!toPath.Contains(fromPath))
        {
            return "";
        }
        var path = toPath.Substring(fromPath.Length).Trim('\\', '/');
        if (path.Length == 0)
        {
            return Path.DirectorySeparatorChar.ToString();
        }
        return path;
    }
}