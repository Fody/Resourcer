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

		throw new WeavingException(string.Format("Could not find a resource based on the search path '{0}'.", searchPath));
	}
}