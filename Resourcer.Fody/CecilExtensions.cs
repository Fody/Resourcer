using System.Collections.Generic;
using Fody;
using Mono.Cecil;
using Mono.Cecil.Cil;

public static class CecilExtensions
{
    public static bool IsClass(this TypeDefinition type)
    {
        return type.BaseType != null &&
               !type.IsEnum &&
               !type.IsInterface;
    }

    public static SequencePoint GetPreviousSequencePoint(this Instruction instruction, MethodDefinition method)
    {
        while (true)
        {
            var sequencePoint = method.DebugInformation.GetSequencePoint(instruction);
            if (sequencePoint != null)
            {
                return sequencePoint;
            }

            instruction = instruction.Previous;
            if (instruction == null)
            {
                return null;
            }
        }
    }

    public static MethodDefinition Find(this TypeDefinition type, string name, params string[] paramTypes)
    {
        foreach (var method in type.AllMethods())
        {
            if (method.IsMatch(name, paramTypes))
            {
                return method;
            }
        }

        throw new WeavingException($"Could not find '{name}' on '{type.Name}'");
    }

    public static IEnumerable<MethodDefinition> AllMethods(this TypeDefinition type)
    {
        while (true)
        {
            if (type.Name == "Object")
            {
                break;
            }

            foreach (var method in type.Methods)
            {
                yield return method;
            }

            type = type.BaseType.Resolve();
        }
    }

    public static string GetNamespace(this TypeDefinition type)
    {
        if (type.IsNested)
        {
            return type.DeclaringType.Namespace;
        }

        return type.Namespace;
    }

    public static bool IsMatch(this MethodReference method, string name, params string[] paramTypes)
    {
        if (method.Parameters.Count != paramTypes.Length)
        {
            return false;
        }

        if (method.Name != name)
        {
            return false;
        }

        for (var index = 0; index < method.Parameters.Count; index++)
        {
            var parameterDefinition = method.Parameters[index];
            var paramType = paramTypes[index];
            if (parameterDefinition.ParameterType.Name != paramType)
            {
                return false;
            }
        }

        return true;
    }
}