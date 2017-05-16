using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

public partial class ModuleWeaver
{

    public Resource FindResource(string searchPath, string @namespace, string codeDirPath, Instruction instruction, MethodDefinition method)
    {
        var resources = ModuleDefinition.Resources;

        //Fully qualified
        var resource = resources.FirstOrDefault(x => x.Name == searchPath);
        if (resource != null)
        {
            return resource;
        }

        //Relative based on namespace
        var namespaceCombine = Path.Combine(@namespace.Replace(@"\", ".").Replace(@"\", "."), searchPath);
        var resourceNameFromNamespace = namespaceCombine.Replace(@"\", ".").Replace(@"\", ".");
        resource = resources.FirstOrDefault(x => x.Name == resourceNameFromNamespace);
        if (resource != null)
        {
            return resource;
        }

        if (codeDirPath == null)
        {
            throw new WeavingException($"Could not find a relative path for `{method.FullName}`. Note that Resourcer requires debugs symbols to be enabled to derive paths.");
        }
        //Relative based on dir
        var fakeDrive = @"C:\";
        var dirCombine = Path.GetFullPath(Path.Combine(fakeDrive, codeDirPath, searchPath))
            .Replace(fakeDrive, string.Empty);

        var suffix = dirCombine
            .Replace(@"\", ".")
            .Replace(@"\", ".");
        var resourceNameFromDir = $"{Path.GetFileNameWithoutExtension(ModuleDefinition.Name)}.{suffix}";
        resource = resources.FirstOrDefault(x => x.Name == resourceNameFromDir);
        if (resource != null)
        {
            return resource;
        }

        var message = $@"Could not find a resource.
CodeDirPath:'{codeDirPath}'
Tried:
'{searchPath}'
'{resourceNameFromNamespace}'
'{resourceNameFromDir}'
";
        LogErrorPoint(message, instruction.GetPreviousSequencePoint(method));
        return null;
    }
}

