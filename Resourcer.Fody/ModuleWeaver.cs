﻿using System.Collections.Generic;
using System.Linq;
using Fody;

public partial class ModuleWeaver : BaseModuleWeaver
{
    public override void Execute()
    {
        FindCoreReferences();
        InjectHelper();
        foreach (var type in ModuleDefinition
                     .GetTypes()
                     .Where(_ => _.IsClass()))
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
    }

    public override bool ShouldCleanReference => true;

    public override IEnumerable<string> GetAssembliesForScanning()
    {
        yield return "mscorlib";
        yield return "System.IO";
        yield return "System.Runtime";
        yield return "System.Reflection";
        yield return "netstandard";
    }
}