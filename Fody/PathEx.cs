using System;
using System.IO;

public class PathEx
{
    public static String MakeRelativePath(String fromPath, String toPath)
    {
        toPath = toPath.TrimEnd('\\', '/');
        fromPath = fromPath.TrimEnd('\\', '/');

        var path = toPath.Substring(fromPath.Length).Trim('\\','/');
        if (path.Length == 0)
        {
            return Path.DirectorySeparatorChar.ToString();
        }
        return path;
    }
}