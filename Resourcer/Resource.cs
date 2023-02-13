using System;
using System.IO;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Resourcer;

/// <summary>
/// Marker class to indicate where to pull in a resource.
/// </summary>
public static class Resource
{
    public static string AsString(string path)
    {
        throw new NotImplementedException();
    }

    public static string AsStringUnChecked(string path)
    {
        throw new NotImplementedException();
    }

    public static Stream AsStream(string path)
    {
        throw new NotImplementedException();
    }

    public static StreamReader AsStreamReader(string path)
    {
        throw new NotImplementedException();
    }

    public static Stream AsStreamUnChecked(string path)
    {
        throw new NotImplementedException();
    }

    public static StreamReader AsStreamReaderUnChecked(string path)
    {
        throw new NotImplementedException();
    }
}