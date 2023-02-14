#nullable enable
using System.IO;
using System.Linq;
using System.Text;
using Fody;
using Mono.Cecil;
using Mono.Cecil.Cil;

public partial class ModuleWeaver
{
    public Resource? FindResource(string searchPath, string @namespace, string? codeDirPath, Instruction instruction, MethodDefinition method)
    {
        var resources = ModuleDefinition.Resources;

        //Fully qualified
        var resource = resources.FirstOrDefault(_ => _.Name == searchPath);
        if (resource != null)
        {
            return resource;
        }

        //Relative based on namespace
        var namespaceCombine = Path.Combine(@namespace.Replace("/", ".").Replace(@"\", "."), searchPath);
        var resourceNameFromNamespace = namespaceCombine.Replace("/", ".").Replace(@"\", ".");
        resource = resources.FirstOrDefault(_ => _.Name == resourceNameFromNamespace);

        if (resource != null)
        {
            return resource;
        }

        if (codeDirPath == null)
        {
            throw new WeavingException($"Could not find a relative path for `{method.FullName}`. Note that Resourcer requires debugs symbols to be enabled to derive paths.");
        }

        //Relative based on dir
        var combine = Path.Combine(codeDirPath, searchPath);
        var dirCombine = Path.GetFullPath(combine)
            .Replace(Directory.GetCurrentDirectory(), "");

        var suffix = dirCombine
            .TrimStart(Path.DirectorySeparatorChar)
            .Replace(Path.DirectorySeparatorChar, '.');
        var resourceNameFromDir = $"{Path.GetFileNameWithoutExtension(ModuleDefinition.Name)}.{suffix}";
        resource = resources.FirstOrDefault(_ => _.Name == resourceNameFromDir);
        if (resource != null)
        {
            return resource;
        }

        var message = new StringBuilder($"""
            Could not find a resource.
            CodeDirPath:'{codeDirPath}'
            Tried:
              * Search path:'{searchPath}'
              * Resource name from namespace:'{resourceNameFromNamespace}'
              * Resource name from directory:'{resourceNameFromDir}'
            Resources:

            """);
        foreach (var item in resources)
        {
            message.AppendLine($"  * {item.Name}");
        }

        WriteError(message.ToString(), instruction.GetPreviousSequencePoint(method));
        return null;
    }
}