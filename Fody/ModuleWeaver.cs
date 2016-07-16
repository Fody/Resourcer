using System;
using System.Diagnostics;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

public partial class ModuleWeaver
{
    public Action<string> LogInfo;
    public Action<string, SequencePoint> LogErrorPoint;
    public ModuleDefinition ModuleDefinition;
    public IAssemblyResolver AssemblyResolver;
    public string ProjectDirectoryPath;

    public ModuleWeaver()
    {
        LogInfo = s => { };
        LogErrorPoint = (s,p) => { };
    }

    public void Execute()
    {
        FindCoreReferences();
        InjectHelper();
        foreach (var type in ModuleDefinition
            .GetTypes()
            .Where(x => x.IsClass()))
        {

            foreach (var method in type.Methods)
            {
                //skip for abstract and delegates
                if (!method.HasBody)
                {
                    continue;
                }
                Process(method);
            }
        }
        CleanReferences();
    }

}