using Resourcer;

namespace AssemblyToProcess.CustomNamespace
{
	public class TargetClass
	{
        public string FullyQualified()
        {
            return Resource.AsString("AssemblyToProcess.CustomNamespace.ResourceInCustomNamespace.txt");
        }
		public string WithAsString()
		{
            return Resource.AsString("ResourceInCustomNamespace.txt");
		}
	}
}