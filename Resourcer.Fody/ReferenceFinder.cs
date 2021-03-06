﻿using Mono.Cecil;

public partial class ModuleWeaver
{
    public void FindCoreReferences()
    {
        var textReaderTypeDefinition = FindTypeDefinition("System.IO.TextReader");
        ReadToEndMethod = ModuleDefinition.ImportReference(textReaderTypeDefinition.Find("ReadToEnd"));

        var exceptionTypeDefinition = FindTypeDefinition("System.Exception");
        ExceptionConstructorReference = ModuleDefinition.ImportReference(exceptionTypeDefinition.Find(".ctor", "String"));

        ConcatReference = ModuleDefinition.ImportReference(TypeSystem.StringDefinition.Find("Concat", "String", "String", "String"));

        DisposeTextReaderMethod = ModuleDefinition.ImportReference(textReaderTypeDefinition.Find("Dispose"));
        var streamTypeDefinition = FindTypeDefinition("System.IO.Stream");
        DisposeStreamMethod = ModuleDefinition.ImportReference(streamTypeDefinition.Find("Dispose"));
        StreamTypeReference = ModuleDefinition.ImportReference(streamTypeDefinition);
        var streamReaderTypeDefinition = FindTypeDefinition("System.IO.StreamReader");
        StreamReaderTypeReference = ModuleDefinition.ImportReference(streamReaderTypeDefinition);
        StreamReaderConstructorReference = ModuleDefinition.ImportReference(streamReaderTypeDefinition.Find(".ctor", "Stream"));
        var assemblyTypeDefinition = FindTypeDefinition("System.Reflection.Assembly");
        AssemblyTypeReference = ModuleDefinition.ImportReference(assemblyTypeDefinition);

        var typeType = FindTypeDefinition("System.Type");
        GetTypeFromHandle = ModuleDefinition.ImportReference(typeType.Find("GetTypeFromHandle", "RuntimeTypeHandle"));

        var introspectionExtensionsType = FindTypeDefinition("System.Reflection.IntrospectionExtensions");
        GetTypeInfo = ModuleDefinition.ImportReference(introspectionExtensionsType.Find("GetTypeInfo", "Type"));

        var typeInfoType = FindTypeDefinition("System.Reflection.TypeInfo");
        GetAssembly = ModuleDefinition.ImportReference(typeInfoType.Find("get_Assembly"));

        GetManifestResourceStreamMethod = ModuleDefinition.ImportReference(assemblyTypeDefinition.Find("GetManifestResourceStream", "String"));
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