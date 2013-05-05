using Mono.Cecil;

public static class CecilExtensions
{

    public static bool IsClass(this TypeDefinition x)
    {
        return (x.BaseType != null) && !x.IsEnum && !x.IsInterface;
    }

    public static MethodDefinition Find(this TypeDefinition typeReference, string name, params string[] paramTypes)
    {
        foreach (var method in typeReference.Methods)
        {
            if (method.IsMatch(name, paramTypes))
            {
                return method;
            }
        }
        throw new WeavingException(string.Format("Could not find '{0}' on '{1}'", name, typeReference.Name) );
    }

    public static string GetNamespace(this TypeDefinition typeDefinition)
    {
        if (typeDefinition.IsNested)
        {
            return typeDefinition.DeclaringType.Namespace;
        }
        return typeDefinition.Namespace;
    }

    public static bool IsMatch(this MethodReference methodReference,string name, params string[] paramTypes)
    {
        if (methodReference.Parameters.Count != paramTypes.Length)
        {
            return false;
        }
        if (methodReference.Name != name)
        {
            return false;
        }
        for (var index = 0; index < methodReference.Parameters.Count; index++)
        {
            var parameterDefinition = methodReference.Parameters[index];
            var paramType = paramTypes[index];
            if (parameterDefinition.ParameterType.Name != paramType)
            {
                return false;
            }
        }
        return true;
    }
}