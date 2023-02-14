#nullable enable
using System;
using System.IO;
using System.Linq;
using Fody;
using Mono.Cecil;
using Mono.Cecil.Cil;

public partial class ModuleWeaver
{
    public void Process(MethodDefinition method)
    {
        var relativePath = FindRelativePath(method);

        try
        {
            var instructions = method.Body.Instructions.Where(_ => _.OpCode == OpCodes.Call).ToList();

            foreach (var instruction in instructions)
            {
                ProcessInstruction(method, instruction, relativePath);
            }
        }
        catch (Exception exception)
        {
            if (exception is WeavingException)
            {
                throw;
            }
            throw new($"Failed to process '{method.FullName}'.", exception);
        }
    }

    string? FindRelativePath(MethodDefinition method)
    {
        foreach (var instruction in method.Body.Instructions)
        {
            var sequencePoint = method.DebugInformation.GetSequencePoint(instruction);
            if (sequencePoint != null)
            {
                var directoryName = Path.GetDirectoryName(sequencePoint.Document.Url);
                return PathEx.MakeRelativePath(ProjectDirectoryPath, directoryName);
            }
        }

        return null;
    }

    void ProcessInstruction(MethodDefinition method, Instruction instruction, string? relativePath)
    {
        if (instruction.Operand is not MemberReference methodReference)
        {
            return;
        }
        if (methodReference.DeclaringType.FullName != "Resourcer.Resource")
        {
            return;
        }

        if (methodReference.Name == "AsStream")
        {
            var resource = FindResource(method, instruction, relativePath);
            if (resource == null)
            {
                return;
            }
            instruction.Previous.Operand = resource.Name;
            instruction.Operand = AsStreamMethod;
            return;
        }
        if (methodReference.Name == "AsStreamUnChecked")
        {
            instruction.Operand = AsStreamMethod;
            return;
        }
        if (methodReference.Name == "AsStreamReader")
        {
            var resource = FindResource(method, instruction, relativePath);
            if (resource == null)
            {
                return;
            }
            instruction.Previous.Operand = resource.Name;
            instruction.Operand = AsStreamReaderMethod;
            return;
        }
        if (methodReference.Name == "AsStreamReaderUnChecked")
        {
            instruction.Operand = AsStreamReaderMethod;
            return;
        }
        if (methodReference.Name == "AsString")
        {
            var resource = FindResource(method, instruction, relativePath);
            if (resource == null)
            {
                return;
            }
            instruction.Previous.Operand = resource.Name;
            instruction.Operand = AsStringMethod;
            return;
        }
        if (methodReference.Name == "AsStringUnChecked")
        {
            instruction.Operand = AsStringMethod;
            return;
        }
        throw new WeavingException($"Unsupported method '{methodReference.FullName}'.");
    }

    Resource? FindResource(MethodDefinition method, Instruction instruction, string? relativePath)
    {
        var stringInstruction = instruction.Previous;
        if (stringInstruction.OpCode != OpCodes.Ldstr)
        {
            throw new WeavingException("Can only be used on string literals");
        }
        var searchPath = (string) stringInstruction.Operand;
        var @namespace = method.DeclaringType.GetNamespace();
        return FindResource(searchPath, @namespace, relativePath, stringInstruction, method);
    }
}