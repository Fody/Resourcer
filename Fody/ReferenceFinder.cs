using System.Linq;
using Mono.Cecil;

public partial class ModuleWeaver
{

    public void FindCoreReferences()
    {
        var assemblyResolver = ModuleDefinition.AssemblyResolver;
        var msCoreLibDefinition = assemblyResolver.Resolve("mscorlib");
        var msCoreTypes = msCoreLibDefinition.MainModule.Types;

        var textReaderTypeDefinition = msCoreTypes.First(x => x.Name == "TextReader");
        ReadToEndMethod = ModuleDefinition.Import(textReaderTypeDefinition.Find("ReadToEnd"));
        DisposeTextReaderMethod = ModuleDefinition.Import(textReaderTypeDefinition.Find("Dispose"));
        var streamTypeDefinition = msCoreTypes.First(x => x.Name == "Stream");
        DisposeStreamMethod = ModuleDefinition.Import(streamTypeDefinition.Find("Dispose"));
        StreamTypeReference = ModuleDefinition.Import(streamTypeDefinition);
        var streamReaderTypeDefinition = msCoreTypes.First(x => x.Name == "StreamReader");
        StreamReaderTypeReference = ModuleDefinition.Import(streamReaderTypeDefinition);
        StreamReaderConstructorReference = ModuleDefinition.Import(streamReaderTypeDefinition.Find(".ctor","Stream"));
        var assemblyTypeDefinition = msCoreTypes.First(x => x.Name == "Assembly");
        AssemblyTypeReference = ModuleDefinition.Import(assemblyTypeDefinition);
        GetExecutingAssemblyMethod = ModuleDefinition.Import(assemblyTypeDefinition.Find("GetExecutingAssembly"));
        GetManifestResourceStreamMethod = ModuleDefinition.Import(assemblyTypeDefinition.Find("GetManifestResourceStream", "String"));
    }

    public MethodReference DisposeStreamMethod;

    public MethodReference DisposeTextReaderMethod;

    public MethodReference ReadToEndMethod;

    public MethodReference StreamReaderConstructorReference;

    public TypeReference StreamTypeReference;
    public TypeReference StreamReaderTypeReference;

    public TypeReference AssemblyTypeReference;

    public MethodReference GetManifestResourceStreamMethod;

    public MethodReference GetExecutingAssemblyMethod;
}