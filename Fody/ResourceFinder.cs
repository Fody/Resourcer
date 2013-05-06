using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

public static class ResourceFinder
{

	public static Resource FindResource(this ModuleDefinition moduleDefinition, string searchPath, string @namespace, string codeDirPath)
	{
		var resources = moduleDefinition.Resources;

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
		var dirCombine =  Path.GetFullPath(Path.Combine(fakeDrive, codeDirPath, searchPath)).Replace(fakeDrive,string.Empty);

		var suffix = dirCombine.Replace(@"\", ".").Replace(@"\", ".");
		var resourceNameFromDir = Path.GetFileNameWithoutExtension(moduleDefinition.Name) + "." + suffix;
		resource = resources.FirstOrDefault(x => x.Name == resourceNameFromDir);
		if (resource != null)
		{
			return resource;
		}

        var message = string.Format("Could not find a resource.\r\nTried:\r\n'{1}'\r\n'{2}'\r\n'{3}'", searchPath, searchPath, resourceNameFromDir, resourceNameFromDir);
	    throw new WeavingException(message);
	}
}