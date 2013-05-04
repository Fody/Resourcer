using System.Linq;
using Mono.Cecil;

public partial class ModuleWeaver
{

    public void FindCoreReferences()
    {
        var assemblyResolver = ModuleDefinition.AssemblyResolver;
        var msCoreLibDefinition = assemblyResolver.Resolve("mscorlib");
        var msCoreTypes = msCoreLibDefinition.MainModule.Types;

        var assemblyDefinition = msCoreTypes.First(x => x.Name == "Assembly");
        GetExecutingAssemblyMethod = ModuleDefinition.Import(assemblyDefinition.Find("GetExecutingAssembly"));
        GetManifestResourceStreamMethod = ModuleDefinition.Import(assemblyDefinition.Find("GetManifestResourceStream","String"));
    }

    public MethodReference GetManifestResourceStreamMethod;

    public MethodReference GetExecutingAssemblyMethod;
}