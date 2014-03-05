using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

public class MethodProcessor
{
	public MethodDefinition Method;
	public Action<string> LogError;
	public ModuleWeaver ModuleWeaver;
	string relativeCodeDirPath;

	public void Process()
	{
		FindRelativePath();

		try
		{
			var instructions = Method.Body.Instructions.Where(x => x.OpCode == OpCodes.Call).ToList();

			foreach (var instruction in instructions)
			{
				ProcessInstruction(instruction);
			}
		}
		catch (Exception exception)
		{
			if (exception is WeavingException)
			{
				throw;
			}
			throw new Exception(string.Format("Failed to process '{0}'.", Method.FullName), exception);
		}
	}

	void FindRelativePath()
	{
		foreach (var instruction1 in Method.Body.Instructions)
		{
			if (instruction1.SequencePoint != null)
			{
				var directoryName = Path.GetDirectoryName(instruction1.SequencePoint.Document.Url);
				relativeCodeDirPath = PathEx.MakeRelativePath(ModuleWeaver.ProjectDirectoryPath, directoryName);
				break;
			}
		}
	}

	void ProcessInstruction(Instruction instruction)
	{
		var methodReference = instruction.Operand as MemberReference;
		if (methodReference == null)
		{
			return;
		}
		if (methodReference.DeclaringType.FullName != "Resourcer.Resource")
		{
			return;
		}

		if (methodReference.Name == "AsStream")
		{
			var resource = FindResource(instruction);
		    instruction.Previous.Operand = resource.Name;
			instruction.Operand = ModuleWeaver.AsStreamMethod;
			return;
		}
		if (methodReference.Name == "AsStreamUnChecked")
		{
			instruction.Operand = ModuleWeaver.AsStreamMethod;
			return;
		}
		if (methodReference.Name == "AsString")
        {
            var resource = FindResource(instruction);
            instruction.Previous.Operand = resource.Name;
			instruction.Operand = ModuleWeaver.AsStringMethod;
			return;
		}
		if (methodReference.Name == "AsStringUnChecked")
		{
			instruction.Operand = ModuleWeaver.AsStringMethod;
			return;
		}
		throw new WeavingException(string.Format("Unsupported method '{0}'.", methodReference.FullName));
	}

    Resource FindResource(Instruction instruction)
    {
        var stringInstruction = instruction.Previous;
        if (stringInstruction.OpCode != OpCodes.Ldstr)
        {
            //TODO:
            throw new WeavingException("Can only be used on string literals");
        }
        var searchPath = (string) stringInstruction.Operand;
        return ModuleWeaver.ModuleDefinition.FindResource(searchPath, Method.DeclaringType.GetNamespace(), relativeCodeDirPath, stringInstruction);
    }
}