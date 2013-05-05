using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

public class MethodProcessor
{
    public MethodDefinition Method;
    public Action<string> LogInfo;
    public Action<string> LogError;
    bool foundUsageInMethod;
    ILProcessor ilProcessor;
    public ModuleWeaver ModuleWeaver;
    string relativeCodeDirPath;

    public void Process()
    {
        try
        {
            ilProcessor = Method.Body.GetILProcessor();
            var instructions = Method.Body.Instructions.Where(x => x.OpCode == OpCodes.Call).ToList();

            foreach (var instruction in instructions)
            {
                ProcessInstruction(instruction);
            }
            if (foundUsageInMethod)
            {
                Method.Body.OptimizeMacros();
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
        SimplifyIfFirst();

        instruction.OpCode = OpCodes.Callvirt;

        if (methodReference.Name == "AsStream")
        {
            var stringInstruction = instruction.Previous;
            var searchPath = FindSearchPath(stringInstruction);
            var resource = FindResource(searchPath);
            stringInstruction.Operand = resource.Name;
            var getAssembly = Instruction.Create(OpCodes.Call, ModuleWeaver.GetExecutingAssemblyMethod);
            ilProcessor.InsertBefore(stringInstruction, getAssembly);
            instruction.Operand = ModuleWeaver.GetManifestResourceStreamMethod;
            return;
        }
        if (methodReference.Name == "AsStreamUnChecked")
        {
            var stringInstruction = instruction.Previous;
            var getAssembly = Instruction.Create(OpCodes.Call, ModuleWeaver.GetExecutingAssemblyMethod);
            ilProcessor.InsertBefore(stringInstruction, getAssembly);
            instruction.Operand = ModuleWeaver.GetManifestResourceStreamMethod;
            return;
        }
        if (methodReference.Name == "AsString")
        {
            //EnsureIsStringLiteral(instruction);
            //var getAssembly = Instruction.Create(OpCodes.Call, ModuleWeaver.GetExecutingAssemblyMethod);
            //ilProcessor.InsertBefore(stringInstruction,getAssembly);
            //instruction.Operand = ModuleWeaver.GetManifestResourceStreamMethod;
            return;
        }
        throw new WeavingException(string.Format("Unsupported method '{0}'.", methodReference.FullName));
    }

    void SimplifyIfFirst()
    {
        if (foundUsageInMethod)
        {
            return;
        } 
        foreach (var instruction1 in Method.Body.Instructions)
        {
            if (instruction1.SequencePoint != null)
            {
                var directoryName = Path.GetDirectoryName(instruction1.SequencePoint.Document.Url);
                relativeCodeDirPath = PathEx.MakeRelativePath(ModuleWeaver.ProjectDirectoryPath, directoryName);
                break;
            }
        }
        Method.Body.InitLocals = true;
        Method.Body.SimplifyMacros();
        foundUsageInMethod = true;
    }

    string FindSearchPath(Instruction instruction)
    {
        if (instruction.OpCode != OpCodes.Ldstr)
        {
            //TODO:
            throw new WeavingException("Can only be used on string literals");
        }
        return (string) instruction.Operand;
    }

    Resource FindResource(string searchPath)
    {
        var resources = ModuleWeaver.ModuleDefinition.Resources;
        var @namespace = Method.DeclaringType.GetNamespace();

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
        var dirCombine = Path.Combine(relativeCodeDirPath, searchPath);

        var suffix = dirCombine.Replace(@"\", ".").Replace(@"\", ".");
        var resourceNameFromDir = Path.GetFileNameWithoutExtension(ModuleWeaver.ModuleDefinition.Name) +"." + suffix;
        resource = resources.FirstOrDefault(x => x.Name == resourceNameFromDir);
        if (resource != null)
        {
            return resource;
        }

        throw new WeavingException(string.Format("Could not find a resource based on the search path '{0}'.", searchPath));
    }
}

public class PathEx
{
    public static String MakeRelativePath(String fromPath, String toPath)
    {
        var fromUri = new Uri(fromPath);
        var toUri = new Uri(toPath);

        var relativeUri = fromUri.MakeRelativeUri(toUri);
        var relativePath = Uri.UnescapeDataString(relativeUri.ToString());

        return relativePath.Replace('/', Path.DirectorySeparatorChar);
    }
}