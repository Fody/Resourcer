using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

public partial class ModuleWeaver
{

	public Resource FindResource(string searchPath, string @namespace, string codeDirPath, Instruction instruction)
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

		//Relative based on dir
		var fakeDrive = @"C:\";
		var dirCombine =  Path.GetFullPath(Path.Combine(fakeDrive, codeDirPath, searchPath))
            .Replace(fakeDrive,string.Empty);

		var suffix = dirCombine
            .Replace(@"\", ".")
            .Replace(@"\", ".");
        var resourceNameFromDir = Path.GetFileNameWithoutExtension(ModuleDefinition.Name) + "." + suffix;
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
	    LogErrorPoint(message, instruction.GetPreviousSequencePoint());
	    return null;
	}
}