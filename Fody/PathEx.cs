using System;

public class PathEx
{
    public static string MakeRelativePath(string fromPath, string toPath)
    {
        toPath = toPath.TrimEnd('\\', '/');
        fromPath = fromPath.TrimEnd('\\', '/');

        if (toPath.IndexOf(fromPath, StringComparison.OrdinalIgnoreCase) < 0)
        {
            return "";
        }
        var path = toPath.Substring(fromPath.Length).Trim('\\', '/');
        if (path.Length == 0)
        {
            return "";
        }
        return path;
    }
}