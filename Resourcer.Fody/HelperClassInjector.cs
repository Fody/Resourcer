using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

public partial class ModuleWeaver
{
    public MethodDefinition AsStringMethod;
    public MethodDefinition AsStreamMethod;
    public MethodDefinition AsStreamReaderMethod;

    MethodAttributes staticMethodAttributes =
        MethodAttributes.Public |
        MethodAttributes.HideBySig |
        MethodAttributes.Static;

    void InjectHelper()
    {
        var typeAttributes = TypeAttributes.AnsiClass | TypeAttributes.Sealed | TypeAttributes.Abstract | TypeAttributes.AutoClass;
        var targetType = new TypeDefinition(ModuleDefinition.Name + ".Resourcer", "ResourceHelper", typeAttributes, ModuleDefinition.TypeSystem.Object);
        ModuleDefinition.Types.Add(targetType);
        var fieldDefinition = new FieldDefinition("assembly", FieldAttributes.Static | FieldAttributes.Private, AssemblyTypeReference)
        {
            DeclaringType = targetType
        };
        targetType.Fields.Add(fieldDefinition);
        InjectConstructor(targetType, fieldDefinition);

        InjectAsStream(targetType, fieldDefinition);
        InjectAsStreamReader(targetType, fieldDefinition);
        InjectAsString(targetType, fieldDefinition);
    }

    void InjectAsStream(TypeDefinition targetType, FieldDefinition fieldDefinition)
    {
        AsStreamMethod = new MethodDefinition("AsStream", staticMethodAttributes, StreamTypeReference);
        var pathParam = new ParameterDefinition(ModuleDefinition.TypeSystem.String);
        AsStreamMethod.Parameters.Add(pathParam);
        AsStreamMethod.Body.InitLocals = true;
        var inst = AsStreamMethod.Body.Instructions;
        inst.Add(Instruction.Create(OpCodes.Ldsfld, fieldDefinition));
        inst.Add(Instruction.Create(OpCodes.Ldarg, pathParam));
        inst.Add(Instruction.Create(OpCodes.Callvirt, GetManifestResourceStreamMethod));
        inst.Add(Instruction.Create(OpCodes.Ret));
        targetType.Methods.Add(AsStreamMethod);
    }

    void InjectAsStreamReader(TypeDefinition targetType, FieldDefinition fieldDefinition)
    {
        AsStreamReaderMethod = new MethodDefinition("AsStreamReader", staticMethodAttributes, StreamReaderTypeReference);
        var streamVariable = new VariableDefinition(StreamTypeReference);
        AsStreamReaderMethod.Body.Variables.Add(streamVariable);
        var pathParam = new ParameterDefinition(ModuleDefinition.TypeSystem.String);
        AsStreamReaderMethod.Parameters.Add(pathParam);
        AsStreamReaderMethod.Body.InitLocals = true;
        var inst = AsStreamReaderMethod.Body.Instructions;

        var skipReturn = Instruction.Create(OpCodes.Nop);

        inst.Add(Instruction.Create(OpCodes.Ldsfld, fieldDefinition));
        inst.Add(Instruction.Create(OpCodes.Ldarg, pathParam));
        inst.Add(Instruction.Create(OpCodes.Callvirt, GetManifestResourceStreamMethod));

        inst.Add(Instruction.Create(OpCodes.Stloc, streamVariable));
        inst.Add(Instruction.Create(OpCodes.Ldloc, streamVariable));
        inst.Add(Instruction.Create(OpCodes.Brtrue_S, skipReturn));

        inst.Add(Instruction.Create(OpCodes.Ldnull));
        inst.Add(Instruction.Create(OpCodes.Ret));
        inst.Add(skipReturn);

        inst.Add(Instruction.Create(OpCodes.Ldloc, streamVariable));
        inst.Add(Instruction.Create(OpCodes.Newobj, StreamReaderConstructorReference));
        inst.Add(Instruction.Create(OpCodes.Ret));
        targetType.Methods.Add(AsStreamReaderMethod);
    }

    void InjectAsString(TypeDefinition targetType, FieldDefinition assemblyField)
    {
        AsStringMethod = new MethodDefinition("AsString", staticMethodAttributes, ModuleDefinition.TypeSystem.String);
        var pathParam = new ParameterDefinition(ModuleDefinition.TypeSystem.String);
        AsStringMethod.Parameters.Add(pathParam);

        AsStringMethod.Body.InitLocals = true;
        var readerVar = new VariableDefinition(StreamReaderTypeReference);
        AsStringMethod.Body.Variables.Add(readerVar);
        var streamVar = new VariableDefinition(StreamTypeReference);
        AsStringMethod.Body.Variables.Add(streamVar);
        var stringVar = new VariableDefinition(ModuleDefinition.TypeSystem.String);
        AsStringMethod.Body.Variables.Add(stringVar);

        var inst = AsStringMethod.Body.Instructions;

        //24
        var assignStreamBeforeReaderConstr = Instruction.Create(OpCodes.Ldloc, streamVar);
        //47
        var assignStringBeforeReturn = Instruction.Create(OpCodes.Ldloc, stringVar);
        //3d
        var assignStreamBeforeDispose = Instruction.Create(OpCodes.Ldloc, streamVar);
        //46
        var endFinally = Instruction.Create(OpCodes.Endfinally);


        inst.Add(Instruction.Create(OpCodes.Ldnull));
        inst.Add(Instruction.Create(OpCodes.Stloc, readerVar));
        inst.Add(Instruction.Create(OpCodes.Ldnull));
        inst.Add(Instruction.Create(OpCodes.Stloc, streamVar));
        var assignAssemblyField = Instruction.Create(OpCodes.Ldsfld, assemblyField);
        inst.Add(assignAssemblyField);
        inst.Add(Instruction.Create(OpCodes.Ldarg, pathParam));
        inst.Add(Instruction.Create(OpCodes.Callvirt, GetManifestResourceStreamMethod));
        inst.Add(Instruction.Create(OpCodes.Stloc, streamVar));
        inst.Add(Instruction.Create(OpCodes.Ldloc, streamVar));
        inst.Add(Instruction.Create(OpCodes.Brtrue_S, assignStreamBeforeReaderConstr));
        inst.Add(Instruction.Create(OpCodes.Ldstr, "Could not find a resource named '"));
        inst.Add(Instruction.Create(OpCodes.Ldarg, pathParam));
        inst.Add(Instruction.Create(OpCodes.Ldstr, "'."));
        inst.Add(Instruction.Create(OpCodes.Call, ConcatReference));
        inst.Add(Instruction.Create(OpCodes.Newobj, ExceptionConstructorReference));
        inst.Add(Instruction.Create(OpCodes.Throw));

        inst.Add(assignStreamBeforeReaderConstr);
        inst.Add(Instruction.Create(OpCodes.Newobj, StreamReaderConstructorReference));
        inst.Add(Instruction.Create(OpCodes.Stloc, readerVar));
        inst.Add(Instruction.Create(OpCodes.Ldloc, readerVar));
        inst.Add(Instruction.Create(OpCodes.Callvirt, ReadToEndMethod));
        inst.Add(Instruction.Create(OpCodes.Stloc, stringVar));
        inst.Add(Instruction.Create(OpCodes.Leave_S, assignStringBeforeReturn));
        var assignReaderBeforeNullCheck = Instruction.Create(OpCodes.Ldloc, readerVar);
        inst.Add(assignReaderBeforeNullCheck);
        inst.Add(Instruction.Create(OpCodes.Brfalse_S, assignStreamBeforeDispose));
        inst.Add(Instruction.Create(OpCodes.Ldloc, readerVar));
        inst.Add(Instruction.Create(OpCodes.Callvirt, DisposeTextReaderMethod));
        inst.Add(assignStreamBeforeDispose);
        inst.Add(Instruction.Create(OpCodes.Brfalse_S, endFinally));
        inst.Add(Instruction.Create(OpCodes.Ldloc, streamVar));
        inst.Add(Instruction.Create(OpCodes.Callvirt, DisposeStreamMethod));
        inst.Add(endFinally);
        inst.Add(assignStringBeforeReturn);
        inst.Add(Instruction.Create(OpCodes.Ret));

        var finallyHandler = new ExceptionHandler(ExceptionHandlerType.Finally)
        {
            TryStart = assignAssemblyField,
            TryEnd = assignReaderBeforeNullCheck,
            HandlerStart = assignReaderBeforeNullCheck,
            HandlerEnd = assignStringBeforeReturn
        };
        AsStringMethod.Body.ExceptionHandlers.Add(finallyHandler);
        AsStringMethod.Body.SimplifyMacros();
        targetType.Methods.Add(AsStringMethod);
    }

    void InjectConstructor(TypeDefinition targetType, FieldDefinition fieldDefinition)
    {
        const MethodAttributes attributes = MethodAttributes.Static
                                            | MethodAttributes.SpecialName
                                            | MethodAttributes.RTSpecialName
                                            | MethodAttributes.HideBySig
                                            | MethodAttributes.Private;
        var staticConstructor = new MethodDefinition(".cctor", attributes, ModuleDefinition.TypeSystem.Void);
        targetType.Methods.Add(staticConstructor);
        var instructions = staticConstructor.Body.Instructions;
        instructions.Add(Instruction.Create(OpCodes.Ldtoken, targetType));
        instructions.Add(Instruction.Create(OpCodes.Call, GetTypeFromHandle));
        instructions.Add(Instruction.Create(OpCodes.Call, GetTypeInfo));
        instructions.Add(Instruction.Create(OpCodes.Callvirt, GetAssembly));
        instructions.Add(Instruction.Create(OpCodes.Stsfld, fieldDefinition));
        instructions.Add(Instruction.Create(OpCodes.Ret));
    }
}