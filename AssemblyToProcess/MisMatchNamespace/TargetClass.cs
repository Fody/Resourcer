using Resourcer;

namespace AssemblyToProcess.DiffNamespace
{
    public class TargetClass
    {
        public string FullyQualified()
        {
            return Resource.AsString("AssemblyToProcess.MisMatchNamespace.ResourceInMisMatchNamespace.txt");
        }

        public string WithAsString()
        {
            return Resource.AsString("ResourceInMisMatchNamespace.txt");
        }
    }
}