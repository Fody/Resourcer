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
        ReadToEndMethod = ModuleDefinition.ImportReference(textReaderTypeDefinition.Find("ReadToEnd"));

        var exceptionTypeDefinition = coreTypes.First(x => x.Name == "Exception");
        ExceptionConstructorReference = ModuleDefinition.ImportReference(exceptionTypeDefinition.Find(".ctor", "String"));

        var stringTypeDefinition = coreTypes.First(x => x.Name == "String");
        ConcatReference = ModuleDefinition.ImportReference(stringTypeDefinition.Find("Concat", "String", "String", "String"));

        DisposeTextReaderMethod = ModuleDefinition.ImportReference(textReaderTypeDefinition.Find("Dispose"));
        var streamTypeDefinition = coreTypes.First(x => x.Name == "Stream");
        DisposeStreamMethod = ModuleDefinition.ImportReference(streamTypeDefinition.Find("Dispose"));
        StreamTypeReference = ModuleDefinition.ImportReference(streamTypeDefinition);
        var streamReaderTypeDefinition = coreTypes.First(x => x.Name == "StreamReader");
        StreamReaderTypeReference = ModuleDefinition.ImportReference(streamReaderTypeDefinition);
        StreamReaderConstructorReference = ModuleDefinition.ImportReference(streamReaderTypeDefinition.Find(".ctor", "Stream"));
        var assemblyTypeDefinition = coreTypes.First(x => x.Name == "Assembly");
        AssemblyTypeReference = ModuleDefinition.ImportReference(assemblyTypeDefinition);

        var typeType = coreTypes.First(x => x.Name == "Type");
        GetTypeFromHandle = ModuleDefinition.ImportReference(typeType.Find("GetTypeFromHandle", "RuntimeTypeHandle"));

        var introspectionExtensionsType = coreTypes.First(x => x.Name == "IntrospectionExtensions");
        GetTypeInfo = ModuleDefinition.ImportReference(introspectionExtensionsType.Find("GetTypeInfo", "Type"));

        var typeInfoType = coreTypes.First(x => x.Name == "TypeInfo");
        GetAssembly = ModuleDefinition.ImportReference(typeInfoType.Find("get_Assembly"));

        GetManifestResourceStreamMethod = ModuleDefinition.ImportReference(assemblyTypeDefinition.Find("GetManifestResourceStream", "String"));
    }

    void AppendTypes(string name, List<TypeDefinition> coreTypes)
    {
        var definition = AssemblyResolver.Resolve(new AssemblyNameReference(name, null));
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
    public MethodReference GetTypeFromHandle;
    public MethodReference GetTypeInfo;
    public MethodReference GetAssembly;
}