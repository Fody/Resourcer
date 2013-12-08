using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

public partial class ModuleWeaver
{

    public void FindCoreReferences()
    {
        var coreTypes = new List<TypeDefinition>();
        AppendTypes("mscorlib", coreTypes);
        AppendTypes("System.IO", coreTypes);
        AppendTypes("System.Runtime", coreTypes);
        AppendTypes("System.Reflection", coreTypes);

        var textReaderTypeDefinition = coreTypes.First(x => x.Name == "TextReader");
        ReadToEndMethod = ModuleDefinition.Import(textReaderTypeDefinition.Find("ReadToEnd"));

        var exceptionTypeDefinition = coreTypes.First(x => x.Name == "Exception");
		ExceptionConstructorReference = ModuleDefinition.Import(exceptionTypeDefinition.Find(".ctor", "String"));

        var stringTypeDefinition = coreTypes.First(x => x.Name == "String");
		ConcatReference = ModuleDefinition.Import(stringTypeDefinition.Find("Concat", "String", "String", "String"));

        DisposeTextReaderMethod = ModuleDefinition.Import(textReaderTypeDefinition.Find("Dispose"));
        var streamTypeDefinition = coreTypes.First(x => x.Name == "Stream");
        DisposeStreamMethod = ModuleDefinition.Import(streamTypeDefinition.Find("Dispose"));
        StreamTypeReference = ModuleDefinition.Import(streamTypeDefinition);
        var streamReaderTypeDefinition = coreTypes.First(x => x.Name == "StreamReader");
        StreamReaderTypeReference = ModuleDefinition.Import(streamReaderTypeDefinition);
        StreamReaderConstructorReference = ModuleDefinition.Import(streamReaderTypeDefinition.Find(".ctor","Stream"));
        var assemblyTypeDefinition = coreTypes.First(x => x.Name == "Assembly");
        AssemblyTypeReference = ModuleDefinition.Import(assemblyTypeDefinition);
        GetExecutingAssemblyMethod = ModuleDefinition.Import(assemblyTypeDefinition.Find("GetExecutingAssembly"));
        GetManifestResourceStreamMethod = ModuleDefinition.Import(assemblyTypeDefinition.Find("GetManifestResourceStream", "String"));
    }

    void AppendTypes(string name, List<TypeDefinition> coreTypes)
    {
        var definition = AssemblyResolver.Resolve(name);
        if (definition != null)
        {
            coreTypes.AddRange(definition.MainModule.Types);
        }
    }

	public MethodReference ConcatReference;
	public MethodReference ExceptionConstructorReference;
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